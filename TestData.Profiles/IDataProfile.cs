using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestData.Profiles.ValueCreators;

namespace TestData.Profiles
{
    public interface IDataProfile
    {
        object Generate(IProfileResolver profileResolver, object referringInstance,
                        Func<object, object> constructor = null);
        object Generate(IProfileResolver profileResolver);
        IEnumerable Generate(IProfileResolver profileResolver, int count);
        Func<object> Constructor { get; }
    }

    public interface IDataProfile<TType> : IDataProfile where TType : class
    {
        new TType Generate(IProfileResolver profileResolver);
        new IEnumerable<TType> Generate(IProfileResolver profileResolver, int count);
        IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, Func<TType, object> constructor = null);
        IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int exactly, Func<TType, object> constructor = null) where TProperty : System.Collections.IEnumerable;
        IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int from, int to, Func<TType, object> constructor = null) where TProperty : System.Collections.IEnumerable;
        IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member);
        IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, IValueCreator valueCreator);
        IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, TProperty value);
        IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, Func<TType, TProperty> expression);
        IDataProfile<TType> CloneInto(IProfileResolver dataConfiguration);
        IEnumerable<TType> GenerateForEach<TEnumeratedType>(IProfileResolver profileResolver, IEnumerable<TEnumeratedType> items, Action<TEnumeratedType, TType> action);
    }
}
