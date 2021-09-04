using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FreDX.Models
{
    public class helpers
    {

        public class count
        {

            public string Id { get; set; }
            public string Country { get; set; }
            public string Code { get; set; }
            public string Department { get; set; }
    

        }

        public class Status
        {
            public int Id { get; set; }
            public string Statuses { get; set; }
        }

        public class language
        {
            public int Id { get; set; }
            public string lang { get; set; }
        }

        public class News
        {
            public int Id { get; set; }
            public string labelname { get; set; }
            public string textnews { get; set; }
        }
    }
}