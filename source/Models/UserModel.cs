using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace collaby_backend.Models
{
    public class UserModel
    {
        public long Id {get; set;}
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfJoing { get; set; }
    }
}