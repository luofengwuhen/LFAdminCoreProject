using LFAdminCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Services.LoginServices
{
    public class Login : ILogin
    {
        public ReturnViewModel LoginOn(string username, string password)
        {
            ReturnViewModel model = new ReturnViewModel();
            using (LFAdminCoreContext context = new LFAdminCoreContext())
            {
                var user = context.TUser.Where(o => o.UserName == username && o.Password == Utility.PasswordString.GenerateMD5(password)).FirstOrDefault();

                if (user!=null)
                {
                    model.Code = 0;
                    model.Msg = "登录成功";
                }
                else
                {
                    model.Code = 1;
                    model.Msg = "用户名或密码不正确";
                } 
                
            }

            return model;
        }
    }
}
