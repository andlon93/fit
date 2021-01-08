using FitnessTracker.Authentication;
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
                    var userId = GetUserId(context, "create");

                    return _workoutCommandService.CreateWorkout(
                        new Workout { Id = Guid.NewGuid(), StartTime = arguments.StartTime, Sport = arguments.Sport, TotalTimeSeconds = arguments.TotalTimeSeconds }
                        , userId);
                }
            );

            Field<WorkoutGraphType>(
                "updateWorkout",
                arguments: new QueryArguments( new QueryArgument<NonNullGraphType<WorkoutUpdateInputType>> {Name = "updateWorkoutRequest" }),
                resolve: context =>
                {
                    var arguments = context.GetArgument<Workout>("updateWorkoutRequest");
                    var userId = GetUserId(context, "update");

                    return _workoutCommandService.UpdateWorkout(new Workout { Id = arguments.Id, StartTime = arguments.StartTime, Sport = arguments.Sport, TotalTimeSeconds = arguments.TotalTimeSeconds }, userId);
                }
            );

            Field<WorkoutGraphType>(
                "deleteWorkout",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<WorkoutDeleteInputType>> { Name = "deleteWorkoutRequest" }),
                resolve: context => _workoutCommandService.DeleteWorkout(new Workout { Id = context.GetArgument<Workout>("deleteWorkoutRequest").Id }, GetUserId(context, "delete"))
            );
        }

        private Guid GetUserId(IResolveFieldContext<object> context, string action)
        {
            var exists = context.UserContext.TryGetValue(AuthorizationConstants.AuthorIdContextTitle, out var userId);
            var userIdString = userId?.ToString();

            if (!exists || userIdString == null) 
            { 
                throw new Exception($"Can not {action} workout because there was no userId in the context."); 
            }

            return new Guid(userIdString);
        }
    }
}
