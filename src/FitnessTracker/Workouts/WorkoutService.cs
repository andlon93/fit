using FitnessTracker.DTO;
using FitnessTracker.Workouts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Workouts
{
    public class WorkoutService
    {
        private readonly WorkoutRepository _workoutRepository;

        public WorkoutService(WorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public IEnumerable<Workout> GetWorkouts(Paging paging, Filter filter)
        {
            return _workoutRepository.GetAll()
                                     .Where(w => filter?.Ids?.Any() != true || filter.Ids.Contains(w.Id))
                                     .OrderByDescending(w => w.StartTime)
                                     .Skip(paging.Offset)
                                     .Take(paging.Rows);
        }

        public IEnumerable<Guid> SaveWorkoutsFromZipFile(IEnumerable<TrainingCenterDatabase_t> workouts)
        {
            var mappedWorkouts = workouts.Select(w => MapTcxToWorkout(w));

            _workoutRepository.SaveWorkouts(mappedWorkouts);

            return mappedWorkouts.Select(w => w.Id);
        }

        private Workout MapTcxToWorkout(TrainingCenterDatabase_t w)
        {
            var activity = w.Activities.Activity.FirstOrDefault();
            var lap = activity?.Lap?.FirstOrDefault();
            return new Workout
            {
                Id = Guid.NewGuid(),
                Sport = activity?.Sport.ToString(),
                StartTime = lap?.StartTime,
                TotalTimeSeconds = lap?.TotalTimeSeconds,
                Distance = lap?.DistanceMeters,
                Calories = lap?.Calories,
                Cadence = lap?.Cadence,
                AverageHeartRate = lap?.AverageHeartRateBpm?.Value,
                MaximumHeartRate = lap?.MaximumHeartRateBpm?.Value,
                Positions = ConcatenateLaps(activity)
            };
        }

        private IEnumerable<TrackPoint>? ConcatenateLaps(Activity_t? activity)
        {
            if (activity == null) { return null; }

            var result = new List<TrackPoint>();
            foreach (var lap in activity.Lap)
            {
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

            return result;
        }
    }
}
