using FitnessTracker.Workouts.DTOs;
using GraphQL.Types;

namespace FitnessTracker.GraphQLTypes
{
    public class PositionGraphType : ObjectGraphType<Position>
    {
        public PositionGraphType()
        {
            Field<DecimalGraphType, double>()
                .Name(nameof(Position.LatitudeDegrees))
                .Description("");

            Field<DecimalGraphType, double>()
                .Name(nameof(Position.LongitudeDegrees))
                .Description("");
        }
    }
}