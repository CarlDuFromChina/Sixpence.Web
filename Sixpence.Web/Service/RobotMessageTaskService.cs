using Sixpence.Web.Auth;
using Sixpence.Web.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.Web.Extensions;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Service
{
    public class RobotMessageTaskService : EntityService<RobotMessageTask>
    {
        #region 构造函数
        public RobotMessageTaskService() : base() { }

        public RobotMessageTaskService(IEntityManager manager) : base(manager) { }
        #endregion

        public new IEnumerable<RobotMessageTask> GetAllData()
        {
            return Repository.GetAllEntity();
        }

        public void RunOnce(string id)
        {
            var data = GetData(id);
            var paramList = new Dictionary<string, object>()
            {
                { "Entity", data },
                { "User", UserIdentityUtil.GetAdmin() }
            };

            JobHelpers.RunOnceNow(data.name, data.robotid_name, paramList);
            var jobState = JobHelpers.GetJobStatus(data.name, data.robotid_name).ToSelectOption();
            data.job_state = jobState.Value.ToString();
            data.job_state_name = jobState.Name;
            UpdateData(data);
        }

        public void PauseJob(string id)
        {
            var data = GetData(id);
            JobHelpers.PauseJob(data.name, data.robotid_name);
            data.job_state = "1";
            data.job_state_name = "暂停";
            UpdateData(data);
        }

        public void ResumeJob(string id)
        {
            var data = GetData(id);
            JobHelpers.ResumeJob(data.name, data.robotid_name);
            data.job_state = "0";
            data.job_state_name = "正常";
            UpdateData(data);
        }
    }
}
