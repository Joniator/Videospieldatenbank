using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videospieldatenbank.Database;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            GameDatabaseConnector gameDatabaseConnector = new GameDatabaseConnector();

            UserDatabaseConnector us = new UserDatabaseConnector();
            us.Login("Crossiny", "Swag1337");
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

            Console.Read();
        }
    }
}
