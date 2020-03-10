using LFAdminCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Services.LoginServices
{
    public class ForgetUser : IForgetUser
    {
        ReturnViewModel IForgetUser.ForgetUser(string cellPhone, string password)
        {
            ReturnViewModel model = new ReturnViewModel();
            using (LFAdminCoreContext context = new LFAdminCoreContext())
            {

                var user = context.TUser.Where(o => o.Phone == cellPhone).FirstOrDefault();
                if(user!=null)
                {
                    user.Password = Utility.PasswordString.GenerateMD5(password); //加密密码 
                    user.ModifyTime = DateTime.Now;

                    context.SaveChanges();

                    model.Code = 0;
                    model.Msg = "密码重置成功";
                }
                else
                {
                    model.Code = 1;
                    model.Msg = "该手机号未注册";
                } 
            }

            return model;
        }
    }
}
