﻿using Sixpence.Web.Auth;
using Sixpence.Web.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Sixpence.Web.Entity;
using Sixpence.Web.Service;

namespace Sixpence.Web.Controller
{
    public class MailVertificationController : EntityBaseController<MailVertification, MailVertificationService>
    {
        /// <summary>
        /// 激活用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("user/activate"), AllowAnonymous]
        public string ActivateUser(string id)
        {
            UserIdentityUtil.SetCurrentUser(UserIdentityUtil.GetSystem());
            return new MailVertificationService().ActivateUser(id);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("passsword/reset"), AllowAnonymous]
        public string ResetPassword(string id)
        {
            UserIdentityUtil.SetCurrentUser(UserIdentityUtil.GetSystem());
            return new MailVertificationService().ResetPassword(id);
        }
    }
}
