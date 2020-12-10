using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FitnessTracker.Users
{
    public record UserEntity
    {
        public Guid Id { get; init; } = new Guid("ae3db2eb-c260-473a-bc57-48e65946aa2d");

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

        public IEnumerable<Guid> WorkoutIds { get; set; } = new List<Guid>();

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
