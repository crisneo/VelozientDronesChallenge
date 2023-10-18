using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velozient.DroneDelivery.Console.Utils;
using Velozient.DroneDelivery.Console;

namespace Velozient.DroneDelivery.Tests
{
    public class UtilsTests
    {
        [Test]
        public void StringRemoveAndTrimTest()
        {
            var droneName = "[Drone1]";
            var droneName2 = "Drone1";

            var removedResult = StringUtils.TrimAndRemove(droneName);
            var notUpdatedResult = StringUtils.TrimAndRemove(droneName2);
            Assert.AreEqual("Drone1", removedResult);
            Assert.AreEqual("Drone1", notUpdatedResult);
        }

        [Test]
        public void PathUtilsGetExecutingFolderTest()
        {
            var currentPath = PathUtils.GetExecutingAssemblyFolder();
            Assert.IsTrue(currentPath.Contains("Velozient.DroneDelivery.Tests"));
        }

    }
}
