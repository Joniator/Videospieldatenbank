using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Videospieldatenbank.Database;
using Videospieldatenbank.Utils;

namespace Test
{
    class Program
    {

        static void Main(string[] args)
        {
            UserDatabaseConnector us = new UserDatabaseConnector();
            us.Login("JonnyB", "stupidboy");
            us.AddGame("https://www.igdb.com/games/simcity-4", "D:\\Spiele\\SimCity 4 Deluxe\\Apps\\SimCity 4.exe");
            Console.Read();
        }
    }
}
