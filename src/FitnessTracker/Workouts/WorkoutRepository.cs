﻿using FitnessTracker.Workouts.DTOs;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FitnessTracker.Workouts
{
    public class WorkoutRepository
    {
        private Dictionary<Guid, Workout> _workouts { get; set; }
        public WorkoutRepository()
        {
            _workouts = new Dictionary<Guid, Workout>();
        }

        public IEnumerable<Workout> GetAll() => _workouts.Values;

        public void SaveWorkouts(IEnumerable<Workout> workouts)
        {
            if (!_workouts.Any()) { _workouts = workouts.ToDictionary(w => Guid.NewGuid()); }

            foreach(var workout in workouts)
            {
                if (_workouts.ContainsKey(workout.Id))
                {                    
                    _workouts[workout.Id] = workout;
                    continue;
                }
                _workouts.Add(workout.Id, workout);
            }
        }
    }
}