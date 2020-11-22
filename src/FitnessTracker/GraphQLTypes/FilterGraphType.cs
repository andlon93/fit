using FitnessTracker.DTO;
using GraphQL.Types;
using System.Collections.Generic;

namespace FitnessTracker.GraphQLTypes
{
    internal class FilterGraphType : InputObjectGraphType<Filter>
    {
        public FilterGraphType()
        {
            Field<ListGraphType<StringGraphType>, IEnumerable<string>>()
                .Name(nameof(Filter.Id))
                .Description("");
        }
    }
}