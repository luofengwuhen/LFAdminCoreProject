using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace LFAdminCoreProject.Services.EMail
{
    public interface IEMailService
    {
        Task<bool> SendEmail(string email, string subject, string message);
    }
}
