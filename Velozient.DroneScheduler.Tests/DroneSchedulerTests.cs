using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Velozient.DroneDelivery.Console;
using Velozient.DroneDelivery.Console.Entities;

namespace Velozient.DroneDelivery.Tests
{
    public class DroneSchedulerTests
    {
        private Scheduler _scheduler;
        private List<Drone> _drones;
        private List<Location> _locations;

        [SetUp]
        public void Setup()
        {
            _scheduler = new Scheduler();
            _drones = new List<Drone>();
            _locations = new List<Location>();
            _drones.Add(new Drone() { Name = "Smart25", MaxWeight = 30 });
            _drones.Add(new Drone() { Name = "TheBoss", MaxWeight = 50 });

            _locations.Add(new Location() { Name = "Madrid", Weight = 10 });
            _locations.Add(new Location() { Name = "Barcelona", Weight = 5 });
            _locations.Add(new Location() { Name = "Sevilla", Weight = 10 });
            _locations.Add(new Location() { Name = "Valencia", Weight = 20 });
            _locations.Add(new Location() { Name = "Zaragoza", Weight = 15 });
            _locations.Add(new Location() { Name = "Salamanca", Weight = 10 });
            _locations.Add(new Location() { Name = "Toledo", Weight = 25 });
            _locations.Add(new Location() { Name = "Murcia", Weight = 5 });
            _locations.Add(new Location() { Name = "Valladolid", Weight = 10 });
            _locations.Add(new Location() { Name = "La Rioja", Weight = 20 });
        }

        [Test]
        public void TestScheduling()
        {
            var schedule = _scheduler.GenerateSchedule(_drones, _locations);
            var totalNumberOfTrips = CalculateTotalTrips(schedule);
            Assert.AreEqual(3, totalNumberOfTrips);

            var smart25 = schedule[0];
            Assert.AreEqual(1, smart25.Trips.Count);


            // schedule for Smart25 drone
            var expectedRoute = new List<Location>() { _locations[0], _locations[1], _locations[2], _locations[7] };
            var smart25Locations = new List<Location>();
            smart25.Trips.ForEach(trip =>
            {
                trip.Locations.ForEach(loc =>
                {
                    smart25Locations.Add(loc);
                });
            });

            Assert.IsTrue(smart25Locations.All(x => ContainsLocation(expectedRoute, x)));

            //schedule for TheBoss drone
            var theBoss = schedule[1];
            Assert.AreEqual(2, theBoss.Trips.Count);

            var expectedRouteForBossDrone = new List<Location>() { _locations[3], _locations[5], _locations[9], _locations[4], _locations[6], _locations[8] };
            var theBossLocations = new List<Location>();
            theBoss.Trips.ForEach(trip =>
            {
                trip.Locations.ForEach(loc =>
                {
                    theBossLocations.Add(loc);
                });
            });
            Assert.IsTrue(theBossLocations.All(x => ContainsLocation(expectedRouteForBossDrone, x)));

        }

        [Test]
        public void TestEmptyDrones()
        {
            var schedule = _scheduler.GenerateSchedule(new List<Drone>(), _locations);
            Assert.IsEmpty(schedule);
        }

        [Test]
        public void TestEmptyLocations()
        {
            var schedule = _scheduler.GenerateSchedule(_drones, new List<Location>());
            Assert.IsEmpty(schedule);
        }

        [Test]
        public void ExceededDronesTest()
        {
            var tDrones = new List<Drone>();
            for (int i = 0; i < 1500; i++)
            {
                tDrones.Add(new Drone());
            }
            Assert.Throws<ArgumentException>(() => _scheduler.GenerateSchedule(tDrones, new List<Location>()));
        }

        private double CalculateTotalTrips(List<DroneSchedule> schedule)
        {
            return schedule.Aggregate(0, (double numberOfTrips, DroneSchedule droneSchedule) => numberOfTrips + droneSchedule.Trips.Count);
        }

        private bool ContainsLocation(List<Location> locations, Location location)
        {
            var names = locations.Select(x => x.Name);
            return names.Contains(location.Name);
        }
    }
}
