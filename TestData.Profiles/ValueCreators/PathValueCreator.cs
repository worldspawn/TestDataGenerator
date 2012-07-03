using System.Reflection;

namespace TestData.Profiles.ValueCreators
{
    public class PathValueCreator : IValueCreator
    {
        private readonly PropertyInfo _propertyInfo;
        
        public PathValueCreator(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public object CreateValue(object instance, DataConfiguration dataConfiguration)
        {
            var dataProfile = dataConfiguration.Get(_propertyInfo.PropertyType);
            var reference = dataProfile.Generate();
            return reference;
        }
    }
}