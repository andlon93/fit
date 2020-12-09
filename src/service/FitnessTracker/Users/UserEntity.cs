using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FitnessTracker.Users
{
    public record UserEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        [JsonPropertyName("FirstName")]
        public string? FirstName { get; init; }

        [JsonPropertyName("LastName")]
        public string? LastName { get; init; }

        [JsonPropertyName("email")]
        public string? Email { get; init; }

        [JsonPropertyName("country")]
        public string? Country { get; init; }

        [JsonPropertyName("TimeZone")]
        public string? TimeZone { get; init; }

        [JsonPropertyName("Gender")]
        public string? Gender { get; init; }

        [JsonPropertyName("heigtInCm")]
        public int? HeigtInCm { get; init; }

        public IEnumerable<Guid> Workouts { get; set; } = new List<Guid>();

        [JsonPropertyName("SiteConnections")]
        public IEnumerable<SiteConnection>? SiteConnections { get; init; }
    }
    
    public record SiteConnection
    {
        [JsonPropertyName("site")]
        public string? Site { get; init; }

        [JsonPropertyName("identifier")]
        public string? Identifier { get; init; }
    } 
}
