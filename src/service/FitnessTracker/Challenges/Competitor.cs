using FitnessTracker.Users.DTOs;

namespace FitnessTracker.Challenges
{
    public record Competitor
    {
        public User? User { get; init; }
        public int? Rank { get; set; }
        public int? Score { get; init; }
    }
}