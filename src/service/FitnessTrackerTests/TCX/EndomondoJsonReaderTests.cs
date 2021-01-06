using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace FitnessTracker.TCX.Tests
{
    [TestClass]
    public class EndomondoJsonReaderTests
    {
        [TestMethod]
        public void ReadTrainingCenterDatabaseFromFileTest()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            var test = thisAssembly.GetManifestResourceNames();

            //string path = "My.Tool.Namespace";

            //return new StreamReader(thisAssembly.GetManifestResourceStream(path + "." + filename));

            var workout = EndomondoJsonReader.ReadTrainingCenterDatabaseFromFile(@"C:\Users\ola\git\fit2\src\service\FitnessTrackerTests\TCX\2020-09-19_11-52-42.0.json");

            Assert.AreEqual("RUNNING", workout.Sport);
            Assert.AreEqual("IMPORT_POLAR", workout.Source);
            Assert.AreEqual(new DateTime(2020, 9, 19, 11, 52, 42, 0), workout.CreatedDate);
            Assert.AreEqual(new DateTime(2020, 9, 19, 8, 26, 16, 0), workout.StartTime);
            Assert.AreEqual(new DateTime(2020, 9, 19, 9, 24, 36, 0), workout.EndTime);
            Assert.AreEqual(3500, workout.DurationS);
            Assert.AreEqual(10.013, workout.DistanceKm);
            Assert.AreEqual(574, workout.CaloriesKcal);
            Assert.AreEqual(102.719, workout.AltitudeMinM);
            Assert.AreEqual(146.763, workout.AltitudeMaxM);
            Assert.AreEqual(186, workout.HeartRateAvgBpm);
            Assert.AreEqual(196, workout.HeartRateMaxBpm);
            Assert.AreEqual(87, workout.CadenceAvgRpm);
            Assert.AreEqual(10.299085714285715, workout.SpeedAvgKmh);
            Assert.AreEqual(14.4, workout.SpeedMaxKmh);
            Assert.AreEqual(60.503, workout.AscendM);
            Assert.AreEqual(50.292, workout.DescendM);

            var firstTrackPoint = workout.Points.First();
            Assert.AreEqual(59.88717833, firstTrackPoint.Location.Latitude);
            Assert.AreEqual(10.82581167, firstTrackPoint.Location.Longitude);
            Assert.AreEqual(103.024, firstTrackPoint.Altitude);
            Assert.AreEqual(159, firstTrackPoint.HeartRateBpm);
            Assert.AreEqual(0.0002, firstTrackPoint.DistanceKm);
            Assert.AreEqual(new DateTime(2020, 9, 19, 8, 26, 17), firstTrackPoint.Timestamp);

            var lastTrackPoint = workout.Points.Last();
            Assert.AreEqual(59.883335, lastTrackPoint.Location.Latitude);
            Assert.AreEqual(10.833985, lastTrackPoint.Location.Longitude);
            Assert.AreEqual(107.748, lastTrackPoint.Altitude);
            Assert.AreEqual(195, lastTrackPoint.HeartRateBpm);
            Assert.AreEqual(10.0069, lastTrackPoint.DistanceKm);
            Assert.AreEqual(new DateTime(2020, 9, 19, 9, 24, 36), lastTrackPoint.Timestamp);
        }
    }
}