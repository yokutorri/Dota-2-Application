using Dota_2_Console_App.Requests;
using Dota_2_Console_App.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Dota_2_Console_App
{
    class Program
    {
        public static List<PlayerDisplay> PlayerDisplays { get; private set; }

        static void Main(string[] args)
        {
            int[] colors = new int[10] { 10, 10, 10, 10, 10, 4, 4, 4, 4, 4 };

            Console.BufferHeight = 120;
            Console.BufferWidth = 700;

            StartUpUtils.StartUp();

            Console.WriteLine("Press enter for receive data or input exit...");

            while (Console.ReadLine().ToLower() != "exit")
            {
                Console.Clear();
                Console.WriteLine("Receiving data...");

                PlayerDisplays = new List<PlayerDisplay>();

                for (int i = 0; i < 10; i++)
                {
                    var playerDisplay = new PlayerDisplay();
                    PlayerDisplays.Add(playerDisplay);
                }

                RetrieveData();

                Console.Clear();
                Console.WriteLine(" # |         ID |         Name |         Rank |     WR |  AK |  AD |  AK |  KDA |  XPM |  GPM |    AHD |   ATD |   AHH |  ALH | Heroes\n");
                for (int i = 0; i < 10; i++)
                {
                    Console.ForegroundColor = (ConsoleColor)colors[i];
                    Console.Write($"{i + 1,2} |");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(PlayerDisplays[i].Data.WriteData());
                    if (i == 4)
                        Console.WriteLine();

                }
                Console.WriteLine("\nPress enter for receive data or input exit...");
            }
        }
        private static void RetrieveData()
        {
            var playerIDs = OpenDotaAPI.GetPlayerIDs();
            for (int i = 0; i < 10; i++)
            {
                if (i < playerIDs.Count)
                    PlayerDisplays[i].Update(playerIDs[i]);
                else
                    PlayerDisplays[i].Update("");
            }
        }

    }
}
