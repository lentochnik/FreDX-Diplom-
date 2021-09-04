using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreDX.Models
{
    public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Post { get; set; }
            public DateTime CreationDate { get; set; }

            public int? RoleId { get; set; }
            public Role  Role { get; set; }
        }
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Log
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public DateTime Last_login { get; set; }
        public string Procedure { get; set; }
    }

    public class ActionLog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public DateTime ActionDate { get; set; }
        public string Procedure { get; set; }
    }
}

