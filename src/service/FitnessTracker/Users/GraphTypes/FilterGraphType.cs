using GraphQL.Types;
using System;
using System.Collections.Generic;
using Filter = FitnessTracker.Users.DTOs.Filter;

namespace FitnessTracker.Users.GraphTypes
{
    public class FilterGraphType : InputObjectGraphType<Filter>
    {
        public FilterGraphType()
        {
            Name = "UserFilter";
            Field<ListGraphType<GuidGraphType>, IEnumerable<Guid>>()
                .Name(nameof(Filter.Ids))
                .Description("User ids");
        }
    }
}
