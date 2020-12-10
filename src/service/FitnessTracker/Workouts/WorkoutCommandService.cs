﻿using FitnessTracker.Workouts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Workouts
{
    public class WorkoutCommandService
    {
        private readonly WorkoutRepository _workoutRepository;
        public WorkoutCommandService(WorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }
        public Workout CreateWorkout(Workout workout) => _workoutRepository.SaveOrUpdateWorkouts(new List<Workout> { workout }).First();

        public IEnumerable<Guid> SaveWorkoutsFromZipFile(IEnumerable<TrainingCenterDatabase_t> workouts)
        {
            var mappedWorkouts = MapTcxToWorkouts(workouts);

            _workoutRepository.SaveOrUpdateWorkouts(mappedWorkouts);

            return mappedWorkouts.Select(w => w.Id);
        }

        private List<Workout> MapTcxToWorkouts(IEnumerable<TrainingCenterDatabase_t> workouts)
        {
            var result = new List<Workout>();
            foreach (var t in workouts)
            {
                if (t?.Activities?.Activity == null)
                {
                    continue;
                }
                foreach (var activity in t.Activities.Activity)
                {
                    if (activity == null)
                    {
                        continue;
                    }
                    result.Add(MapActivityToWorkout(activity));
                }
            }
            return result;
        }

        private Workout MapActivityToWorkout(Activity_t activity)
        {
            var startTime = DateTime.MaxValue;
            var totalTimeSeconds = 0.0;
            var distanceMeters = 0.0;
            var calories = 0;
            var steps = 0.0;
            var heartBeats = 0.0;
            var maximumHeartRate = 0;

            var result = new List<TrackPoint>();
            foreach (var lap in activity.Lap)
            {
                if (lap.StartTime < startTime)
                {
                    startTime = lap.StartTime;
                }
                totalTimeSeconds += lap.TotalTimeSeconds;
                distanceMeters += lap.DistanceMeters;
                calories += lap.Calories;
                steps += lap.Cadence * lap.TotalTimeSeconds;
                heartBeats += (lap.AverageHeartRateBpm?.Value ?? 0) * lap.TotalTimeSeconds;
                if (lap.MaximumHeartRateBpm != null && lap.MaximumHeartRateBpm.Value > maximumHeartRate)
                {
                    maximumHeartRate = lap.MaximumHeartRateBpm.Value;
                }
                foreach (var track in lap.Track)
                {
                    result.Add(new TrackPoint
                    {
                        AltitudeMeters = track.AltitudeMeters,
                        Cadence = track.Cadence,
                        Distancemeters = track.DistanceMeters,
                        HeartRate = track.HeartRateBpm?.Value,
                        Position = new Position { LatitudeDegrees = track.Position?.LatitudeDegrees, LongitudeDegrees = track.Position?.LongitudeDegrees },
                        Time = track.Time,
                    });
                }
            }

            return new Workout
            {
                Id = Guid.NewGuid(),
                Sport = activity?.Sport.ToString(),
                StartTime = startTime,
                TotalTimeSeconds = totalTimeSeconds,
                Distance = distanceMeters,
                Calories = calories,
                Cadence = (int)Math.Round(steps / totalTimeSeconds),
                AverageHeartRate = (int)Math.Round(heartBeats / totalTimeSeconds),
                MaximumHeartRate = maximumHeartRate,
                Positions = result,
            }; ;
        }
    }
}