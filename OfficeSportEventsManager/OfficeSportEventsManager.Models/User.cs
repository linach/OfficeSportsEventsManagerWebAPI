using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeSportEventsManager.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string AuthCode { get; set; }

        public string AuthToken { get; set; }

        public string PictureLink { get; set; }

        public Company Company { get; set; }

        public bool IsAdmin { get; set; }

        public ICollection<SportEvent> SportEvents { get; set; }

        public User()
        {
            this.SportEvents = new HashSet<SportEvent>();
        }
    }
}
