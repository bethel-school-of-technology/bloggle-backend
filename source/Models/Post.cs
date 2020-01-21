using System;
using System.Collections.Generic;

namespace collaby_backend.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Ratings = new HashSet<Rating>();
            Reports = new HashSet<Report>();
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public string Message { get; set; }
        public byte[] DateCreated { get; set; }
        public byte[] DateModified { get; set; }
        public double? RatingValue { get; set; }
        public long? RatingCount { get; set; }
        public byte[] IsDraft { get; set; }
        public long? TotalComments { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
