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
            DatabaseConnector dbConnector = new DatabaseConnector();
            dbConnector.AddGame(IgdbParser.ParseGame("https://www.igdb.com/games/the-witcher-3-wild-hunt"));
            dbConnector.AddGame(IgdbParser.ParseGame("https://www.igdb.com/games/the-legend-of-zelda-ocarina-of-time"));
            Game[] games = IgdbParser.GetTop100();
            foreach (Game game in games)
            {
                dbConnector.AddGame(game);
            }
        }
    }
}
