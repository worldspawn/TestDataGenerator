Have cleaned up generating and dependencies
Now supporting profile cloning. However you could still break it with silly use of expressions.

examples are in the tests

Can now create big graphs and mix/match and modify them.

```csharp
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
```

Note self referencing follow paths will cause a stackoverflow.