using System;

namespace FitnessTracker.Workouts.DTOs
{
    public record DateTimeRange
    {
        public DateTime? Start { get; init; }
        public DateTime? End { get; init; }
    }
}