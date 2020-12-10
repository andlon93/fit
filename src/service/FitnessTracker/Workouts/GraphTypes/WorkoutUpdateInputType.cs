using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Workouts.GraphTypes
{
    public class WorkoutUpdateInputType : WorkoutInputType
    {
        public WorkoutUpdateInputType()
        {
            Name = "WorkoutUpdateInput";
            Field<NonNullGraphType<StringGraphType>>("id");
        }
    }
}
