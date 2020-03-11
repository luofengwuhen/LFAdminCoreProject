using LFAdminCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Services.AliSms
{
    public interface ISendSmsService
    {  
        ReturnViewModel SendSms(string cellPhone,string typeString,string TemplateCode);

        //bool IsJgEmployee(string cellPhone);

        bool IsConstantlyRegister(string cellPhone, string typeString);
        bool IsHasRegister(string cellPhone);
    }
}
