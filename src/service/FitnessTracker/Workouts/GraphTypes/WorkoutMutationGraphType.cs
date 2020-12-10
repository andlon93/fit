using FitnessTracker.Workouts.DTOs;
using GraphQL;
using GraphQL.Types;
using System;


namespace FitnessTracker.Workouts.GraphTypes
{
    public class WorkoutMutationGraphType : ObjectGraphType
    {
        private readonly WorkoutCommandService _workoutCommandService;

        public WorkoutMutationGraphType(WorkoutCommandService workoutCommandService)
        {
            _workoutCommandService = workoutCommandService;

            Field<WorkoutGraphType>(
                "createWorkout",
                arguments: new QueryArguments( new QueryArgument<NonNullGraphType<WorkoutInputType>> {Name = "createWorkoutRequest" }),
                resolve: context =>
                {
                    var arguments = context.GetArgument<Workout>("createWorkoutRequest");
                    var workout = new Workout {Id = Guid.NewGuid(), StartTime = arguments.StartTime, Sport = arguments.Sport, TotalTimeSeconds = arguments.TotalTimeSeconds};
                    return _workoutCommandService.CreateWorkout(workout);
                }
            );

            Field<WorkoutGraphType>(
                "updateWorkout",
                arguments: new QueryArguments( new QueryArgument<NonNullGraphType<WorkoutUpdateInputType>> {Name = "updateWorkoutRequest" }),
                resolve: context =>
                {
                    var arguments = context.GetArgument<Workout>("updateWorkoutRequest");
                    var workout = new Workout { Id = arguments.Id, StartTime = arguments.StartTime, Sport = arguments.Sport, TotalTimeSeconds = arguments.TotalTimeSeconds };
                    return _workoutCommandService.UpdateWorkout(workout);
                }
            );

            Field<WorkoutGraphType>(
                "deleteWorkout",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<WorkoutDeleteInputType>> { Name = "deleteWorkoutRequest" }),
                resolve: context =>
                {
                    var arguments = context.GetArgument<Workout>("deleteWorkoutRequest");
                    return _workoutCommandService.DeleteWorkout(new Workout { Id = arguments.Id });
                }
            );
        }
    }
}
