using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videospieldatenbank.Database;

namespace Videospieldatenbank.Tests
{
    public class UserTest : ITest
    {
        public bool Test()
        {
            try
            {
                UserDatabaseConnector udb = new UserDatabaseConnector();
                return udb.Register("TestUser", "TestPassword") && udb.Login("TestUser", "TestPassword") && udb.Logout();
            }
            catch
            {
                return false;
            }
        }
    }
}
