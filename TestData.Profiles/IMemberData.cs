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
        TProperty GetValue(TType data);
    }

    public interface ICompleteMemberData : IMemberData
    {
        object GetValue(object instance);
    }

    public interface IIncompleteMemberData<TType, TProperty> : IMemberData
    {
        ICompleteMemberData<TType, TProperty> ValueFrom(IValueCreator valueCreator);
        ICompleteMemberData<TType, TProperty> ValueFrom(Func<IValueCreatorFactory<TType, TProperty>, IValueCreator> valueCreator);
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

        public TProperty GetValue(TType data)
        {
            throw new NotImplementedException();
        }

        public IIncompleteMemberData<TType, TNextProperty> ForMember<TNextProperty>(Expression<Func<TType, TNextProperty>> member)
        {
            return DataProfile<TType>.CreateMemberData<TType, TNextProperty>(member, (DataProfile<TType>)DataProfile);
        }

        public IEnumerable<TType> Generate(int count)
        {
            return DataProfile<TType>.Generate((DataProfile<TType>)DataProfile, count);
        }

        public object GetValue(object instance)
        {
            if (instance as TType == null)
                throw new ArgumentException("instance not of correct type", "instance");

            return _valueCreator.CreateValue(instance); 
        }
    }

    public class IncompleteMemberData<TType, TProperty> : MemberData, IIncompleteMemberData<TType, TProperty> where TType : class
    {
        public IncompleteMemberData(PropertyInfo propertyInfo, IDataProfile dataProfile)
            : base(propertyInfo, dataProfile)
        {
            _valueCreatorFactory = new ValueCreatorFactory<TType, TProperty>();
        }

        IValueCreatorFactory<TType, TProperty> _valueCreatorFactory;

        public ICompleteMemberData<TType, TProperty> ValueFrom(IValueCreator valueCreator)
        {
            return new CompleteMemberData<TType, TProperty>(PropertyInfo, valueCreator, (DataProfile<TType>)DataProfile);
        }

        public ICompleteMemberData<TType, TProperty> ValueFrom(Func<IValueCreatorFactory<TType, TProperty>, IValueCreator> valueCreator)
        {
            return ValueFrom(valueCreator(_valueCreatorFactory));
        }
    }
}
