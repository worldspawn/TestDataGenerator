using System;

namespace TestData.Profiles.ValueCreators
{
    public interface IProfileResolver
    {
        IDataProfile Get(Type type);
        IDataProfile<TType> Get<TType>() where TType : class;
        void Add<TType>(DataProfile<TType> dataProfile) where TType : class;
    }
}