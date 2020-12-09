namespace FitnessTracker.Users.DTOs
{
    public record SiteConnection
    {
        public SiteType Site { get; init; } = SiteType.Unknown;
        public string? Identifier { get; init; }
    }
}
