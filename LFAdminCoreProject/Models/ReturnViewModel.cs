using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LFAdminCoreProject.Models
{
    public class ReturnViewModel
    {
        /// <summary>
        /// 0为成功，1为失败
        /// </summary>
        public int Code { get; set; }   
        public string Msg { get; set; }
        public DataModel Data { get; set; } 
         
    }
    public class DataModel
    {
        public string access_token  { get; set; }
    }


}
