using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Profiles
{
    public interface IContinuable<TType>
    {
        ICompleteMemberData<TType, TProperty> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, IValueCreator valueCreator);
        ICompleteMemberData<TType, TProperty> ForMember<TProperty>(Expression<Func<TType, TProperty>> member, Func<IValueCreatorFactory<TType, TProperty>, IValueCreator> valueCreator);
        IEnumerable<TType> Generate(int count);
        TType Generate();
    }
}
