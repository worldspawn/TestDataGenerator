using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TestData.Profiles.Helpers
{
    public static class ExpressionHelpers
    {
        public static PropertyInfo GetFromExpression<TDataType, TProperty>(
            Expression<Func<TDataType, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var ubody = (UnaryExpression) expression.Body;
                memberExpression = ubody.Operand as MemberExpression;
            }

            if (memberExpression == null || (memberExpression.Member as PropertyInfo) == null)
                throw new ArgumentException(string.Format(
                    "The expression {0} is not a member expression for a property", expression));

            return (PropertyInfo) memberExpression.Member;
        }
    }
}