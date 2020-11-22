using FitnessTracker.TCX;
using System.Collections.Generic;
using System.IO;

namespace FitnessTracker.Database
{
    public static class Database
    {
        private const string path = "/app/Data/";
        private const string filename = "endomondo-2020-11-14.zip";
        private static IEnumerable<TrainingCenterDatabase_t>? workoutDatabase;

        public static IEnumerable<TrainingCenterDatabase_t> FindAll()
        {
            if (workoutDatabase == null)
            {
                Unzip(path, filename);
                workoutDatabase = TCXReader.ReadWorkouts(Directory.EnumerateFiles($"Data/{Path.GetFileNameWithoutExtension(filename)}/Workouts", "*.tcx"));
            }

            return workoutDatabase;
        }

        private static void Unzip(string path, string filename)
        {
            if (Directory.Exists(Path.Join(path, Path.GetFileNameWithoutExtension(filename))))
            {
                return;
            }

            System.IO.Compression.ZipFile.ExtractToDirectory(Path.Join(path, filename), path);
        }
    }
}
