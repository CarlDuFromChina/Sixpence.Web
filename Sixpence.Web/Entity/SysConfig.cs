﻿using Sixpence.ORM.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sixpence.Web.Entity
{
    [Entity("sys_config", "系统配置")]
    public partial class SysConfig : BaseEntity
    {
        /// <summary>
        /// 实体id
        /// </summary>
        [DataMember]
        [PrimaryColumn]
        public string id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember, Column, Description("名称")]
        public string name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [DataMember, Column, Description("编码")]
        public string code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember, Column, Description("描述")]
        public string description { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember, Column, Description("值")]
        public string value { get; set; }
    }
}