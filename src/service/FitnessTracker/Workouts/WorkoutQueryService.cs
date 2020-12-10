using FitnessTracker.DTO;
using FitnessTracker.Workouts.DTOs;
using FitnessTracker.WorkoutsGrouped.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Workouts
{
    public class WorkoutQueryService
    {
        private readonly WorkoutRepository _workoutRepository;

        public WorkoutQueryService(WorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public List<Workout> GetWorkouts(Paging paging, Filter filter)
        {
            return _workoutRepository.GetAll()
                                     .Where(w => (filter?.Ids?.Any() != true || filter.Ids.Contains(w.Id))
                                                 && (filter?.StartTime?.Any() != true || filter.StartTime.Any(x => (x.Start == null || w.StartTime >= x.Start) && (x.End == null || w.StartTime <= x.End))))
                                     .OrderByDescending(w => w.StartTime)
                                     .Skip(paging.Offset)
                                     .Take(paging.Rows < 0 ? int.MaxValue : paging.Rows)
                                     .ToList();
        }

        public List<GroupedWorkout> GetGroupedWorkouts(WorkoutGroupType groupBy, Paging paging)
        {
            var groupedWorkouts = _workoutRepository.GetAll()
                .Where(workout => workout.StartTime != null)
#pragma warning disable CS8629 // Nullable value type may be null.
                .GroupBy(workout => GroupByMapper(groupBy, workout.StartTime.Value))
#pragma warning restore CS8629 // Nullable value type may be null.
                .OrderByDescending(w => w.Key)
                .Skip(paging.Offset)
                .Take(paging.Rows < 0 ? int.MaxValue : paging.Rows)
                .Select(workoutGroup => new GroupedWorkout
                {
                    Title = CreateGroupTitle(groupBy, workoutGroup.Key),
                    WorkoutIds = workoutGroup.Select(w => w.Id).ToList(),
                    Calories = workoutGroup.Sum(w => w.Calories) ?? 0,
                    DistanceInMeters = workoutGroup.Sum(w => w.Distance) ?? 0,
                    DurationInSeconds = workoutGroup.Sum(w => w.TotalTimeSeconds) ?? 0,
                });

            return groupedWorkouts.ToList();
        }

        private string CreateGroupTitle(WorkoutGroupType groupBy, object groupKey)
        {
            var keyAsString = groupKey?.ToString();
            if (keyAsString == null) { return string.Empty; }
            return groupBy switch
            {
                WorkoutGroupType.Day => keyAsString,
                WorkoutGroupType.Month => MapMonthGroupTitle(keyAsString),
                WorkoutGroupType.Year => keyAsString,
                _ => throw new ArgumentOutOfRangeException($"No conversion for the enum type {groupBy} exists."),
            };
        }

        private string MapMonthGroupTitle(string key)
        {
            var splited = key.ToString().Split("__");
            return $"{Utilities.TranslateToMonthInNorwegian(Convert.ToInt32(splited[1]))} {splited[0]}";
        }

        private object GroupByMapper(WorkoutGroupType groupBy, DateTime time)
        {
            return groupBy switch
            {
                WorkoutGroupType.Day => time.Date,
                WorkoutGroupType.Month => time.Year + "__" + time.Month,
                WorkoutGroupType.Year => time.Year,
                _ => throw new ArgumentOutOfRangeException($"No conversion for the enum type {groupBy} exists."),
            };
        }
    }
}
