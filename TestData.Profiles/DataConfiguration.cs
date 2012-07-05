using System;
using TestData.Profiles.ValueCreators;

namespace TestData.Profiles
{
    public class DataConfiguration : ProfileResolver
    {
        public IDataProfile<TType> CreateProfileFor<TType>(Func<TType> constructor) where TType : class
        {
            var profile = new DataProfile<TType>(constructor);
            Add(profile);
            return profile;
        }
    }
}