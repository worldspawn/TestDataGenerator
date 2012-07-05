using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestData.Profiles.ValueCreators;

namespace TestData.Profiles.Helpers
{
    public static class ValueCreatorExtensionMethods
    {
        public static IMemberData CreateMemberData<TDataType, TProperty>(
            this IValueCreator valueCreator, Expression<Func<TDataType, TProperty>> expression) where TDataType : class
        {
            return new MemberData(ExpressionHelpers.GetFromExpression(expression), valueCreator);
        }
    }
}
