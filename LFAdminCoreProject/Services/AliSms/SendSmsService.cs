using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using LFAdminCoreProject.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Services.AliSms
{
    public class SendSmsService : ISendSmsService
    {
        //随机生成X位验证码
        public string GetCode(int len)
        {
            string code = "";
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                code += random.Next(0, 9);
            } 
            return code;
        }

        /// <summary>
        /// 是否频繁 获取验证码 最多3次
        /// </summary>
        /// <param name="cellPhone"></param>
        /// <returns></returns>
        public bool IsConstantlyRegister(string cellPhone,string typeString)
        {
            //从注册日志里查询半个小时内是否有连续注册的情况
            using (LFAdminCoreContext context = new LFAdminCoreContext())
            {
                var list = context.TSmsLog.AsNoTracking().Where(o => o.CellPhone == cellPhone && o.LostTime> DateTime.Now && o.UseFor==typeString).ToList();
                if(list.Count>2)
                { 
                    return true;
                }
            }
            return false;
        }
        //判断手机号是否已经注册
        public bool IsHasRegister(string cellPhone)
        {
            using (LFAdminCoreContext context = new LFAdminCoreContext())
            {
                var model = context.TUser.AsNoTracking().Where(o => o.Phone == cellPhone).FirstOrDefault();
                if (model!=null)
                {
                    return true;
                }
            }
            return false;
        }

        //是否本集团员工
        public bool IsJgEmployee(string cellPhone)
        {

            return true;
        }

        //发送验证码，并记录到日志
        public ReturnViewModel SendSms(string cellPhone,string typeString,string TemplateCode)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", Utility.AliSendSmsStrings.AccessKeyId, Utility.AliSendSmsStrings.AccessSecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", cellPhone);
            request.AddQueryParameters("SignName", Utility.AliSendSmsStrings.SignName);
            request.AddQueryParameters("TemplateCode", TemplateCode);

            string verCode = GetCode(4);
            request.AddQueryParameters("TemplateParam", "{\"code\":\"" + verCode + "\"}");

            CommonResponse response = client.GetCommonResponse(request);
            //将 验证码，手机号，返回结果 写入数据库，并返回前端用于判断
            AliReturnModel data = JsonConvert.DeserializeObject<AliReturnModel>(response.Data);

            ReturnViewModel model = new ReturnViewModel();
            using (LFAdminCoreContext context = new LFAdminCoreContext())
            {
                TSmsLog log = new TSmsLog();
                log.CellPhone = cellPhone;
                log.UseFor = typeString;
                log.ResultCode = data.Code;
                log.ApplyTime = DateTime.Now;
                log.ResultMemo = data.Message;
                if (data.Code.Equals("OK"))//成功 
                {
                    log.VerificationCode = verCode;
                    log.LostTime = DateTime.Now.AddMinutes(30);
                    model.Code = 0;
                    model.Msg = verCode;
                }
                else
                {
                    model.Code = 1;
                    model.Msg = cellPhone + "申请验证码失败：" + data.Message;
                    Utility.Log4netHelper.Warning(model.Msg);
                }
                context.TSmsLog.Add(log);
                context.SaveChanges();
            }

            return model;
        }

         
    }
}
