using System;
using System.Collections.Generic;

namespace collaby_backend.Models
{
    public partial class Comment
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? IsDraft { get; set; }
        public long? ReportCount { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}