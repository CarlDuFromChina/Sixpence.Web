using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Sixpence.Common;
using Sixpence.ORM.Extensions;
using Sixpence.Web.Extensions;
using Sixpence.Web.Profiles;

namespace Sixpence.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
               .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services
                .AddControllers(options =>
                {
                    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
                });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(WebApi.WebApiExceptionFilter));
                options.Filters.Add(typeof(WebApi.WebApiContextFilter));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // 添加依赖注入服务
            services.AddServiceContainer();

            // 添加Jwt认证服务
            services.AddJwt();

#if DEBUG
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "接口文档", Version = "v1" });
            });
#endif


            // 添加AutoMapper
            services.AddAutoMapper(MapperHelper.MapType());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            app.UseStaticHttpContext();

            app.UseORM(options =>
            {
                options.EntityClassNameCase = NameCase.Pascal;
                options.AutoGenerate = true;
            });

            #region Sixpence Web
            app.UseJob();
            app.UseSysRole();
            app.UseSysConfig();
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"v1/swagger.json", "接口文档");
                c.RoutePrefix = "Swagger";
            });
#endif

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute
                (
                    name: "default",
                    pattern: "{controller:slugify}/{action:slugify}/{id?}"
                )
                .RequireCors("CorsPolicy");
            });
        }
    }

    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1_$2").ToLower();
        }
    }
}
