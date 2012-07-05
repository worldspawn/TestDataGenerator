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
        object Generate(IProfileResolver profileResolver);
        IEnumerable Generate(IProfileResolver profileResolver, int count);
        Func<object> Constructor { get; }
    }

    public interface IDataProfile<TType> : IDataProfile where TType : class
    {
        new TType Generate(IProfileResolver profileResolver);
        new IEnumerable<TType> Generate(IProfileResolver profileResolver, int count);
        IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path);
        IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int exactly) where TProperty : System.Collections.IEnumerable;
        IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int from, int to) where TProperty : System.Collections.IEnumerable;
        IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, IValueCreator valueCreator);
        IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, TProperty value);
        IDataProfile<TType> CloneInto(IProfileResolver dataConfiguration);
    }
}
