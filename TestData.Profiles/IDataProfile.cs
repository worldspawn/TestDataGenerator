using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Profiles
{
    public interface IDataProfile
    {
        Type Type { get; }
        object Generate();
        IEnumerable Generate(int count);
    }

    public interface IDataProfile<TType> : IDataProfile where TType : class
    {
        new TType Generate();
        new IEnumerable<TType> Generate(int count);
        IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path);
        IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int exactly) where TProperty : System.Collections.IEnumerable;
        IDataProfile<TType> FollowPath<TProperty>(Expression<Func<TType, TProperty>> path, int from, int to) where TProperty : System.Collections.IEnumerable;
        IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, Func<IValueCreatorFactory<TType, TProperty>, IValueCreator> valueCreator);
        IDataProfile<TType> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, IValueCreator valueCreator);
        IDataProfile<TType> CloneInto(DataConfiguration dataConfiguration);
    }
}
