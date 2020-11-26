using FitnessTracker.Workouts.DTOs;
using System;
using System.Collections.Generic;

namespace FitnessTracker.DTO
{
    public record Filter
    {
        public IEnumerable<Guid>? Ids { get; init; }
        public IEnumerable<Guid>? UserIds { get; init; }
        public IEnumerable<DateTimeRange>? StartTime { get; init; }
    }
}