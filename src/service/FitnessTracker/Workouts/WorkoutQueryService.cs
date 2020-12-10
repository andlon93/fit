﻿using FitnessTracker.DTO;
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

        public IEnumerable<Workout> GetWorkouts(Paging paging, Filter filter)
        {
            return _workoutRepository.GetAll()
                                     .Where(w => (filter?.Ids?.Any() != true || filter.Ids.Contains(w.Id))
                                                 && (filter?.StartTime?.Any() != true || filter.StartTime.Any(x => (x.Start == null || w.StartTime >= x.Start) && (x.End == null || w.StartTime <= x.End))))
                                     .OrderByDescending(w => w.StartTime)
                                     .Skip(paging.Offset)
                                     .Take(paging.Rows < 0 ? int.MaxValue : paging.Rows);
        }

        public IEnumerable<GroupedWorkout> GetGroupedWorkouts(WorkoutGroupType groupBy, Paging paging)
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

            return groupedWorkouts;
        }

        private string CreateGroupTitle(WorkoutGroupType groupBy, object groupKey)
        {
            return groupBy switch
            {
                WorkoutGroupType.Day => groupKey.ToString(),
                WorkoutGroupType.Month => Utilities.TranslateToMonthInNorwegian(Convert.ToInt32(groupKey.ToString().Split("__")[1])),
                WorkoutGroupType.Year => groupKey.ToString(),
                _ => throw new ArgumentOutOfRangeException($"No conversion for the enum type {groupBy} exists."),
            };
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
