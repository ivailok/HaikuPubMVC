using Haiku.Data;
using Haiku.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Data
{
    public class HaikuContext : DbContext, IDbContext
    {
        public HaikuContext()
            : base("name=HaikuDBConnectionString")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<HaikuEntity> Haikus { get; set; }

        public DbSet<HaikuRating> Ratings { get; set; }

        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
