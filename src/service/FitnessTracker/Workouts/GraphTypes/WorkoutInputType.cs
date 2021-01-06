using GraphQL.Types;

namespace FitnessTracker.Workouts.GraphTypes
{
    public class WorkoutInputType : InputObjectGraphType
    {
        public WorkoutInputType()
        {
            Name = "WorkoutInput";
            Field<NonNullGraphType<DateTimeGraphType>>("startTime");
            Field<NonNullGraphType<SportTypeEnum>>("sport");
            Field<NonNullGraphType<FloatGraphType>>("totalTimeSeconds");
        }
    }
}
