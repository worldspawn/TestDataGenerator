using System;

namespace TestData.Profiles.ValueCreators
{
    public class DecimalRandomValueCreator : RandomValueCreator<decimal>
    {
        public DecimalRandomValueCreator(decimal start, decimal end) : base(start, end) { }

        [ThreadStatic]
        private static Random _rand;

        private static Random Random
        {
            get { return _rand ?? (_rand = new Random()); }
        }

        public override object CreateValue(object instance, IProfileResolver profileResolver)
        {
            var start = Convert.ToDouble(_start);
            var end = Convert.ToDouble(_end);

            return Convert.ToDecimal(start + Random.NextDouble() * (end - start));
        }
    }
}