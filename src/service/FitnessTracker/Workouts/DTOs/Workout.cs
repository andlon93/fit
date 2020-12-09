using System;
using System.Collections.Generic;

namespace FitnessTracker.Workouts.DTOs
{
    public record Workout
    {
        public Guid Id { get; init; }
        public DateTime? StartTime { get; init; }
        public string? Sport { get; init; }
        public int? Cadence { get; init; }
        public double? TotalTimeSeconds { get; init; }
        public double? Distance { get; init; }
        public int? Calories { get; init; }
        public int? AverageHeartRate { get; init; }
        public int? MaximumHeartRate { get; init; }
        public IEnumerable<TrackPoint>? Positions { get; init; }
    }

    public record TrackPoint
    {
        public double? AltitudeMeters { get; init; }
        public byte? Cadence { get; init; }
        public double? Distancemeters { get; init; }
        public int? HeartRate { get; init; }
        public Position? Position { get; init; }
        public DateTime? Time { get; init; }
    }

    public record Position
    {
        public double? LatitudeDegrees { get; init; }
        public double? LongitudeDegrees { get; init; }
    }
}
