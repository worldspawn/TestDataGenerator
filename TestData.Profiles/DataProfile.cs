using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TestData.Profiles.ValueCreators;

namespace TestData.Profiles
{
    public class DataProfile<TType> : DataProfile, IDataProfile<TType> where TType : class
    {
        public DataProfile(Func<object> constructor, DataConfiguration dataConfiguration)
            : base(typeof(TType), constructor, dataConfiguration)
        {
        }

        public IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path)
        {
            var memberData = CreateMemberData<TType, TProperty>(path, new PathValueCreator(GetFromExpression(path)), this);
            _memberData[memberData.PropertyInfo] = memberData;
            return this;
        }

        public IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int exactly) where TProperty : System.Collections.IEnumerable
        {
            throw new NotImplementedException();
        }

        public IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int from, int to) where TProperty : System.Collections.IEnumerable
        {
            throw new NotImplementedException();
        }

        public IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, Func<IValueCreatorFactory<TType, TProperty>, IValueCreator> valueCreator)
        {
            var memberData = CreateMemberData(member, valueCreator, this);
            _memberData[memberData.PropertyInfo] = memberData;
            return this;
        }

        public IDataProfile<TType> ForMember(PropertyInfo propertyInfo, IValueCreator valueCreator)
        {
            _memberData[propertyInfo] = new CompleteMemberData<TType>(propertyInfo, valueCreator, this);
            return this;
        }

        public IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, IValueCreator valueCreator)
        {
            var memberData = CreateMemberData(member, valueCreator, this);
            _memberData[memberData.PropertyInfo] = memberData;
            return this;
        }

        public IEnumerable<TType> Generate(int count)
        {
            return Generate(this, count);
        }

        public TType Generate()
        {
            return Generate(this);
        }

        public IDataProfile<TType> CloneInto(DataConfiguration dataConfiguration)
        {
            var dataProfile = new DataProfile<TType>(_constructor, dataConfiguration);
            foreach (var md in _memberData)
                dataProfile.ForMember(md.Key, md.Value.ValueCreator);

            dataConfiguration.Add(dataProfile);

            return dataProfile;
        }

        public static TDataType Generate<TDataType>(DataProfile<TDataType> dataProfile) where TDataType : class
        {
            return Generate<TDataType>(dataProfile, 1).First();
        }

        public static IEnumerable<TDataType> Generate<TDataType>(DataProfile<TDataType> dataProfile, int count) where TDataType : class
        {
            return DataProfile.Generate(dataProfile, count).OfType<TDataType>();
        }

        public static PropertyInfo GetFromExpression<TDataType, TProperty>(Expression<Func<TDataType, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var ubody = (UnaryExpression)expression.Body;
                memberExpression = ubody.Operand as MemberExpression;
            }

            if (memberExpression == null || (memberExpression.Member as PropertyInfo) == null)
                throw new ArgumentException(string.Format("The expression {0} is not a member expression for a property", expression));

            return (PropertyInfo)memberExpression.Member;
        }

        public static ICompleteMemberData CreateMemberData<TDataType, TProperty>(Expression<Func<TDataType, TProperty>> expression, Func<IValueCreatorFactory<TType, TProperty>, IValueCreator> valueCreator, DataProfile<TDataType> dataProfile) where TDataType : class
        {
            IValueCreatorFactory<TType, TProperty> valueCreatorFactory = new ValueCreatorFactory<TType, TProperty>();
            return CreateMemberData(expression, valueCreator(valueCreatorFactory), dataProfile);
        }

        public static ICompleteMemberData CreateMemberData<TDataType, TProperty>(Expression<Func<TDataType, TProperty>> expression, IValueCreator valueCreator, DataProfile<TDataType> dataProfile) where TDataType : class
        {
            var memberData = new CompleteMemberData<TDataType>(GetFromExpression(expression), valueCreator, dataProfile);
            return memberData;
        }
    }

    public abstract class DataProfile : IDataProfile
    {
        protected DataProfile(Type type, Func<object> constructor, DataConfiguration dataConfiguration)
        {
            _type = type;
            _memberData = new Dictionary<PropertyInfo, ICompleteMemberData>();
            _constructor = constructor;
            _dataConfiguration = dataConfiguration;
        }

        private readonly DataConfiguration _dataConfiguration;
        internal readonly Func<object> _constructor;
        private readonly Type _type;
        internal readonly IDictionary<PropertyInfo, ICompleteMemberData> _memberData;

        public Type Type
        {
            get { return _type; }
        }

        internal IDictionary<PropertyInfo, ICompleteMemberData> MemberData
        {
            get { return _memberData; }
        }

        public DataConfiguration DataConfiguration
        {
            get { return _dataConfiguration; }
        }

        object IDataProfile.Generate()
        {
            var enumerator = Generate(this, 1).GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;

            throw new InvalidOperationException("no result");
        }

        IEnumerable IDataProfile.Generate(int count)
        {
            return Generate(this, count);
        }

        public static IEnumerable Generate(DataProfile dataProfile, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = dataProfile._constructor();
                foreach (var pair in dataProfile._memberData)
                    pair.Key.SetValue(item, pair.Value.GetValue(item, dataProfile.DataConfiguration));
                yield return item;
            }
        }
    }
}