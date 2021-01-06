using FitnessTracker.Users;
using FitnessTracker.Workouts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Workouts
{
    public class WorkoutCommandService
    {
        private readonly WorkoutRepository _workoutRepository;
        private readonly UserRepository _userRepository;

        public WorkoutCommandService(WorkoutRepository workoutRepository, UserRepository userRepository)
        {
            _workoutRepository = workoutRepository;
            _userRepository = userRepository;
        }

        public Workout CreateWorkout(Workout workout, Guid userId)
        {
            var savedWorkout = _workoutRepository.SaveOrUpdateWorkouts(new List<Workout> { workout }).First();
            _userRepository.AddWorkoutToUser(userId, workout.Id);
            return savedWorkout;
        }

        public Workout UpdateWorkout(Workout workout, Guid userId)
        {
            IsWorkoutConnectedToUser(userId, workout.Id, "update");
            return _workoutRepository.SaveOrUpdateWorkouts(new List<Workout> { workout }).First();
        } 

        public Workout? DeleteWorkout(Workout workout, Guid userId)
        {
            IsWorkoutConnectedToUser(userId, workout.Id, "delete");
            if (_workoutRepository.DeleteWorkouts(new List<Workout> { workout }) == 1)
            {
                _userRepository.RemoveWorkoutFromUser(userId, workout.Id);
                return workout;
            }

            return null;
        }
        public IEnumerable<Guid> SaveWorkoutsFromZipFile(IEnumerable<TrainingCenterDatabase_t> workouts)
        {
            var mappedWorkouts = WorkoutMapper.MapTcxToWorkouts(workouts);

            _workoutRepository.SaveOrUpdateWorkouts(mappedWorkouts);

            return mappedWorkouts.Select(w => w.Id);
        }

        private void IsWorkoutConnectedToUser(Guid userId, Guid workoutId, string action)
        {
            if (!_userRepository.GetById(userId).WorkoutIds.Contains(workoutId))
            {
                throw new Exception($"The workout is not connected to the user which is trying to {action} it.");
            }
        } 
    }
}
