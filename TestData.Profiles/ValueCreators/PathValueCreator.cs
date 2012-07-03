using System.Reflection;

namespace TestData.Profiles.ValueCreators
{
    public class PathValueCreator : IValueCreator
    {
        private readonly PropertyInfo _propertyInfo;
        private DataConfiguration _dataConfiguration;
        
        public PathValueCreator(PropertyInfo propertyInfo, DataConfiguration dataConfiguration)
        {
            _propertyInfo = propertyInfo;
            _dataConfiguration = dataConfiguration;
        }

        public object CreateValue(object instance)
        {
            var dataProfile = _dataConfiguration.Get(_propertyInfo.PropertyType);
            var reference = dataProfile.Generate();
            return reference;
        }
    }
}