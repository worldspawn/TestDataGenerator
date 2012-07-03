using System;
using System.Reflection;

namespace TestData.Profiles
{
    public class CompleteMemberData<TType> : MemberData, ICompleteMemberData where TType : class
    {
        public CompleteMemberData(PropertyInfo propertyInfo, IValueCreator valueCreator, DataProfile<TType> dataProfile)
            : base(propertyInfo, dataProfile)
        {
            _valueCreator = valueCreator;
        }

        private readonly IValueCreator _valueCreator;

        public IValueCreator ValueCreator
        {
            get { return _valueCreator; }
        }

        public object GetValue(object instance, DataConfiguration dataConfiguration)
        {
            if (instance as TType == null)
                throw new ArgumentException("instance not of correct type", "instance");

            return ValueCreator.CreateValue(instance, dataConfiguration);
        }
    }
}