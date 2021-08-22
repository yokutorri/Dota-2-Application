using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Dota_2_Console_App.Classes;
using System.Linq;

namespace Dota_2_Console_App.Requests
{
    public static class OpenDotaAPI
    {
        public static Player GetPlayer(string playerID)
        {
            string result = RequestHandler.GET($"https://api.opendota.com/api/players/{playerID}");

            if (result != null)
                return JsonConvert.DeserializeObject<Player>(result);
            else
                return null;
        }

        private static IEnumerable<string> GetFullSubset(IDictionary<string, string[]> lookup, string[] subset, int iteration = 0)
        {
            if (iteration >= 4)
                return subset;

            subset = subset.Concat(subset.SelectMany(element => lookup[element])).Distinct().ToArray();
            return GetFullSubset(lookup, subset, iteration + 1);
        }
        public static Match[] GetPlayerMatches(string playerID, int limit, int days, int lobbyType)
        {
            string requestString = $@"https://api.opendota.com/api/players/{playerID}/matches?limit={limit}";

            if (days > 0)
                requestString += $@"&date={days}";
            if (lobbyType > 0)
                requestString += $@"&lobby_type={lobbyType}";

            requestString += $@"&project[]=hero_id&project[]=kills&project[]=deaths&project[]=assists&project[]=xp_per_min&project[]=gold_per_min&project[]
                                =hero_damage&project[]=tower_damage&project[]=hero_healing&project[]=last_hits";
            string result = RequestHandler.GET(requestString);

            if (result != null)
                return JsonConvert.DeserializeObject<Match[]>(result);
            else
                return null;
        }

        public static string GetLastLobby(string filePath)
        {
            var ServerLog = new List<string>();

            using (var fs = OpenStream(filePath, FileAccess.Read, FileShare.Read, 3))
            using (StreamReader sr = new StreamReader(fs, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    ServerLog.Add(sr.ReadLine());
                }
            }

            return ServerLog.Last(x => x.Contains("Lobby"));
        }

        static private FileStream OpenStream(String fileName, FileAccess fileAccess, FileShare fileShare, int retryCount)
        {
            FileStream fs = null;
            for (int i = 1; i <= 3; ++i)
            {
                try
                {
                    fs = new FileStream(fileName, FileMode.Open, fileAccess, fileShare);
                    break;
                }
                catch (Exception e)
                {
                    if (i == 3)
                        throw e;

                    Thread.Sleep(200);
                }
            }

            if (fs.Length > 0)
            {
                return fs;
            }
            if (retryCount > 0)
            {
                Thread.Sleep(50);
                return OpenStream(fileName, fileAccess, fileShare, retryCount--);
            }
            else
                throw new Exception("File can't be read");
        }
        
        public static List<string> GetPlayerIDs()
        {
            var GameInfo = GetLastLobby(File.ReadLines("path.cfg").First() + "\\game\\dota\\server_log.txt");

            var playerStartIndex = GameInfo.IndexOf('(') + 1;
            var playerEndIndex = GameInfo.IndexOf(')');
            var PlayerSection = GameInfo.Substring(playerStartIndex, playerEndIndex - playerStartIndex);

            var Players = PlayerSection.Split(' ').Where(x => x.Contains("[U:")).Take(10).ToList();

            var Results = new List<string>();

            foreach (var item in Players)
            {
                var startIndex = item.LastIndexOf(':') + 1;
                var endIndex = item.IndexOf(']');
                var length = endIndex - startIndex;

                Results.Add(item.Substring(startIndex, length));
            }

            return Results;
        }
    }
}
