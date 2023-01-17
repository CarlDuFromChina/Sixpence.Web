using Sixpence.Common.IoC;
using System.Collections.Generic;
using Sixpence.Web.Model;

namespace Sixpence.Web.EntityOptionProvider
{
    [ServiceRegister]
    public interface IEntityOptionProvider
    {
        IEnumerable<SelectOption> GetOptions();
    }
}
