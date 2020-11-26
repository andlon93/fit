using System;
using System.Collections.Generic;

namespace FitnessTracker.Users.DTOs
{
    public record User
    {
        public Guid Id { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? Country { get; init; }
        public string? TimeZone { get; init; }
        public string? Gender { get; init; }
        public int? Height { get; init; } // In cm
        public IEnumerable<Guid>? WorkoutIds { get; init; }
        public IEnumerable<SiteConnection>? SiteConnections { get; init; }
    }        
}
