using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace TestData.Profiles.ValueCreators
{
    public class PathValueCreator : IValueCreator
    {
        protected readonly Type PropertyType;
        private readonly Func<object, object> _constructor;

        public PathValueCreator(PropertyInfo propertyInfo, Func<object, object> constructor)
            : this(propertyInfo.PropertyType, constructor)
        {}

        protected PathValueCreator(Type propertyType, Func<object, object> constructor)
        {
            PropertyType = propertyType;
            _constructor = constructor;
        }

        public virtual object CreateValue(object instance, IProfileResolver profileResolver)
        {
            var dataProfile = profileResolver.Get(PropertyType);
            var reference = dataProfile.Generate(profileResolver, instance, _constructor);
            return reference;
        }
    }

    public class EnumerablePathValueCreator : PathValueCreator
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly int _from;
        private readonly int _to;
        [ThreadStatic] private static Random _rand;

        private static Random Random { 
            get
            {
                if (_rand == null)
                    _rand = new Random();

                return _rand;
            }   
        }

        public EnumerablePathValueCreator(PropertyInfo propertyInfo, int from, int to, Func<object, object> constructor)
            : base(propertyInfo.PropertyType.GetGenericArguments()[0], constructor)
        {
            _propertyInfo = propertyInfo;
            _from = @from;
            _to = to;
        }

        public override object CreateValue(object instance, IProfileResolver profileResolver)
        {
            var list = Activator.CreateInstance(_propertyInfo.PropertyType) as IList;
            var amount = _to;
            if (_from != _to)
                amount = Random.Next(_from, _to);

            for (var i = 0; i < amount; i++)
                list.Add(base.CreateValue(instance, profileResolver));

            return list;
        }
    }
}