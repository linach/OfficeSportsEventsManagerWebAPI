using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeSportEventsManager.Models
{
    public class SportEvent
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime? Date { get; set; }

        public Company Company { get; set; }

        public ICollection<User> ParticipatingPlayers { get; set; }

        public string PictureLink { get; set; }

        public ICollection<Score> ScoresList { get; set; }

        public SportEvent()
        {
            this.ScoresList = new HashSet<Score>();
            this.ParticipatingPlayers = new HashSet<User>();
        }
    }
}
