using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace collaby_backend.Models
{
    public partial class Rating
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public double Value { get; set; }
        
        [JsonIgnore]
        public virtual Post Post { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
