using System;
using System.Collections.Generic;

namespace FitnessTracker.Users.DTOs
{
    public record Filter
    {
        public IEnumerable<Guid> Ids { get; init; } = new List<Guid>();
    }
}
