using FitnessTracker.Workouts.DTOs;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<WorkoutInputType>> {Name = "createWorkoutRequest" }
              ),
                resolve: context =>
                {
                    var arguments = context.GetArgument<Workout>("createWorkoutRequest");
                    var workout = new Workout {Id = Guid.NewGuid(), StartTime = arguments.StartTime, Sport = arguments.Sport, TotalTimeSeconds = arguments.TotalTimeSeconds};
                    var result = _workoutCommandService.CreateWorkout(workout);
                    return result;
                }
          );
        }
    }
}
