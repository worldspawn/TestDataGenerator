using System;
using System.Collections.Generic;

namespace TestData.Profiles.ValueCreators
{
    public class ProfileResolver : IProfileResolver
    {
        private readonly IDictionary<Type, IDataProfile> _profiles = new Dictionary<Type, IDataProfile>();

        public IDataProfile Get(Type type)
        {
            return _profiles[type];
        }

        public IDataProfile<TType> Get<TType>() where TType : class
        {
            return (IDataProfile<TType>) _profiles[typeof (TType)];
        }

        public void Add<TType>(DataProfile<TType> dataProfile) where TType : class
        {
            _profiles[typeof (TType)] = dataProfile;
        }
    }
}