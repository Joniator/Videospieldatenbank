using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Videospieldatenbank
{
    class DatabaseInteraction
    {
        private MySqlConnection _connection;

        public bool Connected => _connection.State == ConnectionState.Open;

        public DatabaseInteraction(string server, int port, string databaseName, string userId)
        {
            string connectionString = $"Server={server};" +
                                      $"port={port};" +
                                      $"Database={databaseName};" +
                                      $"Uid={userId};";

            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }
    }
}
