using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace OfficeSportEventsManager.WebApi.Models
{
    [DataContract]
    public class CompanyModel
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
        public IEnumerable<ParticipantEventModel> Participants { get; set; }

        public CompanyModel()
        {
            this.Participants = new List<ParticipantEventModel>();
        }
    }
}