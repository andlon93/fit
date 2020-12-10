using System;

namespace FitnessTracker
{
    public static class Utilities
    {
        public static string TranslateToMonthInNorwegian(int month)
        {
            return month switch
            {
                1 => "januar",
                2 => "februar",
                3 => "mars",
                4 => "april",
                5 => "mai",
                6 => "juni",
                7 => "juli",
                8 => "august",
                9 => "september",
                10 => "oktober",
                11 => "november",
                12 => "desember",
                _ => throw new ArgumentOutOfRangeException(nameof(month), "Month must be an integer between 1 and 12."),
            };
        }
    }
}
