using FitnessTracker.DTO;
using GraphQL.Types;

namespace FitnessTracker.GraphQLTypes
{
    public static class PagingArgument
    {
        private const string _pagingArgumentName = "paging";

        private static readonly QueryArgument _pagingArgument = new QueryArgument<PagingGraphType>
        {
            Name = _pagingArgumentName,
            DefaultValue = new Paging { Rows = 10, Offset = 0 }
        };
        public static string GetArgumentName() => _pagingArgumentName;
        public static QueryArgument GetArgument() => _pagingArgument;
    }
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
