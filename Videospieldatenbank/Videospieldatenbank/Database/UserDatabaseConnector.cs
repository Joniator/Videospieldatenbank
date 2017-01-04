using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Videospieldatenbank.Utils;

namespace Videospieldatenbank.Database
{
    public class UserDatabaseConnector : DatabaseConnector
    {
        private string _username;

        /// <summary>
        ///     Entspricht dem Profilbild des aktuellen Users.
        /// </summary>
        public byte[] ProfilePicture
        {
            get { return GetProfilePicture(_username); }
            set
            {
                if (!Exists(_username)) return;
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "UPDATE user SET picture=@image WHERE name=@username";
                    command.Parameters.AddWithValue("@image", value);
                    command.Parameters.AddWithValue("@username", _username);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Gibt an ob der User als Online markiert ist.
        /// </summary>
        public bool OnlineStatus
        {
            get
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT online FROM user WHERE name=@username";
                    command.Parameters.AddWithValue("@username", _username);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return reader.GetBoolean(0);
                    }
                    throw new Exception("Fehler beim ermitteln des Onlinestatus.");
                }
            }
            set
            {
                using (MySqlCommand command = MySqlConnection.CreateCommand())
                {
                    command.CommandText = "UPDATE user SET online=@onlineStatus WHERE name=@username";
                    command.Parameters.AddWithValue("@username", _username);
                    command.Parameters.AddWithValue("@onlineStatus", value);
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

        #region Friends

        /// <summary>
        ///     Gibt eine Liste mit den IDs aller Freunde des Users zurück.
        /// </summary>
        /// <returns></returns>
        public List<int> GetFriendsList()
        {
            List<int> list = new List<int>();
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

        #endregion

        #region Account

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
            if (!CheckLogin(username, password)) return false;
            _username = username;
            OnlineStatus = true;
            return true;
        }

        /// <summary>
        ///     Meldet einen User beim Server ab.
        /// </summary>
        /// <returns>True, wenn erfolgreich.</returns>
        public bool Logout()
        {
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                // Setzt Onlinestatus des Users auf false.
                command.CommandText = "UPDATE user SET online=false WHERE name=@username";
                command.Parameters.AddWithValue("@username", _username);
                command.ExecuteNonQuery();
                OnlineStatus = false;
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
            if (Exists(username)) return false;
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "INSERT INTO user (name, password) VALUES (@username, @password)";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", PasswordUtils.GetHash(password));
                command.ExecuteNonQuery();
                return true;
            }
        }

        /// <summary>
        ///     Löscht den Benutzer
        /// </summary>
        /// <param name="username">Username des zu löschenden Users.</param>
        /// <param name="password">Passwort des zu löschenden Users.</param>
        /// <returns>True, wenn das Löschen erfolgreich war.</returns>
        public bool DeleteUser(string username, string password)
        {
            if (!CheckLogin(username, password)) return false;
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "DELETE FROM user WHERE name=@username AND password=@password";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", PasswordUtils.GetHash(password));
                command.ExecuteNonQuery();
                return true;
            }
        }

        #endregion

        #region Profilbild

        /// <summary>
        ///     Ruft das Profilbild des Users ab.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public byte[] GetProfilePicture(string username)
        {
            if (!Exists(_username))
                throw new Exception("Fehler beim herunterladen des Profilbildes, eventuell existiert der User nicht.");
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
        }

        /// <summary>
        ///     Ruft das Profilbild des Users ab.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public byte[] GetProfilePicture(int userId)
        {
            return GetProfilePicture(GetUsername(userId));
        }

        #endregion

        #region Userinfo

        /// <summary>
        ///     Überprüft ob ein User online ist.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsOnline(string username)
        {
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "SELECT online FROM user WHERE name=@username";
                command.Parameters.AddWithValue("@username", username);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) return reader.GetBoolean(0);
                }
                throw new Exception("Fehler beim ermitteln des Onlinestatus.");
            }
        }

        /// <summary>
        ///     Ermittelt den Namen des Users anhand seiner ID.
        /// </summary>
        /// <param name="id">ID des Users.</param>
        /// <returns>Name des Users.</returns>
        public string GetUsername(int id)
        {
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
        ///     Ändert den Usernamen.
        /// </summary>
        /// <param name="username">Der neue Username.</param>
        /// <returns></returns>
        public bool SetUsername(string username)
        {
            if (!Exists(_username) || Exists(username))
                throw new Exception("Fehler beim Ändern des Usernames, eventuell ist der Name schon vergeben.");
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "UPDATE user SET name=@newUsername WHERE name=@oldUsername";
                command.Parameters.AddWithValue("@oldUsername", _username);
                command.Parameters.AddWithValue("@newUsername", username);
                command.ExecuteNonQuery();
                _username = username;
                return true;
            }
        }

        /// <summary>
        ///     Ermittelt die ID des Users anhand seines Namens.
        /// </summary>
        /// <param name="username">Name des Users.</param>
        /// <returns>Name des Users.</returns>
        public int GetId(string username)
        {
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

        #endregion

        #region Games

        /// <summary>
        ///     Gibt eine Liste der Spiele des users zurück
        /// </summary>
        /// <returns>Liste von igdb_urls.</returns>
        public List<string> GetGames()
        {
            List<string> list = new List<string>();
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
        }

        /// <summary>
        ///     Fügt das angegeben Spiel zur Nutzerbibliothek und ggf. Spieledatenbank hinzu.
        /// </summary>
        /// <param name="igdbUrl">Die URL des Spiels.</param>
        /// <param name="execPath">Der Pfad der ausführbaren Datei des Spiels.</param>
        public void AddGame(string igdbUrl, string execPath)
        {
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
                    command.CommandText =
                        "INSERT INTO gameinfo(`user_ID`, `igdb_url`, `exec_path`, `playtime`) VALUES (@userID, @igdbUrl, @execPath, '0')";
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
        /// <param name="userId">Der User dessen Spielzeit ermittelt werden soll.</param>
        /// <returns></returns>
        public TimeSpan GetPlayTime(string igdbUrl, int userId)
        {
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "SELECT playtime FROM gameinfo WHERE user_ID=@userID AND igdb_url=@igdbUrl";
                command.Parameters.AddWithValue("@userID", userId);
                command.Parameters.AddWithValue("@igdbUrl", igdbUrl);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) return TimeSpan.FromMinutes(reader.GetInt32(0));
                }
            }
            throw new Exception("Fehler beim ermitteln der Playtime.");
        }

        /// <summary>
        ///     Ermittelt die gespielte Zeit im angegebenen Spiel.
        /// </summary>
        /// <param name="igdbUrl">Das Spiel dessen Zeit ermittelt werden soll</param>
        /// <returns></returns>
        public TimeSpan GetPlayTime(string igdbUrl)
        {
            return GetPlayTime(igdbUrl, UserId);
        }

        /// <summary>
        ///     Erhöht die Playtime um einen gegebenen Wert.
        /// </summary>
        /// <param name="igdbUrl">Das Spiel das gespielt wurde.</param>
        /// <param name="playedTime">Die Zeit die es gespielt wurde.</param>
        public void AddPlayTime(string igdbUrl, TimeSpan playedTime)
        {
            TimeSpan newPlayTime = GetPlayTime(igdbUrl) + playedTime;
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText =
                    "UPDATE gameinfo SET playtime=@playTime WHERE user_ID=@userID AND igdb_url=@igdbUrl";
                command.Parameters.AddWithValue("@playTime", (int) newPlayTime.TotalMinutes);
                command.Parameters.AddWithValue("@userID", UserId);
                command.Parameters.AddWithValue("@igdbUrl", igdbUrl);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Ermittelt den Pfad unter dem das Spiel gespeichert wurde.
        /// </summary>
        /// <param name="igdbUrl"></param>
        /// <returns></returns>
        public string GetExecPath(string igdbUrl)
        {
            using (MySqlCommand command = MySqlConnection.CreateCommand())
            {
                command.CommandText = "SELECT exec_path FROM gameinfo WHERE user_ID=@userID AND igdb_url=@igdbUrl";
                command.Parameters.AddWithValue("@igdbUrl", igdbUrl);
                command.Parameters.AddWithValue("@userID", UserId);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) return reader.GetString(0);
                }
            }
            throw new Exception("Fehler beim ermitteln des exec_path, eventuell existiert das Spiel nicht oder es ist kein Pfad eingespeichert.");
        }

        #endregion
    }
}