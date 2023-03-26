using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Text;
using Sixpence.Web.Model;

namespace Sixpence.Web.EntityOptionProvider
{
    public class SysEntityEntityOptionProvider : IEntityOptionProvider
    {
        public IEnumerable<SelectOption> GetOptions()
        {
            using var manager = EntityManagerFactory.GetManager();
            return manager.Query<SelectOption>($"select code AS Value, name AS Name from sys_entity");
        }
    }
}
