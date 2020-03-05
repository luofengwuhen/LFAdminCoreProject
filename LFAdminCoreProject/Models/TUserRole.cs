using System;
using System.Collections.Generic;

namespace LFAdminCoreProject.Models
{
    public partial class TUserRole
    {
        public long Id { get; set; }
        public long? RoleId { get; set; }
        public long? UserId { get; set; }
        public DateTime? OperateTime { get; set; }
    }
}
