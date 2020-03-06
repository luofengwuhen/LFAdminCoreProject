using System;
using System.Collections.Generic;

namespace LFAdminCoreProject.Models
{
    public partial class TSmsLog
    {
        public long Id { get; set; }
        public string CellPhone { get; set; }
        public string VerificationCode { get; set; }
        public DateTime? ApplyTime { get; set; }
        public DateTime? LostTime { get; set; }
        public string UseFor { get; set; }
        public string ResultCode { get; set; }
        public string ResultMemo { get; set; }
    }
}
