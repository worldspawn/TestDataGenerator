using System;
using System.Collections.Generic;
using System.Linq;
using TestData.Profiles.Tests.TestClasses;
using Xunit;

namespace TestData.Profiles.Tests
{
    public class DataConfigurationTests
    {
        private readonly DataConfiguration _dataConfiguration;

        private readonly string[] _names = new[]
                                               {
                                                   "Smith", "Jones", "Williams", "Brown", "Wilson", "Taylor"
                                               };

        public DataConfigurationTests()
        {
            _dataConfiguration = new DataConfiguration();
            _dataConfiguration.CreateProfileFor(() => new Role())
                .ForMember(x => x.Id, f => f.ValueFromExpression((u) => Guid.NewGuid()))
                .ForMember(x => x.Name, f => f.ValueFromRandomString(4, 8));

            _dataConfiguration.CreateProfileFor(() => new User())
                .FollowPath(x => x.Role)
                .ForMember(x => x.Id, f => f.ValueFromExpression((u) => Guid.NewGuid()))
                .ForMember(x => x.FirstName, f => f.ValueFromRandomString(4, 8))
                .ForMember(x => x.Surname, f => f.ValueFromCollection(_names))
                .ForMember(x => x.LogonCount, f => f.ValueFromRandom(0, 5));
        }

        [Fact]
        public void CanCreate50000Users()
        {
            IEnumerable<User> users = _dataConfiguration.Get<User>().Generate(50000);

            users.Count();
        }

        [Fact]
        public void CanCreateUsers()
        {
            IEnumerable<User> users = _dataConfiguration.Get<User>().Generate(5);

            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.Equal(5, users.Count());
            User first = users.First();
            Assert.Contains(first.Surname, _names);
            Assert.NotEqual(Guid.Empty, users.First().Id);
            Assert.NotNull(first.Role);
        }

        [Fact]
        public void CanCloneAndFiddle()
        {
            var dc = new DataConfiguration();
            dc.CreateProfileFor(() => new Foo())
                .ForMember(x => x.Id, f => f.ValueFromExpression((u) => Guid.NewGuid()));

            IDataProfile<User> userProfile = _dataConfiguration.Get<User>();
            userProfile = userProfile.CloneInto(dc);
            userProfile
                .ForMember(x => x.FirstName, f => f.ValueFromConstant("Jimmy"))
                .ForMember(x => x.Role, f => f.ValueFromConstant(null))
                .FollowPath(x => x.Friends, 2);

            var user = dc.Get<User>().Generate();
            Assert.NotNull(user);
            Assert.Equal(2, user.Friends.Count);
        }
    }
}