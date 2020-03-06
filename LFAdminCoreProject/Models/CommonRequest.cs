using Aliyun.Acs.Core.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Models
{
    public class CommonRequest
    {
        public MethodType Method { get; set; }
        public string Domain { get; set; }
        public string Version { get; set; }
        public string Action { get; set; }
        public string Protocol { get; set; }
        //public string Method { get; set; }
        //public string Method { get; set; }

    }
}
