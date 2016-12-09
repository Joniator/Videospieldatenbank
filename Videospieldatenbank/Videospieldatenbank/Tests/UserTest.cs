using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Videospieldatenbank.Database;
using Videospieldatenbank.Utils;

namespace Videospieldatenbank.Tests
{
    public class UserTest : ITest
    {
        public bool Test()
        {
            try
            {
                UserDatabaseConnector udb = new UserDatabaseConnector();
                udb.DeleteUser("TestUser", "TestPassword");
                using (WebClient client = new WebClient())
                {
                    byte[] image = client.DownloadData("http://www.skringers.com/wp-content/uploads/Beautiful-Landscape-Wallpapers-Island-150x150.jpg");

                    bool success = udb.Register("TestUser", "TestPassword") &&
                                   udb.Login("TestUser", "TestPassword") &&
                                   udb.SetProfilePicture(image) &&
                                   udb.GetProfilePicture("TestUser") != null;
                    var friendsList = udb.GetFriendsList();
                    return success &&
                    udb.Logout() &&
                    udb.DeleteUser("TestUser", "TestPassword");
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
