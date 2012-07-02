using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Profiles
{
    public interface IMemberData
    {
        PropertyInfo PropertyInfo { get; }
    }

    public interface ICompleteMemberData<TType, TProperty> : ICompleteMemberData
    {
        //TProperty GetValue(TType data);
    }

    public interface ICompleteMemberData : IMemberData
    {
        object GetValue(object instance);
    }

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

    public class CompleteMemberData<TType, TProperty> : MemberData, ICompleteMemberData<TType, TProperty> where TType : class
    {
        public CompleteMemberData(PropertyInfo propertyInfo, IValueCreator valueCreator, DataProfile<TType> dataProfile)
            : base(propertyInfo, dataProfile)
        {
            dataProfile.MemberData[propertyInfo] = this;
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
