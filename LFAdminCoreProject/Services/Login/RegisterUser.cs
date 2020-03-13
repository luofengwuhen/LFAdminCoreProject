using LFAdminCoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Services.Login
{
    public class RegisterUser : IRegisterUser
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="cellPhone"></param>
        /// <param name="password"></param>
        /// <param name="Chinese_Name"></param>
        /// <returns></returns>
        ReturnViewModel IRegisterUser.RegisterUser(string cellPhone, string password, string Chinese_Name)
        {

            ReturnViewModel model = new ReturnViewModel();
            using (LFAdminCoreContext context = new LFAdminCoreContext())
            {
                TUser user = new TUser();
                user.ChineseName = string.IsNullOrEmpty(Chinese_Name)? cellPhone: Chinese_Name;
                user.Password = Utility.PasswordString.GenerateMD5(password); //加密密码
                user.Phone = cellPhone;
                user.UserName = cellPhone;
                user.CreateTime = DateTime.Now; 
                 
                context.TUser.Add(user);
                context.SaveChanges();

                model.Code = 0;
                model.Msg = "注册成功";
            }

            return model;
        }
         

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="veryCode"></param>
        /// <param name="cellPhone"></param>
        /// <returns></returns>
        public bool IsVeryCode(string veryCode,string cellPhone,string typeString )
        {
            using (LFAdminCoreContext context = new LFAdminCoreContext())
            {
                var model = context.TSmsLog.AsNoTracking().Where(o => o.CellPhone == cellPhone && o.LostTime > DateTime.Now && o.VerificationCode==veryCode && o.UseFor==typeString) .FirstOrDefault();
                if (model!=null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
