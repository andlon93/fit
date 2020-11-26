using FitnessTracker.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Challenges
{
    public class ChallengeService
    {
        private readonly UserService _userService;

        public ChallengeService(UserService userService)
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
                    Name = $"Flest aktive minutter i {TranslateToMonthInNorwegian(now.Month)}",
                    Type = ChallengeType.MostActiveMinutes,
                    StartTime = new DateTime(now.Year, now.Month, 1, 0, 0, 0),
                    EndTime = new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1).AddSeconds(-1),
                    UserIds = userIds,
                }
            };
        }

        private string TranslateToMonthInNorwegian(int month)
        {
            return month switch
            {
                1 => "januar",
                2 => "februar",
                3 => "mars",
                4 => "april",
                5 => "mai",
                6 => "juni",
                7 => "juli",
                8 => "august",
                9 => "september",
                10 => "oktober",
                11 => "november",
                12 => "desember",
                _ => throw new ArgumentOutOfRangeException(nameof(month), "Month must be an integer between 1 and 12."),
            };
        }        
    }
}
