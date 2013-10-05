using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace OfficeSportEventsManager.WebApi.Models
{
    public class EventModel
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime? Date { get; set; }

        public string PictureLink { get; set; }

        public IEnumerable<ScoreModel> ScoresList { get; set; }

        public EventModel()
        {
            this.ScoresList = new List<ScoreModel>();
        }
    }
}