using System;
using System.Collections.Generic;
using System.Linq;
using TestData.Profiles.Tests.TestClasses;
using TestData.Profiles.ValueCreators;
using Xunit;

namespace TestData.Profiles.Tests
{
    public class DataConfigurationTests
    {
        private readonly DataConfiguration _dataConfiguration;

        private readonly string[] _names = new[] {
            "Smith", "Jones", "Williams", "Brown", "Wilson", "Taylor"
        },
        _roleNames = new[]{
            "Administrator", "Manager", "Clerk"
        },
        _productNames = new[] { "Apples", "Oranges", "Pears" };

        

        public DataConfigurationTests()
        {
            var roles = new DataProfile<Role>(() => new Role())
                .ForMember(x => x.Id, (r) => Guid.NewGuid())
                .GenerateForEach(_dataConfiguration, _roleNames, (name, role) => role.Name = name).ToList();

            var products = new DataProfile<Product>(() => new Product())
                .ForMember(x => x.Id, (r) => Guid.NewGuid())
                .ForMember(x=>x.Amount, new DecimalRandomValueCreator(5, 500))
                .GenerateForEach(_dataConfiguration, _productNames, (name, product) => product.Name = name).ToList();

            _dataConfiguration = new DataConfiguration();
            _dataConfiguration.CreateProfileFor(() => new OrderItem())
                .ForMember(x => x.Product, new CollectionItemValueCreator<Product>(products))
                .ForMember(x => x.Quantity, new IntRandomValueCreator(1, 10));

            _dataConfiguration.CreateProfileFor(() => new Order())
                .ForMember(x => x.CreatedOn, (x) => DateTime.UtcNow)
                .FollowPath(x => x.Items, 1, (o) => new OrderItem(o));

            _dataConfiguration.CreateProfileFor(() => new User())
                .FollowPath(x => x.Role)
                .ForMember(x => x.Id, (u) => Guid.NewGuid())
                .ForMember(x => x.FirstName, new RandomStringValueCreator(4, 8))
                .ForMember(x => x.Surname, new CollectionItemValueCreator<string>(_names))
                .ForMember(x => x.Role, new CollectionItemValueCreator<Role>(roles))
                .ForMember(x => x.LogonCount, new IntRandomValueCreator(1, 200))
                .FollowPath(x => x.Orders, 3, 200, (u) => new Order(u));
        }

        [Fact]
        public void CanCreate1000Users()
        {
            IEnumerable<User> users = _dataConfiguration.Get<User>().Generate(_dataConfiguration, 1000);
            int count = users.Count();
            Assert.Equal(1000, count);
        }

        [Fact]
        public void CanCreateUsers()
        {
            IEnumerable<User> users = _dataConfiguration.Get<User>().Generate(_dataConfiguration, 5);

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
            
            IDataProfile<User> userProfile = _dataConfiguration.Get<User>();
            userProfile = userProfile.CloneInto(dc);
            userProfile
                .ForMember(x => x.FirstName, "Jimmy")
                .ForMember(x => x.Role)
                .ForMember(x => x.Orders);

            User user = dc.Get<User>().Generate(dc);
            Assert.NotNull(user);
            Assert.Null(user.Orders);
        }

        [Fact]
        public void UsingEmptyCollectionValueCreatorFails()
        {
            var dc = new DataConfiguration();
            IDataProfile<User> userProfile = _dataConfiguration.Get<User>();
            userProfile = userProfile.CloneInto(dc);
            userProfile
                .ForMember(x => x.Surname, new CollectionItemValueCreator<string>(new string[0]))
                .ForMember(x => x.Role);

            Assert.Throws<InvalidOperationException>(() => dc.Get<User>().Generate(dc));
        }

        [Fact]
        public void CanFollowEnumerablePathWithRandomReferenceCount()
        {
            User user = _dataConfiguration.Get<User>().Generate(_dataConfiguration);
            Assert.NotNull(user);
            Assert.True(user.Orders.Count() <= 200);
            Assert.True(user.Orders.Count() >= 3);
        }

        [Fact]
        public void InvalidExpressionTypesFail()
        {
            IDataProfile<User> userProfile = _dataConfiguration.Get<User>();
            Assert.Throws<ArgumentException>(() => userProfile.ForMember(u => u.LogonCount.GetType(), GetType()));
            Assert.Throws<ArgumentException>(() => userProfile.ForMember(u => u.LogonCount == 4, true));
        }

        [Fact]
        public void UnaryExpressionsSupported()
        {
            IDataProfile<User> userProfile = _dataConfiguration.Get<User>();
            userProfile.ForMember(u => (object)u.LogonCount, 2);
        }

        [Fact]
        public void GenerateOneForEach()
        {
            var dc = new DataConfiguration();
            var products = dc.CreateProfileFor(() => new Product())
                .GenerateForEach(dc, _productNames, (name, product) =>
                                                       {
                                                           product.Name = name;
                                                       }).ToList();

            Assert.Equal(3, products.Count);
            foreach(var name in _productNames)
                Assert.Contains(name, products.Select(p=>p.Name));

        }
    }
}