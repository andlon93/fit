using FitnessTracker.Users.DTOs;
using System;
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

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll().Select(ue => MapUserEntityToDto(ue));
        }

        public IEnumerable<User> GetUsersByIds(IEnumerable<Guid> ids)
        {
            return _userRepository.GetAll().Where(u => ids.Contains(u.Id)).Select(u => MapUserEntityToDto(u));
        }

        public Guid SaveOrUpdateUser(UserEntity user) => _userRepository.SaveOrUpdateUser(user);

        private User MapUserEntityToDto(UserEntity entity)
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
                WorkoutIds = entity.WorkoutIds,
                SiteConnections = entity?.SiteConnections?.Select(e => new DTOs.SiteConnection { Site = SiteEnumMapper(e.Site), Identifier = e.Identifier })
            };
        }

        private static SiteType SiteEnumMapper(string? site)
        {
            return (site?.ToLower().Trim()) switch
            {
                "facebook" => SiteType.Facebook,
                "google" => SiteType.Google,
                "polar" => SiteType.Polar,
                "garmin" => SiteType.Garmin,
                _ => SiteType.Unknown,
            };
        }
    }
}
