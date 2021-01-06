using FitnessTracker.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Challenges
{
    public class ChallengeService
    {
        private readonly UserQueryService _userService;

        public ChallengeService(UserQueryService userService)
        {
            _userService = userService;
        }

        public IEnumerable<Challenge> GetChallenges()
        {
            var userIds = _userService.GetAllUsers().Select(u => u.Id);
            var now = DateTime.Now;

            return new List<Challenge>
            {
                new Challenge
                {
                    Name = $"Flest økter i {now.Year}",
                    Type = ChallengeType.MostWorkouts,
                    StartTime = new DateTime(now.Year, 1, 1, 0, 0, 0),
                    EndTime = new DateTime(now.Year, 12, 31, 23, 59, 59),
                    UserIds = userIds,
                },
                new Challenge
                {
                    Name = $"Flest aktive minutter i {Utilities.TranslateToMonthInNorwegian(now.Month)}",
                    Type = ChallengeType.MostActiveMinutes,
                    StartTime = new DateTime(now.Year, now.Month, 1, 0, 0, 0),
                    EndTime = new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1).AddSeconds(-1),
                    UserIds = userIds,
                }
            };
        }      
    }
}
