using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppWebApi.Interfaces
{
    public interface IMailService
    {
        public bool SendEmail(string userEmail, string confirmationLink);
    }
}
