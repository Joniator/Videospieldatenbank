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
    }
}