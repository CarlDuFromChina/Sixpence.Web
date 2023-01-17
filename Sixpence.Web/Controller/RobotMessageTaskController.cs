using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.Web.Entity;
using Sixpence.Web.Service;

namespace Sixpence.Web.Controller
{
    public class RobotMessageTaskController : EntityBaseController<RobotMessageTask, RobotMessageTaskService>
    {
        [HttpGet("{id}/run")]
        public void RunOnce(string id)
        {
            new RobotMessageTaskService().RunOnce(id);
        }

        [HttpGet("{id}/pause")]
        public void PauseJob(string id)
        {
            new RobotMessageTaskService().PauseJob(id);
        }

        [HttpGet("{id}/resume")]
        public void ResumeJob(string id)
        {
            new RobotMessageTaskService().ResumeJob(id);
        }
    }
}
