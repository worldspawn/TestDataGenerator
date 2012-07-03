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
}
