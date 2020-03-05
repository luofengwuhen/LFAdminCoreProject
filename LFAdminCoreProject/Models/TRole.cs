using System;
using System.Collections.Generic;

namespace LFAdminCoreProject.Models
{
    public partial class TRole
    {
        public long Id { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Memo { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public DateTime? BanTime { get; set; }
        public bool? IsBan { get; set; }
    }
}
