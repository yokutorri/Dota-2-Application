using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Dota_2_Console_App.Requests
{
    public class Connection
    {
        public static bool GetStatus()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
