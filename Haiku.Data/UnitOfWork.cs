using Haiku.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext context;
        private IAsyncRepository<User> usersRepository;
        private IAsyncRepository<HaikuEntity> haikusRepository;
        private IAsyncRepository<Report> reportsRepository;
        private IAsyncRepository<HaikuRating> ratingsRepository;
        private IAsyncRepository<Session> sessionsRepository;
        private bool disposedValue = false;

        public UnitOfWork(
            IDbContext context,
            IAsyncRepository<User> usersRepository, 
            IAsyncRepository<HaikuEntity> haikusRepository,
            IAsyncRepository<Report> reportsRepository,
            IAsyncRepository<HaikuRating> ratingsRepository,
            IAsyncRepository<Session> sessionsRepository)
        {
            this.context = context;
            this.usersRepository = usersRepository;
            this.haikusRepository = haikusRepository;
            this.reportsRepository = reportsRepository;
            this.ratingsRepository = ratingsRepository;
            this.sessionsRepository = sessionsRepository;
        }

        public IAsyncRepository<User> UsersRepository
        {
            get { return this.usersRepository; }
        }

        public IAsyncRepository<HaikuEntity> HaikusRepository
        {
            get { return this.haikusRepository; }
        }

        public IAsyncRepository<Report> ReportsRepository
        {
            get { return this.reportsRepository; }
            
        }

        public IAsyncRepository<HaikuRating> RatingsRepository
        {
            get { return this.ratingsRepository; }
        }

        public IAsyncRepository<Session> SessionsRepository
        {
            get { return this.sessionsRepository; }
        }

        public Task CommitAsync()
        {
            return this.context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (this.context != null)
                    {
                        this.context.Dispose();
                        this.context = null;
                    }
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
