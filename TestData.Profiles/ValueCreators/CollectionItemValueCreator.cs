using System;
using System.Collections.Generic;
using System.Linq;

namespace TestData.Profiles.ValueCreators
{
    public class CollectionItemValueCreator<TProperty> : IValueCreator
    {
        public CollectionItemValueCreator(IEnumerable<TProperty> collection)
        {
            _collection = collection.ToArray();
        }

        [ThreadStatic]
        private static Random _rand = new Random();
        private readonly TProperty[] _collection;

        public object CreateValue(object instance, DataConfiguration dataConfiguration)
        {
            if (_collection.Length == 0)
                throw new InvalidOperationException("collection is empty");

            var index = _rand.Next(0, _collection.Length - 1);
            return _collection[index];
        }
    }
}