using LFAdminCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Services.Login
{
    public interface IRegisterUser
    {
        ReturnViewModel RegisterUser(string cellPhone,string password,string Chinese_Name);
        bool IsVeryCode(string veryCode, string cellPhone, string typeString);
    }
}
