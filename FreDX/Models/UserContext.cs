using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;



namespace FreDX.Models
{
    public class UserContext : DbContext
    {
        public UserContext()
            : base("conn")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}