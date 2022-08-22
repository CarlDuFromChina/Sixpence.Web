using Sixpence.Web.Entity;
using Sixpence.Common.IoC;
using System.Collections.Generic;

namespace Sixpence.Web.Module.SysParamGroup
{
    [ServiceRegister]
    public interface IEntityOptionProvider
    {
        IEnumerable<SelectOption> GetOptions();
    }
}
