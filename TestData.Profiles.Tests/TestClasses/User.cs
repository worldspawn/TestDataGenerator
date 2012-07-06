using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Profiles.Tests.TestClasses
{
    public class User : TestClass
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int LogonCount { get; set; }
        public Role Role { get; set; }
        public List<Order> Orders { get; set; }
    }
}
