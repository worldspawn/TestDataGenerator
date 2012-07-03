using System;
using System.Reflection;

namespace TestData.Profiles
{
    public class CompleteMemberData<TType, TProperty> : MemberData, ICompleteMemberData<TType, TProperty> where TType : class
    {
        public CompleteMemberData(PropertyInfo propertyInfo, IValueCreator valueCreator, DataProfile<TType> dataProfile)
            : base(propertyInfo, dataProfile)
        {
            _valueCreator = valueCreator;
        }

        private readonly IValueCreator _valueCreator;

        public object GetValue(object instance)
        {
            if (instance as TType == null)
                throw new ArgumentException("instance not of correct type", "instance");

            return _valueCreator.CreateValue(instance); 
        }
    }
}