using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LFAdminCoreProject.Models;
using System.Xml;
using System.Reflection;

namespace LFAdminCoreProject.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //private static readonly log4net.ILog log =log4net.LogManager.GetLogger(typeof(Program));


        public HomeController()
        {
          
        }

        public IActionResult Index()
        {
            Utility.Log4netHelper.Info("这是一条测试 Infomation");

            Utility.Log4netHelper.Error("这是一条测试 Error");

            //throw new Exception("这是一条测试 Error");

            using (LFAdminCoreContext context = new LFAdminCoreContext() )
            {
                var list = context.TUser.ToList();
                int counts = list.Count(); 
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Console()
        {
            return View();
        }
        public IActionResult Homepage1()
        {
            return View();
        }

        public IActionResult Homepage2()
        {
            return View();
        }
        public IActionResult Search()
        {
            return Content("");
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
