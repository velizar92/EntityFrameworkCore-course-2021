using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;
using System;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {

        public FootballBettingContext()
        {

        }

        public FootballBettingContext(DbContextOptions options)
          : base(options)
        {

        }


        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<User> Users { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(Configuration.CONNECTION_STRING);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>(e =>
                {
                    e.HasKey(ps => new { ps.GameId, ps.PlayerId });
                });


            modelBuilder.Entity<Team>(e =>
                    e.HasOne(t => t.PrimaryKitColor).WithMany(c => c.PrimaryKitTeams)
                    .HasForeignKey(t => t.PrimaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Team>(e =>
                  e.HasOne(t => t.SecondaryKitColor).WithMany(c => c.SecondaryKitTeams)
                  .HasForeignKey(t => t.SecondaryKitColorId)
                  .OnDelete(DeleteBehavior.Restrict));


            modelBuilder.Entity<Game>(e =>
                  e.HasOne(g => g.HomeTeam).WithMany(t => t.HomeGames)
                  .HasForeignKey(g => g.HomeTeamId)
                  .OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Game>(e =>
                  e.HasOne(g => g.AwayTeam).WithMany(t => t.AwayGames)
                  .HasForeignKey(g => g.AwayTeamId)
                  .OnDelete(DeleteBehavior.Restrict));
        }




        

    }
}
