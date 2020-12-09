using System;
using System.Collections.Generic;

namespace FitnessTracker.Challenges
{
    public record Challenge
    {
        public string? Name { get; init; }
        public ChallengeType? Type { get; init; }
        public DateTime? StartTime { get; init; }
        public DateTime? EndTime { get; init; }
        //public IEnumerable<Competitor>? LeaderBoard { get; set; }
        public IEnumerable<Guid>? UserIds { get; init; } 
    }
}