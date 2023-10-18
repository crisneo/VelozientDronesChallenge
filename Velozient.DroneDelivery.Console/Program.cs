// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Velozient.DroneDelivery.Console;
using Velozient.DroneDelivery.Console.Utils;

var dir = PathUtils.GetExecutingAssemblyFolder();
var dataLoader = new DataLoader(dir + "\\Data\\data.csv");
var drones = dataLoader.LoadDrones();
var locations = dataLoader.LoadLocations();
var scheduler = new Scheduler();
var res = scheduler.GenerateSchedule(drones, locations);
var droneIndex = 0;
res.ForEach(schedule =>
{
    Console.WriteLine($"[Dron: {schedule.Drone.Name}]");
    var tripIndex = 0;
    schedule.Trips.ForEach(trip =>
    {
        if (trip.Locations.Count > 0)
        {
            Console.WriteLine($"Trip #{tripIndex + 1}");
            trip.Locations.ForEach(loc =>
            {
                Console.WriteLine($"[{loc.Name}], ");
            });
            tripIndex++;
        }
    });
    Console.WriteLine(" ");
    droneIndex++;
});
Console.ReadKey();
