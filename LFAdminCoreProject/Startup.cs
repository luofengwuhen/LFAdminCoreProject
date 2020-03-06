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

namespace LFAdminCoreProject
{
    public class Startup
    {
        public static string LFAdminCoreContextConnectionStr="";
        public static string LFAdminCoreContextConnectionStrRead = "";
      
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

            //全局注册filter
            services.AddMvc(config =>
            {
                config.Filters.Add(new ActionFilter());
                config.Filters.Add(new ExceptionFilter());
                config.Filters.Add(new AuthorizationFilter());
            }).AddRazorRuntimeCompilation();//修改cshtml之后可以直接刷新生效  

            services.AddControllersWithViews();
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
