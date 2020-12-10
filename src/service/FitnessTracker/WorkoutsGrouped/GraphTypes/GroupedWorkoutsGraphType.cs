using FitnessTracker.WorkoutsGrouped.DTOs;
using GraphQL.Types;

namespace FitnessTracker.WorkoutsGrouped.GraphTypes
{
    public class GroupedWorkoutsGraphType : ObjectGraphType<GroupedWorkout>
    {
        public GroupedWorkoutsGraphType()
        {
            Field<StringGraphType>().Name(nameof(GroupedWorkout.Title)).Description("Title");
            Field<FloatGraphType>().Name(nameof(GroupedWorkout.DurationInSeconds)).Description("DurationInSeconds");
            Field<FloatGraphType>().Name(nameof(GroupedWorkout.DistanceInMeters)).Description("DistanceInMeters");
            Field<LongGraphType>().Name(nameof(GroupedWorkout.Calories)).Description("Calories");
            Field<IntGraphType>().Name(nameof(GroupedWorkout.NumberOfWorkouts)).Description("Total number of workouts within group");
        }
    }
}
