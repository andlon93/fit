using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FitnessTracker.Users
{
    public class UserRepository
    {
        private const string path = "/app/Data/endomondo-2020-11-14/Profile";
        private List<UserEntity>? userEntities;

        public IEnumerable<UserEntity> GetUserEntities()
        {
            if (userEntities != null)
            {
                return userEntities;
            }

            userEntities = new List<UserEntity>();
            foreach (string filename in Directory.EnumerateFiles(path, "*.json"))
            {
                var user = JsonSerializer.Deserialize<UserEntity>(File.ReadAllText(filename));
                if (user != null)
                {
                    userEntities.Add(user);
                }
            }

            return userEntities;
        }
    }
}
