using System;
using System.Collections.Generic;
using System.Text;

namespace Dota_2_Application.Classes
{
    public class Player
    {
        public string tracked_until { get; set; }
        public string solo_competitive_rank { get; set; }
        public object competitive_rank { get; set; }
        public MmrEstimate mmr_estimate { get; set; }
        public Profile profile { get; set; }
        public int? rank_tier { get; set; }
    }

    public class MmrEstimate
    {
        public int? estimate { get; set; }
        public double? stdDev { get; set; }
        public int? n { get; set; }
    }

    public class Profile
    {
        public int account_id { get; set; }
        public string personaname { get; set; }
        public object name { get; set; }
        public int cheese { get; set; }
        public string steamid { get; set; }
        public string avatar { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
        public string profileurl { get; set; }
        public string last_login { get; set; }
        public object loccountrycode { get; set; }
    }
}
