using FitnessTracker.DTO;
using GraphQL.Types;

namespace FitnessTracker.GraphQLTypes
{
    public class PagingType : InputObjectGraphType<Paging>
    {
        public PagingType()
        {
            Field<IntGraphType>().Name(nameof(Paging.Rows));
            Field<IntGraphType>().Name(nameof(Paging.Offset));
        }
    }
}
