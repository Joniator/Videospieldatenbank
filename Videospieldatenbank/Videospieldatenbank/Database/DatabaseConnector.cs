using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Videospieldatenbank.Database
{
    public class DatabaseConnector
    {
        protected readonly MySqlConnection MySqlConnection = new MySqlConnection();

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
            MySqlConnection.ConnectionString = builder.ToString();
            MySqlConnection.Open();
        }

        public bool Connected => MySqlConnection.Ping();
    }
}