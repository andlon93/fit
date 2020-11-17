using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FitnessTracker.TCX
{
    public partial class EndomondoWorkout
    {
        [JsonPropertyName("sport")]
        public string? Sport { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("created_date")]
        public string? CreatedDate { get; set; }

        [JsonPropertyName("start_time")]
        public string? StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public string? EndTime { get; set; }

        [JsonPropertyName("duration_s")]
        public long? DurationS { get; set; }

        [JsonPropertyName("distance_km")]
        public double? DistanceKm { get; set; }

        [JsonPropertyName("calories_kcal")]
        public long? CaloriesKcal { get; set; }

        [JsonPropertyName("altitude_min_m")]
        public double? AltitudeMinM { get; set; }

        [JsonPropertyName("altitude_max_m")]
        public double? AltitudeMaxM { get; set; }

        [JsonPropertyName("heart_rate_avg_bpm")]
        public long? HeartRateAvgBpm { get; set; }

        [JsonPropertyName("heart_rate_max_bpm")]
        public long? HeartRateMaxBpm { get; set; }

        [JsonPropertyName("cadence_avg_rpm")]
        public long? CadenceAvgRpm { get; set; }

        [JsonPropertyName("speed_avg_kmh")]
        public double? SpeedAvgKmh { get; set; }

        [JsonPropertyName("speed_max_kmh")]
        public double? SpeedMaxKmh { get; set; }

        [JsonPropertyName("ascend_m")]
        public double? AscendM { get; set; }

        [JsonPropertyName("descend_m")]
        public double? DescendM { get; set; }

        [JsonPropertyName("points")]
        public Point[][]? Points { get; set; }
    }

    public partial class Point
    {
        [JsonPropertyName("location")]
        public Location[][]? Location { get; set; }

        [JsonPropertyName("heart_rate_bpm")]
        public long? HeartRateBpm { get; set; }

        [JsonPropertyName("distance_km")]
        public double? DistanceKm { get; set; }

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        [JsonPropertyName("altitude")]
        public double? Altitude { get; set; }
    }

    public partial class Location
    {
        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }
    }
}
