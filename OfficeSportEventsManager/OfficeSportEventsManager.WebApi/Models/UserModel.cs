using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSportEventsManager.WebApi.Models
{
    public class UserModel
    {
        public string Username { get; set; }

        public string AuthCode { get; set; }
    }
}