using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace OfficeSportEventsManager.Models
{
    public class Score
    {
        public int Id { get; set; }

        public User Player { get; set; }

        public int Value { get; set; }

        public SportEvent SportEvent { get; set; }
    }
}
