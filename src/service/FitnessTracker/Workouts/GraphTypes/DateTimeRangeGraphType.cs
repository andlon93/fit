using FitnessTracker.Workouts.DTOs;
using GraphQL.Types;
using System;

namespace FitnessTracker.Workouts.GraphTypes
{
    public class DateTimeRangeGraphType : InputObjectGraphType<DateTimeRange>
    {
        public DateTimeRangeGraphType()
        {
            Field<DateTimeGraphType, DateTime>()
                .Name(nameof(DateTimeRange.Start))
                .Description("");

            Field<DateTimeGraphType, DateTime>()
                .Name(nameof(DateTimeRange.End))
                .Description("");
        }
    }
}