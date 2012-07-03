using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Profiles
{
    public interface IMemberData
    {
        PropertyInfo PropertyInfo { get; }
    }
}
