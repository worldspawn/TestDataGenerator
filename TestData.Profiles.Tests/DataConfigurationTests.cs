using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Profiles.Tests.TestClasses;
using Xunit;

namespace TestData.Profiles.Tests
{
    public class DataConfigurationTests
    {
        private readonly string[] _names = new string[] {
            "Smith", "Jones", "Williams", "Brown", "Wilson", "Taylor"
        };

        public DataConfigurationTests()
        {
            _dataConfiguration = new DataConfiguration();
            _dataConfiguration.CreateProfileFor<Role>(() => new Role())
                .ForMember(x => x.Id, f => f.ValueFromExpression((u) => Guid.NewGuid()))
                .ForMember(x => x.Name, f => f.ValueFromRandomString(4, 8));

            _dataConfiguration.CreateProfileFor<User>(() => new User())
                .FollowPath(x => x.Role)
                .ForMember(x => x.Id, f => f.ValueFromExpression((u) => Guid.NewGuid()))
                .ForMember(x => x.FirstName, f => f.ValueFromRandomString(4, 8))
                .ForMember(x => x.Surname, f => f.ValueFromCollection(_names))
                .ForMember(x => x.LogonCount, f => f.ValueFromRandom(0, 5));
        }

        private readonly DataConfiguration _dataConfiguration;

        [Fact]
        public void CanCreate50000Users()
        {
            var users = _dataConfiguration.Get<User>().Generate(50000);
            
            users.Count();
        }

        [Fact]
        public void CanCreateUsers()
        {
            var users = _dataConfiguration.Get<User>().Generate(500);
            
            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.Equal(500, users.Count());
            var first = users.First();
            Assert.Contains(first.Surname, _names);
            Assert.NotEqual(Guid.Empty, users.First().Id);
            Assert.NotNull(first.Role);
        }

        [Fact]
        public void CanCreateArrayOfRolesAndUseForMember()
        {
            
        }
    }
}
