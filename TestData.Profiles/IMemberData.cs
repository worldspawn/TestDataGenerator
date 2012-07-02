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

    public interface ICompleteMemberData<TType, TProperty> : ICompleteMemberData, IContinuable<TType>
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


        public ICompleteMemberData<TType, TNextProperty> ForMember<TNextProperty>(Expression<Func<TType, TNextProperty>> member, Func<IValueCreatorFactory<TType, TNextProperty>, IValueCreator> valueCreator)
        {
            return DataProfile<TType>.CreateMemberData<TType, TNextProperty>(member, valueCreator, (DataProfile<TType>)DataProfile);
        }

        public ICompleteMemberData<TType, TNextProperty> ForMember<TNextProperty>(Expression<Func<TType, TNextProperty>> member, IValueCreator valueCreator)
        {
            return DataProfile<TType>.CreateMemberData<TType, TNextProperty>(member, valueCreator, (DataProfile<TType>)DataProfile);
        }

        public IEnumerable<TType> Generate(int count)
        {
            return DataProfile<TType>.Generate((DataProfile<TType>)DataProfile, count);
        }

        public TType Generate()
        {
            return DataProfile<TType>.Generate((DataProfile<TType>)DataProfile);
        }

        public object GetValue(object instance)
        {
            if (instance as TType == null)
                throw new ArgumentException("instance not of correct type", "instance");

            return _valueCreator.CreateValue(instance); 
        }
    }
}
