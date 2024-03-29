﻿using Sixpence.ORM.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Sixpence.Web.Entity
{
    [Entity("job_history", "作业执行记录")]
    public class JobHistory : BaseEntity
    {
        [DataMember]
        [PrimaryColumn]
        public string id { get; set; }

        /// <summary>
        /// 作业名
        /// </summary>
        [DataMember, Column, Description("作业名")]
        public string job_name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember, Column, Description("开始时间")]
        public DateTime? start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember, Column, Description("结束时间")]
        public DateTime? end_time { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, Column, Description("状态")]
        public string status { get; set; }

        /// <summary>
        /// 错误原因
        /// </summary>
        [DataMember, Column, Description("错误原因")]
        public string error_msg { get; set; }
    }
}
