using Disaster_Relief.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebsiteExampleMain.Models
{
    public class User_Context :DbContext
    {
        public static List<Users> userObject = new List<Users>();

        public User_Context()
        {
        }

        public User_Context(DbContextOptions<User_Context> options)
            : base (options) 
            { }

        public DbSet<Users> Users { get; set; }
    }
}
