using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videospieldatenbank.Tests;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ITest> tests = new List<ITest>();
            tests.Add(new ImageUtilsTest());
            tests.Add(new UserTest());
            
            // Startet jeden Test in der Liste, true wenn 0 Tests gescheitert sind.
            foreach (ITest test in tests)
            {
                Console.WriteLine(test.Test());
            }
            Console.Read();
        }
    }
}
