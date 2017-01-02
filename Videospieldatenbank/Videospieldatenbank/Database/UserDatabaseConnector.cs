using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Videospieldatenbank.Utils;

namespace Videospieldatenbank.Database
{
    public class UserDatabaseConnector : DatabaseConnector
    {
        private bool _isLoggedIn;
        private string _username;

        /// <summary>
        ///     Entspricht dem Profilbild des aktuellen Users.
        /// </summary>
        public byte[] ProfilePicture
        {
            get { return GetProfilePicture(_username); }
            set
            {
                if (Exists(_username))
                    using (MySqlCommand command = MySqlConnection.CreateCommand())
                    {
                        command.CommandText = "UPDATE user SET picture=@image WHERE name=@username";
                        command.Parameters.AddWithValue("@image", value);
                        command.ExecuteNonQuery();
                    }
            }
        }

        /// <summary>
        ///     Entspricht der UserID des angemeldeten Users.
        /// </summary>
        public int UserId
        {
            get
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT id FROM user WHERE name=@username";
                    command.Parameters.AddWithValue("@username", _username);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader.GetInt32(0);
                    }
                }
                throw new Exception("Fehler beim ermitteln der UserID");
            }
        }

        /// <summary>
        ///     Gibt eine Liste mit den IDs aller Freunde des Users zurück.
        /// </summary>
        /// <returns></returns>
        public List<int> GetFriendsList()
        {
            List<int> list = new List<int>();
            if (_isLoggedIn)
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT friend_id FROM friends WHERE user_ID=@userID";
                    command.Parameters.AddWithValue("@userID", UserId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) list.Add(reader.GetInt32(0));
                    }
                }
            return list;
        }

        /// <summary>
        ///     Markiert einen User als Freund.
        /// </summary>
        /// <param name="friendId">Die ID des als Freund zu markierenden Users.</param>
        /// <returns></returns>
        public bool AddFriend(int friendId)
        {
            if (IsFriend(friendId)) return false;
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "INSERT INTO friends(user_ID, friend_ID) VALUES (@userID, @friendID)";
                command.Parameters.AddWithValue("@userID", UserId);
                command.Parameters.AddWithValue("@friendID", friendId);
                command.ExecuteNonQuery();
                return true;
            }
        }

        /// <summary>
        ///     Überprüft ob die User befreundet sind.
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns></returns>
        private bool IsFriend(int friendId)
        {
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM friends WHERE user_ID=@userID AND friend_ID=@friendID";
                command.Parameters.AddWithValue("@userID", UserId);
                command.Parameters.AddWithValue("@friendID", friendId);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }

        /// <summary>
        ///     Entfernt einen User aus der Freundesliste
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public bool RemoveFriend(int friendId)
        {
            if (!IsFriend(friendId)) return false;
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "DELETE FROM friends WHERE user_ID=@userID AND friend_ID=@friendID";
                command.Parameters.AddWithValue("@userID", UserId);
                command.Parameters.AddWithValue("@friendID", friendId);
                command.ExecuteNonQuery();
                return true;
            }
        }

        /// <summary>
        ///     Ermittelt den Namen des Users anhand seiner ID.
        /// </summary>
        /// <param name="id">ID des Users.</param>
        /// <returns>Name des Users.</returns>
        public string GetUsername(int id)
        {
            if (_isLoggedIn)
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT name FROM user WHERE id=@id";
                    command.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader.GetString(0);
                    }
                }
            throw new Exception("Fehler beim ermitteln des Usernames, eventuell existiert die ID nicht.");
        }

        /// <summary>
        ///     Ermittelt die ID des Users anhand seines Namens.
        /// </summary>
        /// <param name="username">Name des Users.</param>
        /// <returns>Name des Users.</returns>
        public int GetId(string username)
        {
            if (_isLoggedIn)
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT id FROM user WHERE name=@username";
                    command.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader.GetInt32(0);
                    }
                }
            throw new Exception("Fehler beim ermitteln der ID, eventuell existiert der Username nicht.");
        }

        /// <summary>
        ///     Überprüft die Anmeldedaten eines Users.
        /// </summary>
        /// <param name="username">Der Name des Users.</param>
        /// <param name="password">Das Passwort des Users.</param>
        /// <returns>True, wenn der Login erfolgreich war.</returns>
        private bool CheckLogin(string username, string password)
        {
            if (!Exists(username)) return false;
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "SELECT password FROM user WHERE name=@username";
                command.Parameters.AddWithValue("@username", username);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() && (PasswordUtils.GetHash(password) == reader[0] as string);
                }
            }
        }

        /// <summary>
        ///     Meldet den User beim Server an.
        /// </summary>
        /// <param name="username">Name des Users.</param>
        /// <param name="password">Passwort des Users.</param>
        /// <returns>True, wenn Login erfolgreich war.</returns>
        public bool Login(string username, string password)
        {
            if (CheckLogin(username, password))
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    _username = username;
                    _isLoggedIn = true;

                    // Setzt Onlinestatus des Users auf true.
                    command.CommandText = "UPDATE user SET online=true WHERE name=@username";
                    command.Parameters.AddWithValue("@username", username);
                    command.ExecuteNonQuery();
                    return true;
                }
            return false;
        }

        /// <summary>
        ///     Meldet einen User beim Server ab.
        /// </summary>
        /// <returns>True, wenn erfolgreich.</returns>
        public bool Logout()
        {
            if (!_isLoggedIn) return false;
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                // Setzt Onlinestatus des Users auf false.
                command.CommandText = "UPDATE user SET online=false WHERE name=@username";
                command.Parameters.AddWithValue("@username", _username);
                command.ExecuteNonQuery();
                _isLoggedIn = false;
                return true;
            }
        }

        /// <summary>
        ///     Erstellt einen neuen User.
        /// </summary>
        /// <param name="username">Username des neuen Users.</param>
        /// <param name="password">Password des neuen Users.</param>
        /// <returns>True, wenn die Registrierung erfolgreich war.</returns>
        public bool Register(string username, string password)
        {
            if (!Exists(username))
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO user (name, password) VALUES (@username, @password)";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", PasswordUtils.GetHash(password));
                    command.ExecuteNonQuery();
                    return true;
                }
            return false;
        }

        /// <summary>
        ///     Gibt an ob ein User bereits existiert.
        /// </summary>
        /// <param name="username">Der zu überprüfende Username</param>
        /// <returns>True, wenn der User existiert.</returns>
        private bool Exists(string username)
        {
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM user WHERE name=@username";
                command.Parameters.AddWithValue("@username", username);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }

        /// <summary>
        ///     Ruft das Profilbild des Users ab.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public byte[] GetProfilePicture(string username)
        {
            if (Exists(_username))
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT picture FROM user WHERE name=@username";
                    command.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        byte[] image = new byte[65556];
                        if (reader.Read())
                            reader.GetBytes(0, 0, image, 0, image.Length);
                        return image;
                    }
                }
            throw new Exception("Fehler beim herunterladen des Profilbildes, eventuell existiert der User nicht.");
        }

        /// <summary>
        ///     Ruft das Profilbild des Users ab.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public byte[] GetProfilePicture(int userId)
        {
            return GetProfilePicture(GetUsername(userId));
        }

        /// <summary>
        ///     Löscht den Benutzer
        /// </summary>
        /// <param name="username">Username des zu löschenden Users.</param>
        /// <param name="password">Passwort des zu löschenden Users.</param>
        /// <returns>True, wenn das Löschen erfolgreich war.</returns>
        public bool DeleteUser(string username, string password)
        {
            if (CheckLogin(username, password))
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM user WHERE name=@username AND password=@password";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", PasswordUtils.GetHash(password));
                    command.ExecuteNonQuery();
                    return true;
                }
            return false;
        }

        /// <summary>
        ///     Gibt eine Liste der Spiele des users zurück
        /// </summary>
        /// <returns>Liste von igdb_urls.</returns>
        public List<string> GetGames()
        {
            //TODO: Testen von GetGames
            List<string> list = new List<string>();
            if (_isLoggedIn)
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT igdb_url FROM gameinfo WHERE user_ID=@userID";
                    command.Parameters.AddWithValue("@userID", UserId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) list.Add(reader.GetString(0));
                    }
                }
            return list;
        }

        /// <summary>
        ///     Überprüft ob der Benutzer das Spiel besitzt/hinzugefügt hat.
        /// </summary>
        /// <param name="igdbUrl"></param>
        /// <returns></returns>
        public bool OwnsGame(string igdbUrl)
        {
            if (_isLoggedIn)
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM gameinfo WHERE user_ID=@userID AND igdb_url=@igdbUrl";
                    command.Parameters.AddWithValue("@userID", UserId);
                    command.Parameters.AddWithValue("@igdbUrl", igdbUrl);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            return false;
        }

        /// <summary>
        ///     Fügt das angegeben Spiel zur Nutzerbibliothek und ggf. Spieledatenbank hinzu.
        /// </summary>
        /// <param name="igdbUrl">Die URL des Spiels.</param>
        /// <param name="execPath">Der Pfad der ausführbaren Datei des Spiels.</param>
        public void AddGame(string igdbUrl, string execPath)
        {
            if (_isLoggedIn)
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    // Updated den Execpath wenn der User das Spiel bereits besitzt oder fügt es neu hinzu wenn nicht.
                    if (OwnsGame(igdbUrl))
                    {
                        command.CommandText = "UPDATE gameinfo SET exec_path=@execPath WHERE igdb_url=@igdbUrl";
                        command.Parameters.AddWithValue("@execPath", execPath);
                        command.Parameters.AddWithValue("@igdbUrl", igdbUrl);
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO gameinfo(`user_ID`, `igdb_url`, `exec_path`, `playtime`) VALUES (@userID, @igdbUrl, @execPath, '0')";
                        command.Parameters.AddWithValue("@userID", UserId);
                        command.Parameters.AddWithValue("@igdbUrl", igdbUrl);
                        command.Parameters.AddWithValue("@execPath", execPath);
                    }
                    command.ExecuteNonQuery();

                    // Fügt das Spiel zur Datenbank hinzu für den Fall das es noch nicht existiert.
                    GameDatabaseConnector gdc = new GameDatabaseConnector();
                    gdc.AddGame(IgdbParser.ParseGame(igdbUrl));
                }
        }

        /// <summary>
        ///     Ermittelt die gespielte Zeit im angegebenen Spiel.
        /// </summary>
        /// <param name="igdbUrl">Das Spiel dessen Zeit ermittelt werden soll</param>
        /// <returns></returns>
        public TimeSpan GetPlayTime(string igdbUrl)
        {
            if (_isLoggedIn && OwnsGame(igdbUrl))
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT playtime FROM gameinfo WHERE user_ID=@userID AND igdb_url=@igdbUrl";
                    command.Parameters.AddWithValue("@userID", UserId);
                    command.Parameters.AddWithValue("@igdbUrl", igdbUrl);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return TimeSpan.FromMinutes(reader.GetInt32(0));
                    }
                }
            throw new Exception("Fehler beim ermitteln der Playtime.");
        }

        /// <summary>
        ///     Erhöht die Playtime um einen gegebenen Wert.
        /// </summary>
        /// <param name="igdbUrl">Das Spiel das gespielt wurde.</param>
        /// <param name="playedTime">Die Zeit die es gespielt wurde.</param>
        public void AddPlayTime(string igdbUrl, TimeSpan playedTime)
        {
            TimeSpan newPlayTime = GetPlayTime(igdbUrl) + playedTime;
            if (_isLoggedIn && OwnsGame(igdbUrl))
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "UPDATE gameinfo SET playtime=@playTime WHERE user_ID=@userID AND igdb_url=@igdbUrl";
                    command.Parameters.AddWithValue("@playTime", (int) newPlayTime.TotalMinutes);
                    command.Parameters.AddWithValue("@userID", UserId);
                    command.Parameters.AddWithValue("@igdbUrl", igdbUrl);
                    command.ExecuteNonQuery();
                }
        }
    }
}