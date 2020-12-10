using System;
using System.Collections.Generic;

namespace FitnessTracker.WorkoutsGrouped.DTOs
{
    public record GroupedWorkout
    {
        public string? Title { get; init; }
        public double DurationInSeconds { get; init; } = 0;
        public double DistanceInMeters { get; init; } = 0;
        public int Calories { get; init; } = 0;
        public List<Guid> WorkoutIds { get; init; } = new List<Guid>();
        public int NumberOfWorkouts => WorkoutIds.Count;
    }
}
