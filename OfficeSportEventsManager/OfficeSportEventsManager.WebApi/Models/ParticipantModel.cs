using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSportEventsManager.WebApi.Models
{
    public class ParticipantModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PictureLink { get; set; }

        public bool IsAdmin { get; set; }

        public string AuthToken { get; set; }
    }
}