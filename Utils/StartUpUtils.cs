using Dota_2_Console_App.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dota_2_Console_App.Utils
{
    public static class StartUpUtils
    {
        public static void StartUp()
        {
            if (!Connection.GetStatus())
            {
                Console.WriteLine("Please, check your internet connection");
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (!File.Exists("path.cfg"))
            {
                File.Create("path.cfg");
                Console.WriteLine("Please, specify the path to Dota 2 in path.cfg");
                Console.WriteLine("Example: D:\\\\SteamLibrary\\steamapps\\common\\dota 2 beta");
                Console.ReadLine();
                Environment.Exit(0);
            }
            string[] strcount = File.ReadAllLines("path.cfg");
            if(strcount.Length == 0)
            {
                Console.WriteLine("It looks like path.cfg is empty");
                Console.ReadLine();
                Environment.Exit(0);
            }
            if (!File.Exists(File.ReadLines("path.cfg").First() + "\\game\\dota\\server_log.txt"))
            {
                Console.WriteLine("Please, check path to Dota 2 in path.cfg");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
        
    }
}
