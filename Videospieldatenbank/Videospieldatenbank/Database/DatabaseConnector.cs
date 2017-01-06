using System;
using MySql.Data.MySqlClient;

namespace Videospieldatenbank.Database
{
    public class DatabaseConnector : IDisposable
    {
        protected readonly MySqlConnection MySqlConnection = new MySqlConnection();

        /// <summary>
        ///     Stellt eine Verbindung mit der Datenbank her.
        /// </summary>
        /// <exception cref="MySqlException">Verbindung zum Server fehlgeschlagen.</exception>
        public DatabaseConnector()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = "tazed.tk",
                UserID = "igdb",
                Password = "QhwnUzLpyWbchfUF",
                Database = "igdb"
            };
            MySqlConnection.ConnectionString = builder.ToString();
            MySqlConnection.Open();
        }

        public bool Connected => MySqlConnection.Ping();

        public void Dispose()
        {
            MySqlConnection.Dispose();
        }
    }
}