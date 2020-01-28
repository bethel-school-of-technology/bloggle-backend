using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
    

namespace collaby_backend.Models
{    
    public class ApplicationUser: DbContext
    {
        public ApplicationUser(DbContextOptions<ApplicationUser> options)
        : base(options)
        {
        }

        public DbSet<UserModel> User { get; set; }
    }

}