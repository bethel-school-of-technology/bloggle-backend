using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace collaby_backend.Models
{
    public class AppUser: IdentityUser
    {
        /*public long Id {get; set;}
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int IsAdmin { get; set; }
        public int IsBand { get; set; }
        public DateTime DateOfJoing { get; set; }
        public String token { get; set; }*/
        public DateTime CreationDate = DateTime.Now.ToUniversalTime();

        /*public AppUser()
        {
            Comments = new HashSet<Comment>();
            Posts = new HashSet<Post>();
            Ratings = new HashSet<Rating>();
            Reports = new HashSet<Report>();
        }

        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }//
        public string Email { get; set; }//
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public long IsAdmin { get; set; }//
        public long IsBand { get; set; }//
        public string Img { get; set; }
        public double? TotalRating { get; set; }
        public long? RatedPosts { get; set; }
        public long? TotalPosts { get; set; }
        public string Followings { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Report> Reports { get; set; }*/
    }
}