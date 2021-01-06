using System;

namespace FitnessTracker.Users
{
    public class UserCommandService
    {
        private readonly UserRepository _userRepository;
        public UserCommandService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Guid SaveOrUpdateUser(UserEntity user) => _userRepository.SaveOrUpdateUser(user);
    }
}
