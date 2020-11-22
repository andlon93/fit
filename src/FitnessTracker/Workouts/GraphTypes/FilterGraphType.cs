using FitnessTracker.DTO;
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
        }
    }
}