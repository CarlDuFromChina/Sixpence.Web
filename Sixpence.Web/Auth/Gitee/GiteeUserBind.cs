using Sixpence.Web.Auth.UserInfo;
using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sixpence.Web.Auth.Gitee
{
    public  class GiteeUserBind : IThirdPartyBindStrategy
    {
        public string GetName() => "Gitee";

        public void Bind(string code, string userid)
        {
            var manager = EntityManagerFactory.GetManager();
            var giteeAuthService = new GiteeAuthService(manager);
            manager.ExecuteTransaction(() =>
            {
                var user = manager.QueryFirst<user_info>(userid);
                var githubToken = giteeAuthService.GetAccessToken(code, userid);
                var githubUser = giteeAuthService.GetGiteeUserInfo(githubToken);
                user.gitee_id = githubUser.id.ToString();
                manager.Update(user);
            });
        }
    }
}
