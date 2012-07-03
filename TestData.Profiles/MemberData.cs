using System.Reflection;

namespace TestData.Profiles
{
    public abstract class MemberData : IMemberData
    {
        protected MemberData(PropertyInfo propertyInfo, IDataProfile dataProfile)
        {
            _propertyInfo = propertyInfo;
            _dataProfile = dataProfile;
        }

        private readonly PropertyInfo _propertyInfo;
        private readonly IDataProfile _dataProfile;

        protected IDataProfile DataProfile { get { return _dataProfile; } }

        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
        }
    }
}