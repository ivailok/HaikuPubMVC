using Haiku.Data.Entities;
using Haiku.DTO.Request;
using Haiku.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Services
{
    public static class Mapper
    {
        public static User MapAuthorRegisterDtoToUser(AuthorRegisteringDto dto)
        {
            return new User()
            {
                Nickname = dto.Nickname,
                AccessToken = dto.PublishCode,
                Role = UserRole.Author,
                HaikusRatingSum = 0.0,
                HaikusCount = 0
            };
        }

        public static HaikuEntity MapHaikuPublishingDtoToHaikuEntity(HaikuPublishingDto dto)
        {
            return new HaikuEntity()
            {
                Text = dto.Text,
                DatePublished = DateTime.Now,
                RatingsSum = 0,
                RatingsCount = 0
            };
        }

        public static HaikuPublishedDto MapHaikuEntityToHaikuPublishedDto(HaikuEntity haiku)
        {
            return new HaikuPublishedDto()
            {
                Id = haiku.Id,
                DatePublished = haiku.DatePublished
            };
        }

        public static HaikuGetDto MapHaikuEntityToHaikuGetDto(HaikuEntity haiku)
        {
            return new HaikuGetDto()
            {
                Id = haiku.Id,
                Text = haiku.Text,
                Rating = haiku.Rating,
                Author = haiku.User.Nickname
            };
        }

        public static HaikuRating MapHaikuRateDtoToHaikuRating(HaikuRatingDto dto)
        {
            return new HaikuRating()
            {
                Value = dto.Rating,
                DateCreated = DateTime.Now
            };
        }

        public static UserGetDto MapUserToUserGetDto(User user)
        {
            var dto = new UserGetDto()
            {
                Nickname = user.Nickname,
                Rating = user.Rating,
                Vip = user.Role == UserRole.VIP
            };
            var haikus = new List<HaikuGetDto>();
            foreach (var haiku in user.Haikus)
            {
                haikus.Add(MapHaikuEntityToHaikuGetDto(haiku));
            }
            dto.Haikus = haikus;
            return dto;
        }

        public static Report MapHaikuReportingDtoToReport(HaikuReportingDto dto)
        {
            return new Report()
            {
                Reason = dto.Reason,
                DateSent = DateTime.Now
            };
        }

        public static ReportGetDto MapReportToReportGetDto(Report report)
        {
            return new ReportGetDto()
            {
                Reason = report.Reason,
                DateSent = report.DateSent,
                HaikuId = report.HaikuId
            };
        }
    }
}
