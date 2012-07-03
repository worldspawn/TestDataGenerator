using System;

namespace TestData.Profiles.ValueCreators
{
    public class DecimalRandomValueCreator : RandomValueCreator<decimal>
    {
        public DecimalRandomValueCreator(decimal start, decimal end) : base(start, end) { }

        [ThreadStatic]
        private static Random _rand = new Random();

        public override object CreateValue(object instance, DataConfiguration dataConfiguration)
        {
            var start = Convert.ToDouble(_start);
            var end = Convert.ToDouble(_end);

            return Convert.ToDecimal(start + _rand.NextDouble() * (end - start));
        }
    }
}