﻿using Quartz;
using Sixpence.Web.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sixpence.ORM.Entity;
using Sixpence.ORM.EntityManager;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Job
{
    public class RobotMessageTaskJob : DynamicJobBase
    {
        public RobotMessageTaskJob() { }
        public RobotMessageTaskJob(string name, string group, string cron) : base(name, group, cron) { }

        public override void Executing(IJobExecutionContext context)
        {
            var entity = context.JobDetail.JobDataMap.Get("Entity") as RobotMessageTask;
            using var Manager = EntityManagerFactory.GetManager();
            var robot = Manager.QueryFirst<Entity.Robot>(entity.robotid);
            try
            {
                var client = RobotClientFacotry.GetClient(robot.robot_type, robot.hook);
                client.SendTextMessage(entity.content);
                Logger.Debug($"机器人[{robot.name}]发送了一条消息[{entity.content}]");
            }
            catch (Exception e)
            {
                Logger.Error($"机器人[{robot.name}]的消息[{entity.name}]发送失败", e);
                entity.job_state = "3";
                entity.job_state_name = "错误";
                Manager.Update(entity);
            }
        }
    }
}
