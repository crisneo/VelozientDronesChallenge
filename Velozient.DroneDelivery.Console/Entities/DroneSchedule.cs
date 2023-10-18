using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velozient.DroneDelivery.Console.Entities
{
    public class DroneSchedule
    {
        public Drone Drone { get; set; }
        public List<Trip> Trips { get; set; }
    }

    public class Trip
    {
        public List<Location> Locations { get; set; }
    }

}
