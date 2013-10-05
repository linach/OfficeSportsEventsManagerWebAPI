using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace OfficeSportEventsManager.WebApi.Models
{
    public class CompanyModelShort
    {
        
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double? Longitude { get; set; }
        [DataMember]
        public double? Latitude { get; set; }
        [DataMember]
        public IEnumerable<ParticipantModel> Participants { get; set; }

        public CompanyModelShort()
        {
            this.Participants = new List<ParticipantModel>();
        }
    }
}