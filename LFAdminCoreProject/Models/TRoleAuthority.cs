using System;
using System.Collections.Generic;

namespace LFAdminCoreProject.Models
{
    public partial class TRoleAuthority
    {
        public long Id { get; set; }
        public long? RoleId { get; set; }
        public long? AuthorityId { get; set; }
        public DateTime? OperateTime { get; set; }
    }
}
