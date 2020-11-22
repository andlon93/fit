using FitnessTracker.Users.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Users
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetUsers()
        {
            var userEntitties = _userRepository.GetUserEntities();
            return userEntitties.Select(ue => MapUserEntity(ue));
        }

        private User MapUserEntity(UserEntity entity)
        {
            return new User
            {
                Id = entity.Id,
                Country = entity.Country,
                Email = entity.Email,
                Gender = entity.Gender,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Height = entity.HeigtInCm,
                TimeZone = entity.TimeZone,
                SiteConnections = entity?.SiteConnections?.Select(e => new DTOs.SiteConnection { Site = SiteEnumMapper(e.Site), Identifier = e.Identifier })
            };
        }

        private static SiteType SiteEnumMapper(string? site)
        {
            switch (site?.ToLower().Trim())
            {
                case "facebook":
                    return SiteType.Facebook;
                case "google":
                    return SiteType.Google;
                case "polar":
                    return SiteType.Polar;
                case "garmin":
                    return SiteType.Garmin;
                default:
                    return SiteType.Unknown;
            }
        }
    }
}
