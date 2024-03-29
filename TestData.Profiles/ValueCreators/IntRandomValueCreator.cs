using System;

namespace TestData.Profiles.ValueCreators
{
    public class IntRandomValueCreator : RandomValueCreator<int>
    {
        public IntRandomValueCreator(int start, int end) : base(start, end) { }

        [ThreadStatic]
        private static Random _rand = new Random();

        public override object CreateValue(object instance, IProfileResolver profileResolver)
        {
            return _rand.Next(_start, _end);
        }
    }
}