using Dota_2_Console_App.Classes;
using Dota_2_Console_App.Enums;
using Dota_2_Console_App.Requests;
using Dota_2_Console_App.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dota_2_Console_App
{
    public class PlayerDisplay
    {
        public DisplayData Data { get; set; }

        public PlayerDisplay()
        {
            Data = new DisplayData();

        }

        public void Update(string playerID)
        {
            Data.Clear();

            Player playerData = null;
            Match[] recentMatches = null;

            playerData = OpenDotaAPI.GetPlayer(playerID);
            recentMatches = OpenDotaAPI.GetPlayerMatches(playerID, 20, 30, -1);
            Data.ConsumeData(playerID, playerData, recentMatches);

        }

        public class DisplayData
        {
            public string ID { get; private set; }

            public string Name { get; private set; }
            public string SoloMMR { get; private set; }
            public string EstimateMMR { get; private set; }
            public Rank? Rank { get; private set; }
            public double Winrate { get; private set; }
            public TimeSpan AverageDuration { get; private set; }
            public Match[] RecentMatches { get; private set; }

            public double AverageKills { get; private set; }
            public double AverageDeaths { get; private set; }
            public double AverageAssists { get; private set; }
            public double AverageKDA { get; private set; }
            public double AverageXPM { get; private set; }
            public double AverageGPM { get; private set; }
            public double AverageHeroDamage { get; private set; }
            public double AverageTowerDamage { get; private set; }
            public double AverageHeroHealing { get; private set; }
            public double AverageLastHits { get; private set; }

            public string WriteData()
            {
                if (Name != "Anonymous")
                {
                    string heroes = "";
                    for (int i = 0; i < RecentMatches.Length; i++)
                    {
                        if (RecentMatches[i].Won)
                            heroes += $" {(Hero)RecentMatches[i].hero_id}(W), ";
                        else
                            heroes += $" {(Hero)RecentMatches[i].hero_id}(L), ";
                    }

                    return $" {ID,10} | {Name,12} | {Rank,12} | {Winrate,5}% | {AverageKills,3} | {AverageDeaths,3} | {AverageAssists,3} | {AverageKDA,4} | {AverageXPM,4} | {AverageGPM,4} | {AverageHeroDamage,6} | {AverageTowerDamage,5} | {AverageHeroHealing,5} | {AverageLastHits,4} |" + heroes;
                }
                else
                    return " Anonymous  |            ? |            ? |      ? |   ? |   ? |   ? |    ? |    ? |    ? |      ? |     ? |     ? |    ? |";
            }

            public void ConsumeData(string id, Player playerData, Match[] recentMatches)
            {
                ID = id;

                ConsumePlayerData(playerData, recentMatches);
                ConsumeRecentMatches(recentMatches);
            }
            private void ConsumePlayerData(Player playerData, Match[] recentMatches)
            {
                if (playerData == null || playerData.profile == null || recentMatches.Length == 0)
                {
                    Name = "Anonymous";
                    SoloMMR = "X";
                    EstimateMMR = "X";
                    Rank = null;
                }
                else
                {
                    Name = StringUtils.TruncateLongString(playerData.profile.personaname, 12, "");
                    SoloMMR = playerData.solo_competitive_rank ?? "X";
                    EstimateMMR = playerData.mmr_estimate.estimate.HasValue ? playerData.mmr_estimate.estimate.ToString() : "X";
                    Rank = playerData.rank_tier.HasValue ? (Rank?)playerData.rank_tier.Value : 0;
                }
            }
            private void ConsumeRecentMatches(Match[] recentMatches)
            {
                if (recentMatches != null && recentMatches.Length > 0)
                {
                    RecentMatches = recentMatches;

                    double wonMatches = 0;
                    int totalSeconds = 0;
                    double totalKills = 0;
                    double totalDeaths = 0;
                    double totalAssists = 0;
                    double totalXPM = 0;
                    double totalGPM = 0;
                    double totalHeroDamage = 0;
                    double totalTowerDamage = 0;
                    double totalHeroHealing = 0;
                    double totalLastHits = 0;

                    foreach (var match in recentMatches)
                    {
                        if (match.Won)
                            wonMatches++;

                        totalSeconds += match.duration;
                        totalKills += match.kills;
                        totalDeaths += match.deaths;
                        totalAssists += match.assists;
                        totalXPM += match.xp_per_min;
                        totalGPM += match.gold_per_min;
                        totalHeroDamage += match.hero_damage.GetValueOrDefault(0);
                        totalTowerDamage += match.tower_damage.GetValueOrDefault(0);
                        totalHeroHealing += match.hero_healing.GetValueOrDefault(0);
                        totalLastHits += match.last_hits;
                    }

                    Winrate = Math.Round(wonMatches / recentMatches.Length * 100, 2);
                    TimeSpan sumDuration = new TimeSpan(0, 0, totalSeconds);
                    AverageDuration = new TimeSpan(sumDuration.Ticks / recentMatches.Length);
                    AverageKills = Math.Round(totalKills / recentMatches.Length, 0);
                    AverageDeaths = Math.Round(totalDeaths / recentMatches.Length, 0);
                    AverageAssists = Math.Round(totalAssists / recentMatches.Length, 0);
                    AverageKDA = Math.Round((AverageKills + AverageAssists) / (AverageDeaths == 0 ? 1 : AverageDeaths), 2);
                    AverageXPM = Math.Round(totalXPM / recentMatches.Length, 0);
                    AverageGPM = Math.Round(totalGPM / recentMatches.Length, 0);
                    AverageHeroDamage = Math.Round(totalHeroDamage / recentMatches.Length, 0);
                    AverageTowerDamage = Math.Round(totalTowerDamage / recentMatches.Length, 0);
                    AverageHeroHealing = Math.Round(totalHeroHealing / recentMatches.Length, 0);
                    AverageLastHits = Math.Round(totalLastHits / recentMatches.Length, 0);
                }
            }

            public void Clear()
            {
                ID = "";
                Name = "";
                SoloMMR = "";
                EstimateMMR = "";
                Rank = null;
                Winrate = 0;
                AverageDuration = TimeSpan.Zero;
                RecentMatches = null;
                AverageKills = 0;
                AverageDeaths = 0;
                AverageAssists = 0;
                AverageKDA = 0;
                AverageXPM = 0;
                AverageGPM = 0;
                AverageHeroDamage = 0;
                AverageTowerDamage = 0;
                AverageHeroHealing = 0;
                AverageLastHits = 0;
            }
        }
    }
}
