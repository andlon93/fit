using FitnessTracker.TCX;
using FitnessTracker.Users;
using FitnessTracker.Workouts;
using System.IO;
using System.Text.Json;

namespace FitnessTracker
{
    public class EndomondeZipFileReader
    {
        private const string _pathToZipFiles = "/app/Data/";
        private const string _filename = "endomondo-2020-11-14.zip";
        private readonly WorkoutCommandService _workoutService;
        private readonly UserService _userService;

        public EndomondeZipFileReader(WorkoutCommandService workoutService, UserService userService)
        {
            _workoutService = workoutService;
            _userService = userService;
        }        

        // TDOD: change to support n zip files.
        public void ReadZipFile()
        {
            TryUnzip(_filename);

            var workouts = TCXReader.ReadWorkouts(Directory.EnumerateFiles($"{_pathToZipFiles}{Path.GetFileNameWithoutExtension(_filename)}/Workouts", "*.tcx"));
            var workoutIds = _workoutService.SaveWorkoutsFromZipFile(workouts);

            var user = GetUserEntityFromFile();
            if (user != null) 
            {
                user.Workouts = workoutIds;
                _userService.SaveOrUpdateUser(user);
            }           
        }

        private UserEntity? GetUserEntityFromFile() =>
            JsonSerializer.Deserialize<UserEntity>(File.ReadAllText($"Data/{Path.GetFileNameWithoutExtension(_filename)}/Profile/Profile.json"));

        private void TryUnzip(string filename)
        {
            if (!Directory.Exists(Path.Join(_pathToZipFiles, Path.GetFileNameWithoutExtension(filename))))
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(Path.Join(_pathToZipFiles, filename), _pathToZipFiles);
            }            
        }
    }
}
