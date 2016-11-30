using System;
using System.Diagnostics;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Videospieldatenbank
{
    public class DatabaseConnector
    {
        private readonly MySqlConnection _mySqlConnection = new MySqlConnection();
        public bool Connected => _mySqlConnection.Ping();

        /// <exception cref="MySqlException">Verbindung zum Server fehlgeschlagen.</exception>
        public DatabaseConnector()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder()
            {
                Server = "tazed.tk",
                UserID = "igdb",
                Password = "QhwnUzLpyWbchfUF",
                Database = "igdb"
            };
            _mySqlConnection.ConnectionString = builder.ToString();
            _mySqlConnection.Open();
        }
    }
}