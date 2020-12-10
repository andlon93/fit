using FitnessTracker.Workouts.DTOs;
using GraphQL;
using GraphQL.Types;
using System;


namespace FitnessTracker.Workouts.GraphTypes
{
    public class WorkoutMutationGraphType : ObjectGraphType
    {
        private readonly WorkoutCommandService _workoutCommandService;
        private readonly Guid _userId = new Guid("ae3db2eb-c260-473a-bc57-48e65946aa2d");
        public WorkoutMutationGraphType(WorkoutCommandService workoutCommandService)
        {
            _workoutCommandService = workoutCommandService;

            Field<WorkoutGraphType>(
                "createWorkout",
                arguments: new QueryArguments( new QueryArgument<NonNullGraphType<WorkoutInputType>> {Name = "createWorkoutRequest" }),
                resolve: context =>
                {
                    var arguments = context.GetArgument<Workout>("createWorkoutRequest");
                    return _workoutCommandService.CreateWorkout(
                        new Workout { Id = Guid.NewGuid(), StartTime = arguments.StartTime, Sport = arguments.Sport, TotalTimeSeconds = arguments.TotalTimeSeconds }
                        , _userId);
                }
            );

            Field<WorkoutGraphType>(
                "updateWorkout",
                arguments: new QueryArguments( new QueryArgument<NonNullGraphType<WorkoutUpdateInputType>> {Name = "updateWorkoutRequest" }),
                resolve: context =>
                {
                    var arguments = context.GetArgument<Workout>("updateWorkoutRequest");
                    return _workoutCommandService.UpdateWorkout(new Workout { Id = arguments.Id, StartTime = arguments.StartTime, Sport = arguments.Sport, TotalTimeSeconds = arguments.TotalTimeSeconds });
                }
            );

            Field<WorkoutGraphType>(
                "deleteWorkout",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<WorkoutDeleteInputType>> { Name = "deleteWorkoutRequest" }),
                resolve: context => _workoutCommandService.DeleteWorkout(new Workout { Id = context.GetArgument<Workout>("deleteWorkoutRequest").Id }, _userId)
            );
        }
    }
}
