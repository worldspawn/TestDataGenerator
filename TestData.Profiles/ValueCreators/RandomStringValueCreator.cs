using System;
using System.Text;

namespace TestData.Profiles.ValueCreators
{
    public class RandomStringValueCreator : IValueCreator
    {
        public RandomStringValueCreator(int minLength, int maxLength) : this(maxLength)
        {
            _minLength = minLength;
        }

        public RandomStringValueCreator(int length)
        {
            _length = length;
        }

        private int _length;
        private int? _minLength;
        [ThreadStatic]
        private static Random _rand = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public object CreateValue(object instance, IProfileResolver profileResolver)
        {
            var sb = new StringBuilder();
            int length = _length;
            if (_minLength.HasValue)
                length = _rand.Next(_minLength.Value, length);

            while (sb.Length < length)
                sb.Append(_chars[_rand.Next(0, _chars.Length - 1)]);

            return sb.ToString();
        }
    }
}