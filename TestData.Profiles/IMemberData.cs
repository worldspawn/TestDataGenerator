using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestData.Profiles.ValueCreators;

namespace TestData.Profiles
{
    public interface IMemberData
    {
        PropertyInfo PropertyInfo { get; }
        object GetValue(object instance, IProfileResolver profileResolver);
        IValueCreator ValueCreator { get; }
    }
}
