using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSportEventsManager.WebApi.Models
{
    public class ScoreModelShort
    {
        public string Username { get; set; }

        public int Value { get; set; }

        public DateTime? Date { get; set; }

        public string Type { get; set; }
    }
}