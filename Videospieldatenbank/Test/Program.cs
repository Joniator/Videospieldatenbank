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
            List<Game> games = new List<Game>();
            games.Add(IgdbParser.ParseGame("https://www.igdb.com/games/ark-survival-evolved"));
            games.Add(IgdbParser.ParseGame("https://www.igdb.com/games/the-witcher-3-wild-hunt"));
            games.Add(IgdbParser.ParseGame("https://www.igdb.com/games/the-legend-of-zelda-a-link-to-the-past"));
        }
    }
}
