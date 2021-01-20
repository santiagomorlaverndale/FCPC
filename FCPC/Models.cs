using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FCPC
{
    public class BloggingContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Candidate> Cadidates { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Region> Regions { get; set; }

        private static bool _created = false;
        public BloggingContext()
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Vote>()
        //        .Property(e => e.VoteId)
        //        .ValueGeneratedOnAdd();
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=fcpc.db");
    }

    public class User
    {
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }

        public int RegionId { get; set; }
        public List<Vote> Votes { get; set; }
    }

    public class Vote
    {
        public int VoteId { get; set; }
        public int RegionId { get; set; }
        public string CandidateId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }

    public class Region
    {
        public int RegionId { get; set; }
        public string Name { get; set; }
        public int Max { get; set; }
    }

    public class Candidate
    {
        public string CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public int RegionId { get; set; }
        public string Photo { get; set; }
    }
}
