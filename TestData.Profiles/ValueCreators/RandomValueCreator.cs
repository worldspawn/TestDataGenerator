namespace TestData.Profiles.ValueCreators
{
    public abstract class RandomValueCreator<TProperty> : IValueCreator
    {
        public RandomValueCreator(TProperty start, TProperty end)
        {
            _start = start;
            _end = end;
        }

        protected TProperty _start, _end;

        public abstract object CreateValue(object instance, DataConfiguration dataConfiguration);
    }
}