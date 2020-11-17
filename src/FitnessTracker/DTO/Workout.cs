using System;

namespace FitnessTracker.DTO
{
    public record Workout
    {
        public DateTime? StartTime { get; init; }
        public string? Sport { get; init; }
        public int? Cadence { get; init; }
        public double? TotalTimeSeconds { get; init; }
        public double? Distance { get; init; }
        public int? Calories { get; init; }
        public int? AverageHeartRate { get; init; }
        public int? MaximumHeartRate { get; init; }
    }
}
