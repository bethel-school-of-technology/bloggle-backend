using System;
using System.Collections.Generic;

namespace collaby_backend.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Posts = new HashSet<Post>();
            Ratings = new HashSet<Rating>();
            Reports = new HashSet<Report>();
        }

        public long Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Img { get; set; }
        public double? TotalRating { get; set; }
        public long? RatedPosts { get; set; }
        public long? TotalPosts { get; set; }
        public long? Followers { get; set; }
        public string Followings { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
