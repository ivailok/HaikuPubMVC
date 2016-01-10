using Haiku.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Services
{
    public interface ISessionsService
    {
        Task<SessionDto> AddNewSessionAsync(string nickname, string salt);

        Task<SessionDto> CheckAndUpdateSessionAsync(SessionDto session);
    }
}
