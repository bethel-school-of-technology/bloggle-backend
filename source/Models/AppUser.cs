using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace collaby_backend.Models
{
    public class AppUser
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int IsAdmin { get; set; }
        public int IsBand { get; set;}
        public DateTime DateCreated { get; set; }

    }
}