using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppWebApi.Models
{
    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string CurrentPassword { get; set; }

        /*public Action data { get; set; }

        public Action error { get; set; }*/
    }
}
