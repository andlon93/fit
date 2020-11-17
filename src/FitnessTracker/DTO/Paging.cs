namespace FitnessTracker.DTO
{
    public record Paging
    {
        public int Rows { get; init; }
        public int Offset { get; init; }
    }
}
