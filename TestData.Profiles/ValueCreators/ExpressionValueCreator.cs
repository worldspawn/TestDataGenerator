using System;

namespace TestData.Profiles.ValueCreators
{
    public class ExpressionValueCreator<TType, TProperty> : IValueCreator where TType : class
    {
        private readonly Func<TType, TProperty> _expression;

        public ExpressionValueCreator(Func<TType, TProperty> expression)
        {
            _expression = expression;
        }

        public object CreateValue(object instance, DataConfiguration dataConfiguration)
        {
            if (instance == null)
                return default(TProperty);
            var value = _expression(instance as TType);
            return value;
        }
    }
}