using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using LFAdminCoreProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Services.LoginServices
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

        public ReturnViewModel SendSms(string cellPhone)
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
            request.AddQueryParameters("TemplateCode", Utility.AliSendSmsStrings.TemplateCode);
       
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
                log.UseFor = "注册";
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
