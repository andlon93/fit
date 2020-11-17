using System.Collections.Generic;
using System.Xml.Serialization;

namespace FitnessTracker.TCX
{
    // https://www8.garmin.com/xmlschemas/TrainingCenterDatabasev2.xsd
    [XmlRoot(Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2", IsNullable = false)]
    public record TrainingCenterDatabase
    {
        public List<Activity>? Activities { get; init; }
    }

    public record Activity
    {
        [XmlAttribute()]
        public string? Sport { get; init; }
        public string? Id { get; init; }
        public Lap? Lap { get; init; }
        public string? Notes { get; init; }
    }

    public record Lap
    {
        [XmlAttribute()]
        public string? StartTime { get; init; }
        public double? TotalTimeSeconds { get; init; }
        public double? DistanceMeters { get; init; }
        public double? MaximumSpeeed { get; init; }
        public int? Calories { get; init; }
        public HeartRateInBeatsPerMinute? AverageHeartRateBpm { get; init; }
        public HeartRateInBeatsPerMinute? MaximumHeartRateBpm { get; init; }
        public string? Intensity { get; init; }
        public int? Cadence { get; init; }
        public string? TriggerMethod { get; init; }
        public List<TrackPoint>? Track { get; init; }
    }

    [XmlType(TypeName = "HeartRateInBeatsPerMinute_t")]
    public record HeartRateInBeatsPerMinute
    {
        public int? Value { get; init; }
    }

    public record TrackPoint
    {
        public string? Time { get; init; }
        public Position? Position { get; init; }
        public double? AltitudeMeters { get; init; }
        public double? DistanceMeters { get; init; }
        public HeartRateBpm? HeartRateBpm { get; init; }
        public int? Cadence { get; init; }
    }

    public record HeartRateBpm
    {
        public string? Value { get; init; }
    }

    public record Position
    {
        public double? LatitudeDegrees { get; init; }
        public double? LongitudeDegrees { get; init; }
    }
}
