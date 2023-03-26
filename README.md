# Sixpence.Web

> 一个强大的 .Net Web 框架

## 介绍

一个基于`.net 6.0`的`WebApi`框架，通过此框架能迅速搭建一个接口平台

+ 基础功能
  + 登录/注册
    + JWT
    + 集成了Github、Gitee等第三方登录策略
    + 支持邮箱注册
  + 基于RBAC的角色管理
    + 权限管理
    + 角色管理
    + 用户管理
  + 图库
    + 本地图片
    + Pixabay 云图库
  + 作业管理（quartz）
  + 数据库表维护
    + 实体管理
    + 字段管理
  + 选项集
  + 版本管理
    + 支持CSV文件、SQL脚本初始化数据
    + 启动自动执行迁移脚本
+ 集成 Swagger（手动开启）

## 安装

```powershell
dotnet add package Sixpence.Web --version 2.1.0
```

## 使用

1、初始化脚本

将项目下的`version`文件夹到目录下（注意：编译选择内容）

2、Startup

参考：

```csharp
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
            options.Filters.Add(typeof(Web.WebApi.WebApiExceptionFilter));
            options.Filters.Add(typeof(Web.WebApi.WebApiContextFilter));
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
            options.EntityClassNameCase = NameCase.UnderScore;
            options.AutoGenerate = true;
        });

        app.UseJob();
        app.UseSysRole();
        app.UseSysConfig();

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
```

## 关于

本项目是作者利用空余时间开发，维护全靠热爱发电。如果使用过程中遇到问题，欢迎报告 issue。
