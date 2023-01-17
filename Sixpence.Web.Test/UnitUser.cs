using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixpence.Common;
using Sixpence.ORM.EntityManager;
using Sixpence.ORM.Extensions;
using Sixpence.Web.Auth;
using Sixpence.Web.Config;
using Sixpence.Web.Entity;

namespace Sixpence.Web.Test
{
    [TestClass]
    public class UnitUser
    {
        private IEntityManager manager { get; set; }

        [TestInitialize]
        public void Init()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddServiceContainer();
            SixpenceORMBuilderExtension.UseORM(null, options =>
            {
                options.AutoGenerate = true;
                options.EntityClassNameCase = NameCase.Pascal;
            });
            manager = EntityManagerFactory.GetManager();
        }

        [TestMethod]
        public void TestCreateUser()
        {
            UserIdentityUtil.SetCurrentUser(UserIdentityUtil.GetSystem());
            var curUser = UserIdentityUtil.GetCurrentUser();
            var user = new UserInfo()
            {
                id = "8F982672-75F6-4118-9FB0-B6FB02689B55",
                name = "测试A",
                code = "testa",
                password = SystemConfig.Config.DefaultPassword,
                statecode = true,
                statecode_name = "启用",
                roleid = curUser.Id,
                roleid_name = curUser.Name
            };
            manager.Create(user);
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            manager.Delete("user_info", "8F982672-75F6-4118-9FB0-B6FB02689B55");
        }
    }
}
