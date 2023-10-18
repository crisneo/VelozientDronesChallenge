using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Velozient.DroneDelivery.Console.Entities;

namespace Velozient.DroneDelivery.Console
{
    public class Scheduler
    {
        public List<DroneSchedule> GenerateSchedule(List<Drone> drones, List<Location> locations)
        {
            if (drones.Count > 100) throw new ArgumentException("Max number of 100 drones exceeded");
            var result = new List<DroneSchedule>();
            var locationsLeft = locations.ToList();
            while (drones.Count > 0 && locationsLeft.Count > 0)
            {
                result = drones.Select(drone =>
                {
                    var tripsForDrone = GetScheduledTripsForDrone(drone, result);
                    var locationsPicked = GetLocationsForCapacity(drone.MaxWeight, locationsLeft);
                    if (locationsPicked.Count > 0)
                    {
                        locationsLeft = locationsLeft.Where(location => locationsPicked.All(x => x.Name != location.Name)).ToList();
                        tripsForDrone.Add(new Trip() { Locations = locationsPicked });
                    }
                    return new DroneSchedule() { Drone = drone, Trips = tripsForDrone };
                }).ToList();
            }

            return result;
        }

        private List<Trip> GetScheduledTripsForDrone(Drone drone, List<DroneSchedule> schedule)
        {
            return schedule.Aggregate(new List<Trip>(), (List<Trip> trips, DroneSchedule droneSchedule) => droneSchedule.Drone.Name == drone.Name ? droneSchedule.Trips : trips);
        }

        private List<Location> GetLocationsForCapacity(double capacity, List<Location> locationsLeft)
        {
            var locationsPicked = new List<Location>();
            int index = 0;
            var res = locationsLeft.Aggregate(new List<Location>(), (List<Location> locationsPicked, Location currentLocation) =>
            {
                var currentLoad = CalculateLoadForLocations(locationsPicked);
                var remainingCapacity = capacity - currentLoad;
                if (currentLocation.Weight < remainingCapacity && locationsLeft.Count > 1)
                {
                    var found = GetLocationsForCapacity(remainingCapacity - currentLocation.Weight, locationsLeft.Skip(index + 1).ToList());
                    if (found.Count > 0)
                    {
                        locationsPicked.Add(currentLocation);
                        locationsPicked.AddRange(found);
                        index++;
                        return locationsPicked;
                    }
                }
                else if (currentLocation.Weight <= remainingCapacity)
                {
                    locationsPicked.Add(currentLocation);
                    index++;
                    return locationsPicked;
                }
                index++;
                return locationsPicked;

            });
            return res;
        }

        private double CalculateLoadForLocations(List<Location> locations)
        {
            return locations.Aggregate(0, (double weight, Location loc) => weight + loc.Weight);
        }

    }
}
