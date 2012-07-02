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

        [Fact]
        public void CanCreateUsers()
        {
            DataConfiguration dataConfiguration = new DataConfiguration();
            dataConfiguration.CreateProfileFor<User>(() => new User())
                .ForMember(x => x.Id).ValueFrom(f => f.ValueFromExpression((u) => Guid.NewGuid()))
                .ForMember(x => x.FirstName).ValueFrom(f => f.ValueFromRandomString(4, 8))
                .ForMember(x => x.Surname).ValueFrom(f => f.ValueFromCollection(_names))
                .ForMember(x => x.LogonCount).ValueFrom(f => f.ValueFromRandom(0, 5));

            var users = dataConfiguration.Get<User>().Generate(5);

            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.Equal(5, users.Count());
            Assert.Contains(users.First().Surname, _names);
            Assert.NotEqual(Guid.Empty, users.First().Id);
        }
    }
}
