using LFAdminCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Services.LoginServices
{
    interface ISendSmsService
    {  
        ReturnViewModel SendSms(string cellPhone);

        //bool IsJgEmployee(string cellPhone);

        bool IsConstantlyRegister(string cellPhone);
        bool IsHasRegister(string cellPhone);
    }
}
