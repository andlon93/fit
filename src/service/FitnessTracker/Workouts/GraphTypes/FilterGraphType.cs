using FitnessTracker.DTO;
using FitnessTracker.Workouts.DTOs;
using GraphQL.Types;
using System;
using System.Collections.Generic;

namespace FitnessTracker.Workouts.GraphTypes
{
    internal class FilterGraphType : InputObjectGraphType<Filter>
    {
        public FilterGraphType()
        {
            Name = "WorkoutFilter";
            Field<ListGraphType<GuidGraphType>, IEnumerable<Guid>>()
                .Name(nameof(Filter.Ids))
                .Description("Workout ids");

            Field<ListGraphType<DateTimeRangeGraphType>, IEnumerable<DateTimeRange>>()
                .Name(nameof(Filter.StartTime))
                .Description("");
        }
    }
}