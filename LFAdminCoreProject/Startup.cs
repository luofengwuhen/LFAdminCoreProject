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
      
        public static ILoggerRepository repository { get; set; }  //log4net��־

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //��д���� ����
            LFAdminCoreContextConnectionStr = Configuration.GetConnectionString("LFAdminCoreContextConnectionStr");
            LFAdminCoreContextConnectionStrRead = Configuration.GetConnectionString("LFAdminCoreContextConnectionStrRead");
            //ʹ�� һ���� ��ʹ������鷳
            //services.AddDbContext<LFAdminCoreContext>(options => options.UseSqlServer(LFAdminCoreContextConnectionStr));

            //����log4net����
            Utility.Log4netHelper.Repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(Utility.Log4netHelper.Repository, new FileInfo(Environment.CurrentDirectory + "/log4net.config"));

            //ȫ��ע��filter
            services.AddMvc(config =>
            {
                config.Filters.Add(new ActionFilter());
                config.Filters.Add(new ExceptionFilter());
                config.Filters.Add(new AuthorizationFilter());
            }).AddRazorRuntimeCompilation();//�޸�cshtml֮�����ֱ��ˢ����Ч  

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger) 
        {

            logger.AddLog4Net();//log4net ע��

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
