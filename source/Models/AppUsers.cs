using System;
using System.Collections.Generic;

namespace collaby_backend.Models
{
    public partial class AppUsers
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long IsAdmin { get; set; }
        public long IsBand { get; set; }
        public byte[] DateCreated { get; set; }
    }
}
