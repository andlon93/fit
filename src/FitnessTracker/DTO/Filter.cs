using System.Collections.Generic;

namespace FitnessTracker.DTO
{
    public record Filter
    {
        public IEnumerable<string>? Id { get; init; }
    }
}