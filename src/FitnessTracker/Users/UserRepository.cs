using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FitnessTracker.Users
{
    public class UserRepository
    {
        private const string path = "/app/Data/endomondo-2020-11-14/Profile";
        private Dictionary<Guid, UserEntity> _userEntities;

        public UserRepository()
        {
            _userEntities = new Dictionary<Guid, UserEntity>();
        }

        public IEnumerable<UserEntity> GetAll() => _userEntities.Values;

        public Guid SaveOrUpdateUser(UserEntity user)
        {
            if (_userEntities.ContainsKey(user.Id))
            {
                _userEntities[user.Id] = user;
            }

            _userEntities.Add(user.Id, user);

            return user.Id;
        }
    }
}
