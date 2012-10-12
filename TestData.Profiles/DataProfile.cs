using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TestData.Profiles.Helpers;
using TestData.Profiles.ValueCreators;

namespace TestData.Profiles
{
    public class DataProfile<TType> : DataProfile, IDataProfile<TType> where TType : class
    {
        public DataProfile(Func<object> constructor)
            : base(typeof (TType), constructor)
        {
        }

        #region IDataProfile<TType> Members

        public IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, Func<TType, object> constructor = null)
        {
            Func<object, object> constructorCaller = null;
            if (constructor != null)
                constructorCaller = (instance) => constructor((TType)instance);

            var memberData = new PathValueCreator(ExpressionHelpers.GetFromExpression(path), constructorCaller).CreateMemberData(path);
            MemberData[memberData.PropertyInfo] = memberData;
            return this;
        }

        public IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int exactly, Func<TType, object> constructor = null)
            where TProperty : IEnumerable
        {
            return FollowPath(path, exactly, exactly, constructor);
        }

        public IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int from, int to, Func<TType, object> constructor = null)
            where TProperty : IEnumerable
        {
            Func<object, object> constructorCaller = null;
            if (constructor != null)
                constructorCaller = (instance) => constructor((TType) instance);

            var memberData =
                new EnumerablePathValueCreator(ExpressionHelpers.GetFromExpression(path), from, to, constructorCaller)
                .CreateMemberData(path);
            MemberData[memberData.PropertyInfo] = memberData;
            return this;
        }

        public IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member,
                                                        IValueCreator valueCreator)
        {
            var memberData = valueCreator.CreateMemberData(member);
            MemberData[memberData.PropertyInfo] = memberData;
            return this;
        }

        public IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member)
        {
            return ForMember(member, new ConstantValueCreator<TProperty>(default(TProperty)));
        }

        public IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, TProperty value)
        {
            return ForMember(member, new ConstantValueCreator<TProperty>(value));
        }

        public IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, Func<TType, TProperty> expression)
        {
            return ForMember(member, new ExpressionValueCreator<TType, TProperty>(expression));
        }

        public new IEnumerable<TType> Generate(IProfileResolver profileResolver, int count)
        {
            return base.Generate(profileResolver, count).OfType<TType>();
        }
         
        public new TType Generate(IProfileResolver profileResolver)
        {
            return (TType)base.Generate(profileResolver);
        }

        public IDataProfile<TType> CloneInto(IProfileResolver profileResolver)
        {
            var dataProfile = new DataProfile<TType>(Constructor);
            foreach (var md in MemberData)
                dataProfile.ForMember(md.Key, md.Value.ValueCreator);

            profileResolver.Add(dataProfile);

            return dataProfile;
        }

        public IEnumerable<TType> GenerateForEach<TEnumeratedType>(IProfileResolver profileResolver, IEnumerable<TEnumeratedType> items, Action<TEnumeratedType, TType> action)
        {
            foreach(var item in items)
            {
                var generatedItem = Generate(profileResolver);
                action(item, generatedItem);
                yield return generatedItem;
            }
        }

        #endregion

        public IDataProfile<TType> ForMember(PropertyInfo propertyInfo, IValueCreator valueCreator)
        {
            MemberData[propertyInfo] = new MemberData(propertyInfo, valueCreator);
            return this;
        }
    }

    public abstract class DataProfile : IDataProfile
    {
        private readonly Func<object> _constructor;
        private readonly IDictionary<PropertyInfo, IMemberData> _memberData;
        private readonly Type _type;

        protected DataProfile(Type type, Func<object> constructor)
        {
            _type = type;
            _memberData = new Dictionary<PropertyInfo, IMemberData>();
            _constructor = constructor;
        }

        internal IDictionary<PropertyInfo, IMemberData> MemberData
        {
            get { return _memberData; }
        }

        #region IDataProfile Members

        public Func<object> Constructor
        {
            get { return _constructor; }
        }

        public object Generate(IProfileResolver profileResolver, object referringInstance, Func<object, object> constructor = null)
        {
            object item;
            if (constructor == null)
                item = _constructor();
            else
                item = constructor(referringInstance);

            foreach (var pair in _memberData)
                pair.Key.SetValue(item, pair.Value.GetValue(item, profileResolver), null);

            return item;
        }

        public object Generate(IProfileResolver profileResolver)
        {
            IEnumerator enumerator = Generate(profileResolver, 1).GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;

            throw new InvalidOperationException("no result");
        }

        public IEnumerable Generate(IProfileResolver profileResolver, int count)
        {
            for (int i = 0; i < count; i++)
            {
                object item = _constructor();
                foreach (var pair in _memberData)
                    pair.Key.SetValue(item, pair.Value.GetValue(item, profileResolver), null);
                yield return item;
            }
        }

        #endregion
    }
}