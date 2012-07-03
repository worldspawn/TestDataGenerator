namespace TestData.Profiles.ValueCreators
{
    public class ConstantValueCreator<TProperty> : IValueCreator
    {
        private readonly TProperty _value;

        public ConstantValueCreator(TProperty value)
        {
            _value = value;
        }

        public object CreateValue(object instance, DataConfiguration dataConfiguration)
        {
            return _value;
        }
    }
}