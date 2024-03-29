﻿using AutoMapper;
using Sixpence.Web.Auth;
using Sixpence.Common.Logging;
using Sixpence.Web.Profiles;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Sixpence.Common.Current;

namespace Sixpence.Web.WebApi
{
    public class WebApiContextFilter : ActionFilterAttribute
    {
        public void Log(ActionExecutingContext context)
        {
            var log = LoggerFactory.GetLogger("webapi");
            var url = context.HttpContext.Request.Path;
            var method = context.HttpContext.Request.Method;
            var info = $"{method}：{url}";
            log.Debug(info);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            Log(context);

            var tokenHeader = context.HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
            var user = JwtHelper.SerializeJwt(tokenHeader);
            if (user != null)
            {
                UserIdentityUtil.SetCurrentUser(MapperHelper.Map<CurrentUserModel>(user));
            }
            else
            {
                UserIdentityUtil.SetCurrentUser(UserIdentityUtil.GetAnonymous());
            }
        }
    }
}
