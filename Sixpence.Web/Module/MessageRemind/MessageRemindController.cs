using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.Web.Module.MessageRemind
{
    public class MessageRemindController : EntityBaseController<message_remind, MessageRemindService>
    {
        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("unread_message_count")]
        public object GetUnReadMessageCount()
        {
            return new MessageRemindService().GetUnReadMessageCount();
        }
    }
}
