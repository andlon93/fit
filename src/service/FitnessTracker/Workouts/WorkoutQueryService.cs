using FitnessTracker.DTO;
using FitnessTracker.Workouts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.Workouts
{
    public class WorkoutQueryService
    {
        private readonly WorkoutRepository _workoutRepository;

        public WorkoutQueryService(WorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public IEnumerable<Workout> GetWorkouts(Paging paging, Filter filter)
        {
            return _workoutRepository.GetAll()
                                     .Where(w => (filter?.Ids?.Any() != true || filter.Ids.Contains(w.Id))
                                                 && (filter?.StartTime?.Any() != true || filter.StartTime.Any(x => (x.Start == null || w.StartTime >= x.Start) && (x.End == null || w.StartTime <= x.End))))
                                     .OrderByDescending(w => w.StartTime)
                                     .Skip(paging.Offset)
                                     .Take(paging.Rows);
        }        
    }
}
