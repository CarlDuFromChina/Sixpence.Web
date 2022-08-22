using Sixpence.Web.Entity;
using Sixpence.Web.Module.SysParamGroup;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sixpence.Web.Module.SysEntity
{
    public class SysEntityEntityOptionProvider : IEntityOptionProvider
    {
        public IEnumerable<SelectOption> GetOptions()
        {
            var manager = EntityManagerFactory.GetManager();
            return manager.Query<SelectOption>($"select code AS Value, name AS Name from sys_entity");
        }
    }
}
