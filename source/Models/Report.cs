using System;
using System.Collections.Generic;

namespace collaby_backend.Models
{
    public partial class Report
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public byte[] DateCreated { get; set; }
        public string Message { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
