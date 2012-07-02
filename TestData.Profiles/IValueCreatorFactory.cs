using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Profiles
{
    public interface IValueCreatorFactory<TType, TProperty>
    {
        IValueCreator ValueFromExpression(Func<TType, TProperty> expression);
        IValueCreator ValueFromConstant(TProperty constant);
        IValueCreator ValueFromRandom(int start, int end);
        IValueCreator ValueFromRandom(decimal start, decimal end);
        IValueCreator ValueFromCollection(IList<TProperty> collection);
        IValueCreator ValueFromRandomString(int length);
        IValueCreator ValueFromRandomString(int start, int end);
    }

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
