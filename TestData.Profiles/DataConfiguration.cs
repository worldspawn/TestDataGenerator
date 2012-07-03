using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Profiles
{
    public class DataConfiguration
    {
        private readonly IDictionary<Type, IDataProfile> _profiles = new Dictionary<Type, IDataProfile>();

        public IDataProfile Get(Type type)
        {
            return _profiles[type];
        }

        public IDataProfile<TType> Get<TType>() where TType : class
        {
            return (IDataProfile<TType>)_profiles[typeof(TType)];
        }

        public IDataProfile<TType> CreateProfileFor<TType>(Func<TType> constructor) where TType : class
        {
            var profile = new DataProfile<TType>(constructor, this);
            _profiles.Add(typeof(TType), profile);

            return profile;
        }

        public void Add<TType>(DataProfile<TType> dataProfile) where TType : class
        {
            _profiles[typeof(TType)] = dataProfile;
        }
    }
}
