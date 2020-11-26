using FitnessTracker.Users.DTOs;
using FitnessTracker.Users.GraphTypes;
using GraphQL.Types;

namespace FitnessTracker.Challenges
{
    public class CompetitorGraphType : ObjectGraphType<Competitor>
    {
        public CompetitorGraphType()
        {
            Field<UserType, User>()
                .Name(nameof(Competitor.User))
                .Description("");

            Field<IntGraphType, int>()
                .Name(nameof(Competitor.Rank))
                .Description("");

            Field<IntGraphType, int>()
                .Name(nameof(Competitor.Score))
                .Description("");
        }
    }
}
