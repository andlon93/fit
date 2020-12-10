using System;
using System.Collections.Generic;

namespace FitnessTracker.Workouts.DTOs
{
    public record Filter
    {
        public IEnumerable<Guid> Ids { get; init; } = new List<Guid>();
        public IEnumerable<DateTimeRange>? StartTime { get; init; }
    }
}