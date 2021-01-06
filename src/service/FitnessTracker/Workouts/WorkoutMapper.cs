using FitnessTracker.Workouts.DTOs;
using System;
using System.Collections.Generic;

namespace FitnessTracker.Workouts
{
    public static class WorkoutMapper
    {
        public static List<Workout> MapTcxToWorkouts(IEnumerable<TrainingCenterDatabase_t> workouts)
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

        private static Workout MapActivityToWorkout(Activity_t activity)
        {
            var startTime = DateTime.MaxValue;
            var totalTimeSeconds = 0.0;
            var distanceMeters = 0.0;
            var calories = 0;
            var steps = 0.0;
            var heartBeats = 0.0;
            var maximumHeartRate = 0;

            var maxAltitudeMeters = double.MinValue;
            var minAltitudeMeters = double.MaxValue;

            var maxSpeed = 0.0;
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
                if (lap.MaximumSpeedSpecified && lap.MaximumSpeed > maxSpeed)
                {
                    maxSpeed = lap.MaximumSpeed;
                }

                Trackpoint_t? prevTrack = null;
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

                    if (track.AltitudeMeters > maxAltitudeMeters)
                    {
                        maxAltitudeMeters = track.AltitudeMeters;
                    }
                    if (track.AltitudeMeters < minAltitudeMeters)
                    {
                        minAltitudeMeters = track.AltitudeMeters;
                    }
                    //TODO: Something either wrong with logic or data, give wonky results occasionally.. Read from JSON file instead.
                    if (prevTrack != null && track.Time > prevTrack.Time && track.DistanceMeters > prevTrack.DistanceMeters)
                    {
                        var seconds = track.Time.Subtract(prevTrack.Time).TotalSeconds;
                        var meters = track.DistanceMeters - prevTrack.DistanceMeters;
                        var speed = meters / seconds * 3.6;
                        if (speed > maxSpeed)
                        {
                            maxSpeed = speed;
                        }
                    }

                    prevTrack = track;
                }
            }

            var avgSpeed = 3.6 * distanceMeters / totalTimeSeconds;
            var maxPace = 60.0 / maxSpeed;
            var avgPace = 60.0 / avgSpeed;

            return new Workout
            {
                Id = Guid.NewGuid(),
                Sport = MapToSport(activity?.Sport),
                StartTime = startTime,
                TotalTimeSeconds = totalTimeSeconds,
                Distance = distanceMeters,
                Calories = calories,
                Cadence = (int)Math.Round(steps / totalTimeSeconds),
                AverageHeartRate = (int)Math.Round(heartBeats / totalTimeSeconds),
                MaximumHeartRate = maximumHeartRate,
                Positions = result,
                MaxAltitudeMeters = maxAltitudeMeters,
                MinAltitudeMeters = minAltitudeMeters,
                MaximumPace = maxPace,
                AveragePace = avgPace,
                AverageSpeed = avgSpeed,
                MaximumSpeed = maxSpeed,
            };
        }

        private static SportType? MapToSport(Sport_t? sport)
        {
            if (sport == null)
            {
                return SportType.Other;
            }

            return sport switch
            {
                Sport_t.Biking => SportType.CyclingSport,
                Sport_t.Running => SportType.Running,
                _ => SportType.Other,
            };
        }
    }
}
