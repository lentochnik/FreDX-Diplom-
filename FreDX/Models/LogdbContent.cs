using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FreDX.Models
{
    public class LogdbContent : DbContext
    {

            public LogdbContent()
                : base("logss")
            { }
            public DbSet<Log> logss { get; set; }
            public DbSet<ActionLog> alog { get; set; }

        
    }
}
