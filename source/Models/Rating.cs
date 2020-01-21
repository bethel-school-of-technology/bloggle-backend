using System;
using System.Collections.Generic;

namespace collaby_backend.Models
{
    public partial class Rating
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public byte[] Ratings { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
