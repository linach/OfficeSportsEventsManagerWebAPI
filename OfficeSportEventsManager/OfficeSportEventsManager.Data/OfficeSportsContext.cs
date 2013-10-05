using OfficeSportEventsManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeSportEventsManager.Data
{
    public class OfficeSportsContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<SportEvent> SportEvents { get; set; }

        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasMany(x => x.SportEvents)
                .WithMany(x => x.ParticipatingPlayers);
        }
    }
}
