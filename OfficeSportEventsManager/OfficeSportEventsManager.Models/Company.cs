using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeSportEventsManager.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public ICollection<User> Participants { get; set; }

        public ICollection<SportEvent> SportEvents { get; set; }

        public Company()
        {
            this.Participants = new HashSet<User>();
            this.SportEvents = new HashSet<SportEvent>();
        }

    }
}
