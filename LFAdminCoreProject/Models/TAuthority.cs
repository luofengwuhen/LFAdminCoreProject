using System;
using System.Collections.Generic;

namespace LFAdminCoreProject.Models
{
    public partial class TAuthority
    {
        public long Id { get; set; }
        public string AuthorityCode { get; set; }
        public string AuthorityName { get; set; }
        public string ParentAuthorityCode { get; set; }
    }
}
