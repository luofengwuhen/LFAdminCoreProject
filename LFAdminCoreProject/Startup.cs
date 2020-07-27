using LFAdminCoreProject.Filters;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Razor;
using LFAdminCoreProject.Services.Cache; 
using LFAdminCoreProject.Services.Login;
using LFAdminCoreProject.Services.AliSms;
using Mango.Framework.Services;
using Microsoft.Extensions.Caching.Redis;

namespace LFAdminCoreProject
{
    public class Startup
    {
        public static string LFAdminCoreContextConnectionStr="";
        public static string LFAdminCoreContextConnectionStrRead = "";
        public static string InstanceName = "";
        public static ILoggerRepository repository { get; set; }  //log4net日志

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //读写分离 配置
            LFAdminCoreContextConnectionStr = Configuration.GetConnectionString("LFAdminCoreContextConnectionStr");
            LFAdminCoreContextConnectionStrRead = Configuration.GetConnectionString("LFAdminCoreContextConnectionStrRead");
            //使用 一个库 ，使用相对麻烦
            //services.AddDbContext<LFAdminCoreContext>(options => options.UseSqlServer(LFAdminCoreContextConnectionStr));

            //加载log4net配置
            Utility.Log4netHelper.Repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(Utility.Log4netHelper.Repository, new FileInfo(Environment.CurrentDirectory + "/log4net.config"));

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);//.FromSeconds(5); //30分钟  确定放弃服务器缓存中的内容前，内容可以空闲多长时间 此属性独立于 Cookie 到期时间。 通过会话中间件传递的每个请求都会重置超时
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            //全局注册filter
            services.AddMvc(config =>
            {
                config.Filters.Add(new MyActionFilter());
                config.Filters.Add(new MyExceptionFilter());
                config.Filters.Add(new MyAuthorizationFilter() );
                config.Filters.Add(typeof(CheckLoginFilter)); //{ IsCheck=true}
            }); 

            services.AddMvc().AddRazorRuntimeCompilation();//修改cshtml之后可以直接刷新生效  

            #region redis 和sqlserver cache两者在于注册语句不同，其他使用都一样
            //注册redis到全局 需要在NuGet里添加StackExchangeRedis包
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("Cache:ConnectionString").Value;
                options.InstanceName = Configuration.GetSection("Cache:InstanceName").Value;
            });

            //注册SqlServerCache 到全局  需要在NuGet里添加Microsoft.Extensions.Caching.SqlServer包
            // table可以自己建
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = Configuration.GetSection("SQLCache:ConnectionString").Value;
            //    options.SchemaName = "dbo";
            //    options.TableName = Configuration.GetSection("SQLCache:Table").Value;
            //});
            #endregion
            #region 注册 服务，单例模式

            services.AddSingleton<IForgetUser, ForgetUser>();
            services.AddSingleton<ILogin, Login>();
            services.AddSingleton<IRegisterUser, RegisterUser>();
            services.AddSingleton<ISendSmsService, SendSmsService>();
            //services.AddSingleton<IRedisCacheService, RedisCacheService>();


            ServiceContext.RegisterServices(services); //注册到类里，方便调用
            #endregion
            services.AddControllersWithViews();

            //services.AddLogging(); //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger) 
        {

            logger.AddLog4Net();//log4net 注入

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            
            app.UseStaticFiles();
            app.UseSession();//使用session
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}");
            });
        }
    }
}
