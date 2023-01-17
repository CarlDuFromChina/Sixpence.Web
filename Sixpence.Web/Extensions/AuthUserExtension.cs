using Sixpence.Common.Current;
using Sixpence.Web.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sixpence.Web.Extensions
{
    public static class AuthUserExtension
    {
        public static CurrentUserModel ToCurrentUserModel(this AuthUser user)
        {
            return new CurrentUserModel()
            {
                Code = user.code,
                Id = user.user_infoid,
                Name = user.name
            };
        }
    }
}
