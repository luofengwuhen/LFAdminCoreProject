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

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);//.FromSeconds(5); //30����  ȷ�����������������е�����ǰ�����ݿ��Կ��ж೤ʱ�� �����Զ����� Cookie ����ʱ�䡣 ͨ���Ự�м�����ݵ�ÿ�����󶼻����ó�ʱ
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            //ȫ��ע��filter
            services.AddMvc(config =>
            {
                config.Filters.Add(new MyActionFilter());
                config.Filters.Add(new MyExceptionFilter());
                config.Filters.Add(new MyAuthorizationFilter() );
                config.Filters.Add(typeof(CheckLoginFilter)); //{ IsCheck=true}
            }); 

            services.AddMvc().AddRazorRuntimeCompilation();//�޸�cshtml֮�����ֱ��ˢ����Ч  

            #region redis ��sqlserver cache��������ע����䲻ͬ������ʹ�ö�һ��
            //ע��redis��ȫ�� ��Ҫ��NuGet�����StackExchangeRedis��
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("Cache:ConnectionString").Value;
                options.InstanceName = Configuration.GetSection("Cache:InstanceName").Value;
            });

            //ע��SqlServerCache ��ȫ��  ��Ҫ��NuGet�����Microsoft.Extensions.Caching.SqlServer��
            // table�����Լ���
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = Configuration.GetSection("SQLCache:ConnectionString").Value;
            //    options.SchemaName = "dbo";
            //    options.TableName = Configuration.GetSection("SQLCache:Table").Value;
            //});
            #endregion
            #region ע�� ���񣬵���ģʽ

            services.AddSingleton<IForgetUser, ForgetUser>();
            services.AddSingleton<ILogin, Login>();
            services.AddSingleton<IRegisterUser, RegisterUser>();
            services.AddSingleton<ISendSmsService, SendSmsService>();
            //services.AddSingleton<IRedisCacheService, RedisCacheService>();


            ServiceContext.RegisterServices(services); //ע�ᵽ����������
            #endregion
            services.AddControllersWithViews();

            //services.AddLogging(); //
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
            app.UseSession();//ʹ��session
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
