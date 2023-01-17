using Sixpence.ORM.EntityManager;
using System;
using System.Collections.Generic;
using System.Text;
using Sixpence.Web.Service;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Auth.Github
{
    public class GithubUserBind : IThirdPartyBindStrategy
    {
        public string GetName() => "Github";

        public void Bind(string code, string userid)
        {
            var manager = EntityManagerFactory.GetManager();
            var githubService = new GithubAuthService(manager);
            manager.ExecuteTransaction(() =>
            {
                var user = manager.QueryFirst<UserInfo>(userid);
                var githubToken = githubService.GetAccessToken(code);
                var githubUser = githubService.GetUserInfo(githubToken);
                user.github_id = githubUser.id.ToString();
                manager.Update(user);
            });
        }
    }
}
