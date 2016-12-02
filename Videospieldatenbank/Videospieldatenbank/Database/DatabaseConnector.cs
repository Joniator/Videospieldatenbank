using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Videospieldatenbank.Database
{
    public class DatabaseConnector
    {
        private readonly MySqlConnection _mySqlConnection = new MySqlConnection();

        /// <summary>
        ///     Stellt eine Verbindung mit der Datenbank her.
        /// </summary>
        /// <exception cref="MySqlException">Verbindung zum Server fehlgeschlagen.</exception>
        public DatabaseConnector()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "tazed.tk",
                UserID = "igdb",
                Password = "QhwnUzLpyWbchfUF",
                Database = "igdb"
            };
            _mySqlConnection.ConnectionString = builder.ToString();
            _mySqlConnection.Open();
        }

        public bool Connected => _mySqlConnection.Ping();

        /// <summary>
        /// Ruft Spielinformationen über ein Spiel ab.
        /// </summary>
        /// <param name="igdbUrl">URL des Spiels.</param>
        /// <returns><see cref="Game"/>-Objekt mit Spielinformationen.</returns>
        public Game GetGameInfo(string igdbUrl)
        {
            Game game = new Game();
            using (MySqlCommand command = _mySqlConnection.CreateCommand())
            {
                command.CommandText = $"select * from games where igdb_url='{igdbUrl}'";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    game.IgdbUrl = reader["igdb_url"] as string;
                    game.CoverUrl = reader["cover_url"] as string;
                    game.Name = reader["name"] as string;
                    game.Developer = reader["developer"] as string;
                    game.Website = reader["official_website"] as string;
                    game.Plattforms = (reader["plattforms"] as string).Split(';');
                    game.Genres = (reader["genres"] as string).Split(';');
                    game.Rating = (int) reader["rating"];
                }
            }
            return game;
        }

        /// <summary>
        /// Ruft Spielinformationen von mehreren Spielen die <paramref name="name"/> enthalten.
        /// </summary>
        /// <param name="name">Teile des Namens</param>
        /// <returns>Array mit <see cref="Game"/>-Objekt mit Spielinformationen.</returns>
        public Game[] GetGameInfos(string name)
        {
            List<Game> games = new List<Game>();
            using (MySqlCommand command = _mySqlConnection.CreateCommand())
            {
                command.CommandText = $"select * from games where igdb_url LIKE '%{name}%'";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Game game = new Game();
                        game.IgdbUrl = reader["igdb_url"] as string;
                        game.CoverUrl = reader["cover_url"] as string;
                        game.Name = reader["name"] as string;
                        game.Developer = reader["developer"] as string;
                        game.Website = reader["official_website"] as string;
                        game.Plattforms = (reader["plattforms"] as string).Split(';');
                        game.Genres = (reader["genres"] as string).Split(';');
                        game.Rating = (int) reader["rating"];
                        games.Add(game);
                    }
                }
            }
            return games.ToArray();
        }

        /// <summary>
        /// Fügt ein Spiel von igdb.com zur Datenbank hinzu.
        /// </summary>
        /// <param name="igdbUrl">Link zu der igdb-Website des Spiels.</param>
        /// <returns>Gibt an ob das hinzufügen erfolgreich war.</returns>
        public bool AddGame(string igdbUrl)
        {
            
            return true; 
        }
    }
}