using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.Web.Service
{
    public class RobotService : EntityService<Entity.Robot>
    {
        #region 构造函数
        public RobotService() : base() { }

        public RobotService(IEntityManager manager) : base(manager) { }
        #endregion
    }
}
