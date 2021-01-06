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

        public Guid SaveOrUpdateUser(UserEntity user)
        {
            if (string.IsNullOrEmpty(user.GoogleId)) { throw new Exception("Can not create a user without the users google authentication id."); }
            
            return _userRepository.SaveOrUpdateUser(user);
        } 
    }
}
