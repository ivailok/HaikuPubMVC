using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haiku.DTO.Request;
using Haiku.DTO.Response;

namespace Haiku.Services
{
    public interface IUsersService
    {
        Task<string> GetCurrentUser(string publishCode);

        Task<bool> ConfirmAuthorIdentityAsync(string nickname, string publishCode);

        Task<bool> ConfirmAdministratorIdentityAsync(string manageToken);

        Task RegisterAuthorAsync(AuthorRegisteringDto dto);
        
        Task<HaikuPublishedDto> PublishHaikuAsync(string nickname, HaikuPublishingDto dto);

        PagingMetadata GetUsersPagingMetadata();
        
        Task<IEnumerable<UserGetDto>> GetUsersAsync(UsersGetQueryParams queryParams);

        Task<UserGetDto> GetUserAsync(string nickname);

        Task DeleteHaikusAsync(string nickname);

        Task DeleteProfileAsync(string nickname);

        Task ChangeUserRoleAsync(string nickname, ChangeableUserRole role);
    }
}
