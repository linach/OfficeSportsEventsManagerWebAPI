using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSportEventsManager.WebApi.Models
{
    public class ScoreModel
    {
        public ParticipantShortModel Player { get; set; }

        public int Value { get; set; }
    }
}