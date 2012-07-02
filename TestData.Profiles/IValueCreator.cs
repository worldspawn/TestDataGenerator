using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Profiles
{
    public interface IValueCreator
    {
        object CreateValue(object instance);
    }

    public class RandomValueCreator<TProperty> : IValueCreator
    {
        public RandomValueCreator(TProperty start, TProperty end)
        {
            _start = start;
            _end = end;
        }

        private TProperty _start, _end;

        public object CreateValue(object instance)
        {
            return default(TProperty);
        }
    }

    public class CollectionItemValueCreator<TProperty> : IValueCreator
    {
        public CollectionItemValueCreator(IList<TProperty> collection)
        {
            _collection = collection.ToArray();
        }

        [ThreadStatic]
        private static Random _rand = new Random();
        private readonly TProperty[] _collection;

        public object CreateValue(object instance)
        {
            if (_collection.Length == 0)
                throw new InvalidOperationException("collection is empty");

            var index = _rand.Next(0, _collection.Length - 1);
            return _collection[index];
        }
    }

    public class ExpressionValueCreator<TType, TProperty> : IValueCreator where TType : class
    {
        private readonly Func<TType, TProperty> _expression;

        public ExpressionValueCreator(Func<TType, TProperty> expression)
        {
            _expression = expression;
        }

        public object CreateValue(object instance)
        {
            if (instance == null)
                return default(TProperty);
            var value = _expression(instance as TType);
            return value;
        }
    }

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

        public object CreateValue(object instance)
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
