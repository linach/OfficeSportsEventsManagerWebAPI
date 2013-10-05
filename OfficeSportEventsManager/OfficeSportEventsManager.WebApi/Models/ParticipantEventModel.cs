using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSportEventsManager.WebApi.Models
{
    public class ParticipantEventModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PictureLink { get; set; }

        public IEnumerable<EventModel> Events { get; set; }

        public ParticipantEventModel()
        {
            this.Events = new List<EventModel>();
        }
    }
}