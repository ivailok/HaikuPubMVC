using Haiku.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Data
{
    public interface IDbContext : IDisposable
    {
        DbSet<User> Users { get; set; }

        DbSet<HaikuEntity> Haikus { get; set; }

        DbSet<HaikuRating> Ratings { get; set; }

        Task<int> SaveChangesAsync();
    }
}
