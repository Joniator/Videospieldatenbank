using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videospieldatenbank.Database
{
    public class GameDatabaseConnector : DatabaseConnector
    { /// <summary>
      ///     Ruft Spielinformationen über ein Spiel ab.
      /// </summary>
      /// <param name="igdbUrl">URL des Spiels.</param>
      /// <returns><see cref="Game" />-Objekt mit Spielinformationen.</returns>
        public Game GetGameInfo(string igdbUrl)
        {
            var game = new Game();
            using (var command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"select * from games where igdb_url='{igdbUrl}'";
                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    game.IgdbUrl = reader["igdb_url"] as string;
                    game.CoverUrl = reader["cover_url"] as string;
                    game.Name = reader["name"] as string;
                    game.Developer = reader["developer"] as string;
                    game.Plattforms = (reader["plattforms"] as string).Split(';');
                    game.Genres = (reader["genres"] as string).Split(';');
                    game.Rating = (int)reader["rating"];
                }
            }
            return game;
        }

        /// <summary>
        ///     Ruft Spielinformationen von mehreren Spielen die <paramref name="name" /> enthalten.
        /// </summary>
        /// <param name="name">Teile des Namens</param>
        /// <returns>Array mit <see cref="Game" />-Objekt mit Spielinformationen.</returns>
        public Game[] GetGameInfos(string name)
        {
            var games = new List<Game>();
            using (var command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"select * from games where igdb_url LIKE '%{name}%'";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var game = new Game
                        {
                            IgdbUrl = reader["igdb_url"] as string,
                            CoverUrl = reader["cover_url"] as string,
                            Name = reader["name"] as string,
                            Developer = reader["developer"] as string,
                            Plattforms = (reader["plattforms"] as string).Split(';'),
                            Genres = (reader["genres"] as string).Split(';'),
                            Rating = (int)reader["rating"]
                        };
                        games.Add(game);
                    }
                }
            }
            return games.ToArray();
        }

        /// <summary>
        ///     Ruft Spielinformationen von allen Spielen ab.
        /// </summary>
        /// <returns>Array mit <see cref="Game" />-Objekt mit Spielinformationen.</returns>
        public Game[] GetGameInfosAll()
        {
            var games = new List<Game>();
            using (var command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"select * FROM games";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var game = new Game
                        {
                            IgdbUrl = reader["igdb_url"] as string,
                            CoverUrl = reader["cover_url"] as string,
                            Name = reader["name"] as string,
                            Developer = reader["developer"] as string,
                            Plattforms = (reader["plattforms"] as string).Split(';'),
                            Genres = (reader["genres"] as string).Split(';'),
                            Rating = (int)reader["rating"]
                        };
                        games.Add(game);
                    }
                }
            }
            return games.ToArray();
        }


        /// <summary>
        ///     Fügt ein Spiel von igdb.com zur Datenbank hinzu.
        /// </summary>
        /// <param name="igdbUrl">Link zu der igdb-Website des Spiels.</param>
        /// <returns>Gibt an ob das hinzufügen erfolgreich war.</returns>
        public bool AddGame(Game game)
        {
            using (var command = MySqlConnection.CreateCommand())
            {
                var plattforms = "";
                foreach (var plattform in game.Plattforms)
                    plattforms += plattform + ";";
                var genres = "";
                foreach (var genre in game.Genres)
                    genres += genre + ";";
                command.CommandText = "INSERT INTO games("
                                      +
                                      "`igdb_url`, `cover_url`, `name`, `developer`, `plattforms`, `genres`, `rating`)"
                                      +
                                      $"VALUES ('{game.IgdbUrl}', '{game.CoverUrl}', '{game.Name}', '{game.Developer}', '{plattforms}', '{genres}', {game.Rating})";
                Console.WriteLine(command.CommandText);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                }
            }
            return true;
        }
    }
}
