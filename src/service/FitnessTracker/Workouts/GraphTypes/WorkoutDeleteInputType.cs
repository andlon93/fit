using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Workouts.GraphTypes
{
    public class WorkoutDeleteInputType : InputObjectGraphType
    {
        public WorkoutDeleteInputType()
        {
            Name = "WorkoutDeleteInput";
            Field<NonNullGraphType<StringGraphType>>("id");
        }
    }
}
