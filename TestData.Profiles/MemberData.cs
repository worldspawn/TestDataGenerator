using System;
using System.Reflection;
using TestData.Profiles.ValueCreators;

namespace TestData.Profiles
{
    public class MemberData : IMemberData
    {
        public MemberData(PropertyInfo propertyInfo, IValueCreator valueCreator)
        {
            _propertyInfo = propertyInfo;
            _valueCreator = valueCreator;
        }

        private readonly IValueCreator _valueCreator;
        private readonly PropertyInfo _propertyInfo;

        public IValueCreator ValueCreator
        {
            get { return _valueCreator; }
        }
        
        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
        }

        public object GetValue(object instance, IProfileResolver profileResolver)
        {
            return ValueCreator.CreateValue(instance, profileResolver);
        }
    }
}