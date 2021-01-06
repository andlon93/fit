using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Users
{
    public class UserRepository
    {
        private Dictionary<Guid, UserEntity> _userEntities;

        public UserRepository()
        {
            _userEntities = new Dictionary<Guid, UserEntity>();
        }

        public IEnumerable<UserEntity> GetAll() => _userEntities.Values;

        public UserEntity GetById(Guid id) => _userEntities[id];

        public void AddWorkoutToUser(Guid userId, Guid workoutId)
        {
            if (_userEntities.TryGetValue(userId, out var user))
            {
                var workoutIds = user.WorkoutIds.ToList();
                workoutIds.Add(workoutId);
                user.WorkoutIds = workoutIds;
            }
        }

        public void RemoveWorkoutFromUser(Guid userId, Guid workoutId)
        {
            if (_userEntities.TryGetValue(userId, out var user))
            {
                var workoutIds = user.WorkoutIds.ToList();
                workoutIds.Remove(workoutId);
                user.WorkoutIds = workoutIds;
            }
        }

        public async Task<List<UserEntity>> GetUserByGoogleId(string id)
        {
            return await Task.FromResult(_userEntities.Values.Where(u => u.GoogleId == id).ToList());
        }

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
