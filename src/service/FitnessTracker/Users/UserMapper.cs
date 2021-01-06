using FitnessTracker.Users.DTOs;
using System;
using System.Linq;

namespace FitnessTracker.Users
{
    public static class UserMapper
    {
        public static User EntityToDto(UserEntity entity)
        {
            return new User
            {
                Id = entity.Id,
                GoogleId = entity?.SiteConnections?.FirstOrDefault(e => e.Site != null && e.Site.ToLower().Trim().Equals("google"))?.Identifier ?? throw new Exception("User entity has no google Id. This is required."),
                Country = entity.Country,
                Email = entity.Email,
                Gender = entity.Gender,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Height = entity.HeigtInCm,
                TimeZone = entity.TimeZone,
                WorkoutIds = entity.WorkoutIds,
                SiteConnections = entity?.SiteConnections?.Select(e => new DTOs.SiteConnection { Site = SiteStringToEnum(e.Site), Identifier = e.Identifier })
            };
        }

        public static SiteType SiteStringToEnum(string? site)
        {
            return (site?.ToLower().Trim()) switch
            {
                "facebook" => SiteType.Facebook,
                "polar" => SiteType.Polar,
                "garmin" => SiteType.Garmin,
                _ => SiteType.Unknown,
            };
        }
    }
}
