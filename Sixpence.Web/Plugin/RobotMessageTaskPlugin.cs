using Sixpence.ORM.Entity;
using Sixpence.Web.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Plugin
{
    public class RobotMessageTaskPlugin : IEntityManagerPlugin
    {
        public void Execute(EntityManagerPluginContext context)
        {
            if (context.Entity.GetEntityName() != "robot_message_task")
            {
                return;
            }

            var obj = context.Entity as RobotMessageTask;
            switch (context.Action)
            {
                case EntityAction.PostCreate:
                case EntityAction.PostUpdate:
                    JobHelpers.RegisterJob(new RobotMessageTaskJob(obj.name, obj.robotid_name, obj.runtime), obj, obj.job_state.ToTriggerState());
                    break;
                case EntityAction.PreCreate:
                    var jobState = new RobotMessageTaskJob().DefaultTriggerState.ToSelectOption();
                    obj.job_state = jobState.Value.ToString();
                    obj.job_state_name = jobState.Name;
                    break;
                case EntityAction.PreUpdate:
                case EntityAction.PostDelete:
                    JobHelpers.DeleteJob(obj.name, obj.robotid_name);
                    break;
                default:
                    break;
            }
        }
    }
}
