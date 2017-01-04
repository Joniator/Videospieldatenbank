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
            GameDatabaseConnector gameDatabaseConnector = new GameDatabaseConnector();
            UserDatabaseConnector us = new UserDatabaseConnector();
            Console.WriteLine(PasswordUtils.GetHash("xD"));
            bool login = us.Login("Crossiny", "Swag1337");
            us.ProfilePicture = new byte[0];
            int usUserId = us.UserId;
            int id = us.GetId("Crossiny");
            us.SetUsername("Bossiny");
            us.SetUsername("Crossiny");
            Console.WriteLine(id);
            Console.WriteLine(us.GetUsername(id));
            us.AddFriend(24);
            us.RemoveFriend(24);
            List<int> friendsList = us.GetFriendsList();
            us.AddGame("https://www.igdb.com/games/the-last-guardian", null);
            us.AddGame("https://www.igdb.com/games/the-legend-of-zelda-ocarina-of-time", null);
            us.AddGame("https://www.igdb.com/games/super-mario-run", null);
            List<string> games = us.GetGames();

            TimeSpan playTime = us.GetPlayTime("https://www.igdb.com/games/the-last-guardian");
            Console.WriteLine(playTime.ToString());

            us.AddPlayTime("https://www.igdb.com/games/the-last-guardian", TimeSpan.FromMinutes(120));
            playTime = us.GetPlayTime("https://www.igdb.com/games/the-last-guardian");
            Console.WriteLine(playTime.ToString());
            us.Logout();
            Console.Read();
        }
    }
}
