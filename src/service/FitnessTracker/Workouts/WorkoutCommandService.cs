using FitnessTracker.Workouts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Workouts
{
    public class WorkoutCommandService
    {
        private readonly WorkoutRepository _workoutRepository;
        public WorkoutCommandService(WorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }
        public Workout CreateWorkout(Workout workout) => _workoutRepository.SaveOrUpdateWorkouts(new List<Workout> { workout }).First();
    }
}
