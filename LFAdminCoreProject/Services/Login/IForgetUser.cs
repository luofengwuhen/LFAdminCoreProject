using LFAdminCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Services.Login
{
    public interface IForgetUser
    {
        ReturnViewModel ForgetUser(string cellPhone, string password);
    }
}
