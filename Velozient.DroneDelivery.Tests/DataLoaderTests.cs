using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Velozient.DroneDelivery.Console;
using Velozient.DroneDelivery.Console.Utils;

namespace Velozient.DroneDelivery.Tests
{
    public class DataLoaderTests
    {
        private DataLoader _dataLoader;

        [SetUp]
        public void Setup()
        {
            string dir = PathUtils.GetExecutingAssemblyFolder();
            _dataLoader = new DataLoader(dir + "\\testData.csv");
        }

        [Test]
        public void LoadDronesTest()
        {
            var drones = _dataLoader.LoadDrones();
            Assert.IsNotNull(drones);
            Assert.AreEqual(3, drones.Count);
        }

        [Test]
        public void LoadLocationsTest()
        {
            var locations = _dataLoader.LoadLocations();
            Assert.IsNotNull(locations);
            Assert.AreEqual(16, locations.Count);
        }

        [Test]
        public void InvalidFileReturnsEmpty()
        {
            _dataLoader = new DataLoader("x:/path/to/nothing");
            var drones = _dataLoader.LoadDrones();
            var locations = _dataLoader.LoadLocations();

            Assert.IsNotNull(drones);
            Assert.IsNotNull(locations);
            Assert.IsEmpty(drones);
            Assert.IsEmpty(locations);
        }
    }
}
