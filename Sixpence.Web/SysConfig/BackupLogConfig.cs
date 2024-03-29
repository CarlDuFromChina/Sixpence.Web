﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sixpence.Web.SysConfig
{
    /// <summary>
    /// 备份设置
    /// </summary>
    public class BackupLogSysConfig : ISysConfig
    {
        public string Name => "备份天数";

        public object DefaultValue { get => 30; }

        public string Code => "log_backup_days";

        public string Description => "填入数值类型，默认 30";
    }
}