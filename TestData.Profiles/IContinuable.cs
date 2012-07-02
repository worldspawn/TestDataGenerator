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
        IIncompleteMemberData<TType, TProperty> ForMember<TProperty>(Expression<Func<TType, TProperty>> member);
        IEnumerable<TType> Generate(int count);
        TType Generate();
    }
}
