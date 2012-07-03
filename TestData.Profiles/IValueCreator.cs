using System.Collections;
using System.Threading.Tasks;

namespace TestData.Profiles
{
    public interface IValueCreator
    {
        object CreateValue(object instance);
    }
}
