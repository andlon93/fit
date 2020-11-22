using FitnessTracker.DTO;
using GraphQL.Types;

namespace FitnessTracker.GraphQLTypes
{
    public class PagingGraphType : InputObjectGraphType<Paging>
    {
        public PagingGraphType()
        {
            Field<IntGraphType>()
                .Name(nameof(Paging.Rows));
            Field<IntGraphType>()
                .Name(nameof(Paging.Offset));
        }
    }
}
