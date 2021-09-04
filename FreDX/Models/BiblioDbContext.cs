using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FreDX.Models
{
    public class BiblioDbContext : DbContext
    {
        public BiblioDbContext()
             : base("biblio")
        { }


        public DbSet<Agent> agents { get; set; }
        public DbSet<Appl> aplicants { get; set; }
        public DbSet<Enterbiblio> enterbiblio { get; set; }
        public DbSet<Inventionbiblio> inventionbiblio { get; set; }
        public DbSet<Invent> inventors { get; set; }
        public DbSet<Priority> priority { get; set; }
    }
}