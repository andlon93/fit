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
                                     .Where(w => (filter?.Ids?.Any() != true || filter.Ids.Contains(w.Id))
                                                 //&& (filter?.UserIds?.Any() != true)  // TODO: Workouts must contain UserId || filter.UserId.Contains(w.UserId)
                                                 && (filter?.StartTime?.Any() != true || filter.StartTime.Any(x => (x.Start == null || w.StartTime >= x.Start) && (x.End == null || w.StartTime <= x.End))))
                                     .OrderByDescending(w => w.StartTime)
                                     .Skip(paging.Offset)
                                     .Take(paging.Rows);
        }

        public IEnumerable<Guid> SaveWorkoutsFromZipFile(IEnumerable<TrainingCenterDatabase_t> workouts)
        {
            var mappedWorkouts = MapTcxToWorkouts(workouts);

            _workoutRepository.SaveOrUpdateWorkouts(mappedWorkouts);

            return mappedWorkouts.Select(w => w.Id);
        }

        private List<Workout> MapTcxToWorkouts(IEnumerable<TrainingCenterDatabase_t> workouts)
        {
            return workouts.Select(w =>
            {
                var activity = w.Activities.Activity.FirstOrDefault();
                var lap = activity?.Lap?.FirstOrDefault();
                return new Workout
                {
                    Id = Guid.NewGuid(),
                    Sport = activity?.Sport.ToString(),
                    StartTime = lap?.StartTime, // TODO: Henter kun ut fra første runde (kan de være i en annen rekkefølge enn kronologisk?)
                    TotalTimeSeconds = lap?.TotalTimeSeconds, // TODO: Henter kun ut fra første runde
                    Distance = lap?.DistanceMeters, // TODO: Henter kun ut fra første runde
                    Calories = lap?.Calories, // TODO: Henter kun ut fra første runde
                    Cadence = lap?.Cadence, // TODO: Henter kun ut fra første runde
                    AverageHeartRate = lap?.AverageHeartRateBpm?.Value, // TODO: Henter kun ut fra første runde
                    MaximumHeartRate = lap?.MaximumHeartRateBpm?.Value, // TODO: Henter kun ut fra første runde
                    Positions = ConcatenateLaps(activity)
                };
            }).ToList();
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
