using System.Collections;
using System.Threading.Tasks;
using TestData.Profiles.ValueCreators;

namespace TestData.Profiles
{
    public interface IValueCreator
    {
        object CreateValue(object instance, IProfileResolver profileResolver);
    }
}
