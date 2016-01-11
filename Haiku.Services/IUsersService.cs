using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haiku.DTO.Request;
using Haiku.DTO.Response;
using Haiku.DTO;

namespace Haiku.Services
{
    public interface IUsersService
    {
        Task<SessionDto> RegisterAuthorAsync(AuthorRegisteringDto dto);

        Task<SessionDto> LoginAsync(AuthorLoginDto dto);

        Task LogoutAsync(string nickname);
        
        Task<HaikuPublishedDto> PublishHaikuAsync(string nickname, HaikuPublishingDto dto);

        PagingMetadata GetUsersPagingMetadata();
        
        Task<IEnumerable<UserGetDto>> GetUsersAsync(UsersGetQueryParams queryParams);
        
        Task<UserGetDto> GetUserAsync(string nickname);

        Task DeleteHaikusAsync(string nickname);

        Task DeleteProfileAsync(string nickname);

        Task<int> GetRatingForHaiku(string nickname, int haikuId);
    }
}
