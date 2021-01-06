using FitnessTracker.Users.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Users
{
    public class UserQueryService
    {
        private readonly UserRepository _userRepository;
        public UserQueryService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll().Select(ue => UserMapper.EntityToDto(ue));
        }

        public IEnumerable<User> GetUsersByIds(IEnumerable<Guid> ids)
        {
            return _userRepository.GetAll().Where(u => ids.Contains(u.Id)).Select(u => UserMapper.EntityToDto(u));
        }

        public async Task<User?> GetUserByGoogleId(string id)
        {
            var users = await _userRepository.GetUserByGoogleId(id);

            if (users == null || !users.Any()) { return null; }

            if (users.Count > 1) { throw new Exception($"Found more than one user with google id {id}."); }

            return UserMapper.EntityToDto(users.First());
        }
    }
}
