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
            Field<ListGraphType<GuidGraphType>, IEnumerable<Guid>>()
                .Name(nameof(Filter.Ids))
                .Description("");

            Field<ListGraphType<GuidGraphType>, IEnumerable<Guid>>()
                .Name(nameof(Filter.UserIds))
                .Description("");

            Field<ListGraphType<DateTimeRangeGraphType>, IEnumerable<DateTimeRange>>()
                .Name(nameof(Filter.StartTime))
                .Description("");
        }
    }
}