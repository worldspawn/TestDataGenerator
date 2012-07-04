using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace TestData.Profiles.ValueCreators
{
    public class PathValueCreator : IValueCreator
    {
        protected readonly Type PropertyType;
        
        public PathValueCreator(PropertyInfo propertyInfo) : this(propertyInfo.PropertyType)
        {}

        protected PathValueCreator(Type propertyType)
        {
            PropertyType = propertyType;
        }

        public virtual object CreateValue(object instance, DataConfiguration dataConfiguration)
        {
            var dataProfile = dataConfiguration.Get(PropertyType);
            var reference = dataProfile.Generate();
            return reference;
        }
    }

    public class EnumerablePathValueCreator : PathValueCreator
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly int _from;
        private readonly int _to;
        [ThreadStatic] private static Random _rand;

        private static Random Random { get
        {
            if (_rand == null)
                _rand = new Random();

            return _rand;
        }}

        public EnumerablePathValueCreator(PropertyInfo propertyInfo, int from, int to)
            : base(propertyInfo.PropertyType.GetGenericArguments()[0])
        {
            _propertyInfo = propertyInfo;
            _from = @from;
            _to = to;
        }

        public override object CreateValue(object instance, DataConfiguration dataConfiguration)
        {
            //var typeArgs = PropertyInfo.PropertyType.GetGenericArguments();
            var list = Activator.CreateInstance(_propertyInfo.PropertyType) as IList;
            var amount = _to;
            if (_from != _to)
                amount = Random.Next(_from, _to);

            for (var i = 0; i < amount; i++)
                list.Add(base.CreateValue(instance, dataConfiguration));

            return list;
        }
    }
}