using System;
using System.Collections.Generic;

namespace FitnessTracker.DTO
{
    public record Filter
    {
        public IEnumerable<Guid>? Ids { get; init; }
    }
}