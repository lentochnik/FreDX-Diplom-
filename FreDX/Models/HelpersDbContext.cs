using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FreDX.Models
{
    public class HelpersDbContext : DbContext
    {
        public HelpersDbContext()
    :    base("helpers")
        { }
        public DbSet<helpers.count> Count { get; set; }
        public DbSet<helpers.Status> Status { get; set; }
        public DbSet<helpers.language> lang { get; set; }
        public DbSet<helpers.News> news { get; set; }
    }
}