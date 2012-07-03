using System;
using System.Collections.Generic;
using TestData.Profiles.ValueCreators;

namespace TestData.Profiles
{
    public class ValueCreatorFactory<TType, TProperty> : IValueCreatorFactory<TType, TProperty> where TType : class
    {
        public IValueCreator ValueFromExpression(Func<TType, TProperty> expression)
        {
            return new ExpressionValueCreator<TType, TProperty>(expression);
        }

        public IValueCreator ValueFromConstant(TProperty constant)
        {
            throw new NotImplementedException();
        }

        public IValueCreator ValueFromRandom(int start, int end)
        {
            return new IntRandomValueCreator(start, end);
        }

        public IValueCreator ValueFromRandom(decimal start, decimal end)
        {
            return new DecimalRandomValueCreator(start, end);
        }

        public IValueCreator ValueFromCollection(IList<TProperty> collection)
        {
            return new CollectionItemValueCreator<TProperty>(collection);
        }

        public IValueCreator ValueFromRandomString(int length)
        {
            return new RandomStringValueCreator(length);
        }

        public IValueCreator ValueFromRandomString(int start, int end)
        {
            return new RandomStringValueCreator(start, end);
        }
    }
}