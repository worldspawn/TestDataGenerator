using System;
using System.Collections.Generic;
using System.Linq;
using TestData.Profiles.Tests.TestClasses;
using TestData.Profiles.ValueCreators;
using Xunit;
using TestData.Profiles.Helpers;

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
                .ForMember(x => x.Id, (r) => Guid.NewGuid())
                .ForMember(x => x.Name, new RandomStringValueCreator(4, 8));

            _dataConfiguration.CreateProfileFor(() => new User())
                .FollowPath(x => x.Role)
                .ForMember(x => x.Id, (u) => Guid.NewGuid())
                .ForMember(x => x.FirstName, new RandomStringValueCreator(4, 8))
                .ForMember(x => x.Surname, new CollectionItemValueCreator<string>(_names))
                .ForMember(x => x.LogonCount, new IntRandomValueCreator(0, 5));
        }

        [Fact]
        public void CanCreate50000Users()
        {
            var users = _dataConfiguration.Get<User>().Generate(_dataConfiguration, 50000);
            var count = users.Count();
            Assert.Equal(50000, count);
        }

        [Fact]
        public void CanCreateUsers()
        {
            var users = _dataConfiguration.Get<User>().Generate(_dataConfiguration, 5);

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
                .ForMember(x => x.Id, (f) => Guid.NewGuid());

            IDataProfile<User> userProfile = _dataConfiguration.Get<User>();
            userProfile = userProfile.CloneInto(dc);
            userProfile
                .ForMember(x => x.FirstName, "Jimmy")
                .ForMember(x => x.Role)
                .FollowPath(x => x.Friends, 2);

            var user = dc.Get<User>().Generate(dc);
            Assert.NotNull(user);
            Assert.Equal(2, user.Friends.Count);
        }
    }
}