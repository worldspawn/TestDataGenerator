using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Profiles
{
    public interface IDataProfile
    {
        Type Type { get; }
    }

    public interface IDataProfile<TType> : IDataProfile, IContinuable<TType> where TType : class
    {
        IDataProfile<TType> FollowPath<TProperty>(Func<TType, TProperty> path);
        IDataProfile<TType> FollowPath<TProperty>(Func<TType, TProperty> path, int exactly) where TProperty : System.Collections.IEnumerable;
        IDataProfile<TType> FollowPath<TProperty>(Func<TType, TProperty> path, int from, int to) where TProperty : System.Collections.IEnumerable;

    }

    public abstract class DataProfile : IDataProfile
    {
        protected DataProfile(Type type)
        {
            _type = type;
        }

        private readonly Type _type;

        public Type Type
        {
            get { return _type; }
        }
    }

    public class DataProfile<TType> : DataProfile, IDataProfile<TType> where TType : class
    {
        public DataProfile(Func<TType> constructor)
            : base(typeof(TType))
        {
            _constructor = constructor;
            _memberData = new Dictionary<PropertyInfo, ICompleteMemberData>();
        }

        private readonly Func<TType> _constructor;
        private readonly IDictionary<PropertyInfo, ICompleteMemberData> _memberData;

        internal IDictionary<PropertyInfo, ICompleteMemberData> MemberData
        {
            get { return _memberData; }
        }

        public IDataProfile<TType> FollowPath<TProperty>(Func<TType, TProperty> path)
        {
            throw new NotImplementedException();
        }

        public IDataProfile<TType> FollowPath<TProperty>(Func<TType, TProperty> path, int exactly) where TProperty : System.Collections.IEnumerable
        {
            throw new NotImplementedException();
        }

        public IDataProfile<TType> FollowPath<TProperty>(Func<TType, TProperty> path, int from, int to) where TProperty : System.Collections.IEnumerable
        {
            throw new NotImplementedException();
        }

        public IIncompleteMemberData<TType, TProperty> ForMember<TProperty>(Expression<Func<TType, TProperty>> member)
        {
            return CreateMemberData<TType, TProperty>(member, this);
        }

        public IEnumerable<TType> Generate(int count)
        {
            return Generate(this, count);
        }

        public static IEnumerable<TDataType> Generate<TDataType>(DataProfile<TDataType> dataProfile, int count) where TDataType : class
        {
            for (int i = 0; i < count; i++){
                var item = dataProfile._constructor();
                foreach (var pair in dataProfile._memberData)
                    pair.Key.SetValue(item, pair.Value.GetValue(item));
                yield return item;
            }
        }

        public static IIncompleteMemberData<TDataType, TProperty> CreateMemberData<TDataType, TProperty>(Expression<Func<TDataType, TProperty>> expression, DataProfile<TDataType> dataProfile) where TDataType : class
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                UnaryExpression ubody = (UnaryExpression)expression.Body;
                memberExpression = ubody.Operand as MemberExpression;
            }

            if (memberExpression == null || (memberExpression.Member as PropertyInfo) == null)
                throw new ArgumentException(string.Format("The Expression {0} is not a member expression for a property", expression));

            var memberData = new IncompleteMemberData<TDataType, TProperty>((PropertyInfo)memberExpression.Member, dataProfile);            
            
            return memberData;
        }
    }
}
