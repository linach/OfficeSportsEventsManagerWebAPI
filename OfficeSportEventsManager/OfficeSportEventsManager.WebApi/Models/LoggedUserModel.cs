using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSportEventsManager.WebApi.Models
{
    public class LoggedUserModel
    {
        public string AuthToken { get; set; }

        public int Id { get; set; }

        public string Username { get; set; }

        public string PictureLink { get; set; }
    }
}