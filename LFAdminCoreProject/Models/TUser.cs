using System;
using System.Collections.Generic;

namespace LFAdminCoreProject.Models
{
    public partial class TUser
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ChineseName { get; set; }
        public string Phone { get; set; }
        public string Emial { get; set; }
        public string Address { get; set; }
        public string Memo { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public DateTime? BanTime { get; set; }
        public bool? IsBan { get; set; }
    }
}
