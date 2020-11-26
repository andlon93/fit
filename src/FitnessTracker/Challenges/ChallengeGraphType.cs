using FitnessTracker.Users;
using FitnessTracker.Workouts;
using FitnessTracker.Workouts.DTOs;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Challenges
{
    public class ChallengeGraphType : ObjectGraphType<Challenge>
    {
        private readonly WorkoutService _workoutService;
        private readonly UserService _userService;
        public ChallengeGraphType(WorkoutService workoutService, UserService userService)
        {
            _workoutService = workoutService;
            _userService = userService;

            Field<StringGraphType, string>()
                .Name(nameof(Challenge.Name))
                .Description("");

            Field<ChallengeTypeEnum, ChallengeType>()
                .Name(nameof(Challenge.Type))
                .Description("");

            Field<DateTimeGraphType, DateTime>()
                .Name(nameof(Challenge.StartTime))
                .Description("");

            Field<DateTimeGraphType, DateTime>()
                .Name(nameof(Challenge.EndTime))
                .Description("");

            Field<ListGraphType<CompetitorGraphType>, IEnumerable<Competitor>>()
                .Name("leaderboard")
                .Description("")
                .Resolve(ResolveCompetitors);
        }

        private IEnumerable<Competitor> ResolveCompetitors(IResolveFieldContext<Challenge> context)
        {
            if (context?.Source?.UserIds?.Any() != true)
            {
                return new List<Competitor>();
            }

            var challenge = context.Source;

            var workouts = _workoutService.GetWorkouts(
                new DTO.Paging { Offset = 0, Rows = 999999 },
                new DTO.Filter
                {
                    StartTime = new List<DateTimeRange>
                    {
                        new DateTimeRange
                        {
                            Start = challenge.StartTime,
                            End = challenge.EndTime
                        }
                    },
                    UserIds = challenge.UserIds,
                });

            var users = _userService.GetUsersByIds(challenge.UserIds);

            var leaderboard = users.Select(user => new Competitor
            {
                User = user,
                Score = CalculateScore(challenge, workouts.Where(w => user.WorkoutIds?.Contains(w.Id) == true))
            }).OrderByDescending(competitor => competitor.Score)
              .ToList();

            for (int i = 0; i < leaderboard.Count; i++)
            {
                leaderboard[i].Rank = i + 1;
            }

            return leaderboard;
        }

        private int CalculateScore(Challenge challenge, IEnumerable<Workout> workouts)
        {
            return challenge.Type switch
            {
                ChallengeType.MostWorkouts => workouts.Count(),
                ChallengeType.MostActiveMinutes => (int)Math.Round(workouts.Select(w => w.TotalTimeSeconds ?? 0).Sum(x => x) / 60.0),
                _ => 0,
            };
        }
    }
}
