using Haiku.Data;
using Haiku.Data.Entities;
using Haiku.DTO;
using Haiku.DTO.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Services
{
    public class SessionsService : BaseService, ISessionsService
    {
        public const string SessionTokenLabelConst = "SessionToken";
        private const string SessionTokenSeparator = "_";
        private const string SessionSalt = "SVGW3P3XZYkCTMXEwQ/x/g==";

        public SessionsService(IUnitOfWork unitOfWork)
             : base(unitOfWork)
        {
        }

        private static string GenerateNewSessionToken()
        {
            return Guid.NewGuid().ToString();
        }

        private static void GetTokens(string session, out string token, out string nickname)
        {
            int splitIndex = session.IndexOf(SessionsService.SessionTokenSeparator, StringComparison.InvariantCulture);
            if (splitIndex == -1)
            {
                throw new SessionException("You should re-enter your credentials.");
            }

            nickname = session.Substring(0, splitIndex);
            token = session.Substring(splitIndex + 1);
        }

        public async Task<SessionDto> AddNewSessionAsync(string nickname, string salt)
        {
            string token = SessionsService.GenerateNewSessionToken();
            string tokenHash = await HashingService.GetHashAsync(token, salt, HashingType.Weak).ConfigureAwait(false);
            this.UnitOfWork.SessionsRepository.Add(new Session()
            {
                Token = tokenHash,
                Nickname = nickname,
                From = DateTime.Now
            });

            return new SessionDto()
            {
                Nickname = nickname,
                Token = token
            };
        }

        public async Task<SessionDto> CheckAndUpdateSessionAsync(SessionDto session)
        {
            var user = await this.UnitOfWork.UsersRepository.GetUniqueAsync(e => e.Nickname == session.Nickname).ConfigureAwait(false);
            if (user == null)
            {
                throw new SessionException("You should re-enter your credentials.");
            }

            var tokenHash = await HashingService.GetHashAsync(session.Token, user.Salt, HashingType.Weak).ConfigureAwait(false);
            var sessionObj = await this.UnitOfWork.SessionsRepository.GetUniqueAsync(e => e.Token == tokenHash).ConfigureAwait(false);
            if (sessionObj == null)
            {
                throw new SessionException("You should re-enter your credentials.");
            }

            if (sessionObj.Nickname != session.Nickname)
            {
                throw new SessionException("You should re-enter your credentials.");
            }

            string newToken = SessionsService.GenerateNewSessionToken();
            sessionObj.Token = await HashingService.GetHashAsync(newToken, user.Salt, HashingType.Weak).ConfigureAwait(false);
            sessionObj.From = DateTime.Now;
            await this.UnitOfWork.CommitAsync().ConfigureAwait(false);

            return new SessionDto
            {
                Nickname = session.Nickname,
                Token = newToken
            };
        }
    }
}
