using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Videospieldatenbank.Database
{
    public class GameDatabaseConnector : DatabaseConnector
    {
        /// <summary>
        ///     Ruft Spielinformationen über ein Spiel ab.
        /// </summary>
        /// <param name="igdbUrl">URL des Spiels.</param>
        /// <returns><see cref="Game" />-Objekt mit Spielinformationen.</returns>
        public Game GetGameInfo(string igdbUrl)
        {
            Game game = new Game();
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"select * from games where igdb_url='{igdbUrl}'";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    game.IgdbUrl = reader["igdb_url"] as string;
                    game.CoverUrl = reader["cover_url"] as string;
                    game.Name = reader["name"] as string;
                    game.Developer = reader["developer"] as string;
                    game.Plattforms = (reader["plattforms"] as string).Split(';');
                    game.Genres = (reader["genres"] as string).Split(';');
                    game.Rating = (int) reader["rating"];
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
            List<Game> games = new List<Game>();
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM games WHERE igdb_url LIKE '%{name}%'";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Game game = new Game
                        {
                            IgdbUrl = reader["igdb_url"] as string,
                            CoverUrl = reader["cover_url"] as string,
                            Name = reader["name"] as string,
                            Developer = reader["developer"] as string,
                            Plattforms = (reader["plattforms"] as string).Split(';'),
                            Genres = (reader["genres"] as string).Split(';'),
                            Rating = (int) reader["rating"]
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
        public Game[] GetGameInfos()
        {
            var games = new List<Game>();
            using (var command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM games";
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
        /// <param name="game"></param>
        /// <returns>Gibt an ob das hinzufügen erfolgreich war.</returns>
        public bool AddGame(Game game)
        {
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM games WHERE igdb_url='{game.IgdbUrl}'";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) return false;
                }
            }
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                string plattforms = "";
                foreach (string plattform in game.Plattforms)
                    plattforms += plattform + ";";
                string genres = "";
                foreach (string genre in game.Genres)
                    genres += genre + ";";
                command.CommandText = "INSERT INTO games(`igdb_url`, `cover_url`, `name`, `developer`, `plattforms`, `genres`, `rating`)" +
                                      "VALUES (@igdbUrl, @coverUrl, @name, @developer, @plattforms, @genres, @rating)";
                command.Parameters.AddWithValue("@igdbUrl", game.IgdbUrl);
                command.Parameters.AddWithValue("@coverUrl", game.CoverUrl);
                command.Parameters.AddWithValue("@name", game.Name);
                command.Parameters.AddWithValue("@developer", game.Developer);
                command.Parameters.AddWithValue("@plattforms", plattforms);
                command.Parameters.AddWithValue("@genres", genres);
                command.Parameters.AddWithValue("@rating", game.Rating);
                    command.ExecuteNonQuery();
            }
            return true;
        }
    }
}