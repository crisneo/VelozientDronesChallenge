//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//public class Optimize
//{
//    private List<string> input_data;
//    private List<DroneData> raw_drone_data;

//    public Optimize(string fileName = "test.txt")
//    {
//        input_data = RefineData(File.ReadAllLines(fileName).Select(line => line.Trim()).ToList());
//        raw_drone_data = new List<DroneData>();
//    }

//    private List<string> RefineData(List<string> data)
//    {
//        return data.Select(line => line.Split(": ")[1]).ToList();
//    }

//    public void ParseData()
//    {
//        DroneData currentDrone = null;
//        List<Location> locations = new List<Location>();

//        foreach (var data in input_data)
//        {
//            if (data.Contains("Drone"))
//            {
//                if (currentDrone != null && locations.Count > 0)
//                {
//                    raw_drone_data.Add(new DroneData
//                    {
//                        Name = currentDrone.Name,
//                        MaxWeight = currentDrone.MaxWeight,
//                        Locations = locations.OrderByDescending(loc => loc.Weight).ToList()
//                    });
//                    locations.Clear();
//                }

//                currentDrone = new DroneData();
//                var droneParts = data.Split(",");
//                currentDrone.Name = droneParts[0];
//                currentDrone.MaxWeight = double.Parse(droneParts[1]);
//            }
//            else
//            {
//                var locationParts = data.Split(",");
//                locations.Add(new Location
//                {
//                    Name = locationParts[0],
//                    Weight = double.Parse(locationParts[1])
//                });
//            }
//        }

//        if (currentDrone != null)
//        {
//            raw_drone_data.Add(new DroneData
//            {
//                Name = currentDrone.Name,
//                MaxWeight = currentDrone.MaxWeight,
//                Locations = locations.OrderByDescending(loc => loc.Weight).ToList()
//            });
//        }
//    }

//    public List<DroneData> VerifyPackages()
//    {
//        var checkedDrones = new List<DroneData>();

//        foreach (var drone in raw_drone_data)
//        {
//            var validPackages = new List<Location>();
//            var invalidPackages = new List<Location>();

//            foreach (var location in drone.Locations)
//            {
//                location.Id = drone.Locations.IndexOf(location);
//                if (location.Weight > drone.MaxWeight)
//                {
//                    invalidPackages.Add(location);
//                }
//                else
//                {
//                    validPackages.Add(location);
//                }
//            }

//            checkedDrones.Add(new DroneData
//            {
//                Name = drone.Name,
//                MaxWeight = drone.MaxWeight,
//                Locations = validPackages,
//                InvalidPackages = invalidPackages
//            });
//        }

//        return checkedDrones;
//    }

//    public void OptimizeDrones(List<DroneData> data = null, string alg = "first_bigger")
//    {
//        if (data != null)
//        {
//            raw_drone_data = data;
//        }

//        ParseData();
//        var verifiedDroneData = VerifyPackages();
//        Console.WriteLine(verifiedDroneData);

//        foreach (var drone in verifiedDroneData)
//        {
//            var trips = OptimizeTrips(drone.Locations, drone.MaxWeight, alg);
//            Console.WriteLine(drone.Name);

//            for (int i = 0; i < trips.Count; i++)
//            {
//                Console.WriteLine($"Trip {i + 1}");
//                Console.WriteLine(string.Join(", ", trips[i].Locations.Select(loc => loc.Name)));
//            }

//            if (drone.InvalidPackages != null && drone.InvalidPackages.Count > 0)
//            {
//                Console.WriteLine($"Invalid packages: {string.Join(", ", drone.InvalidPackages.Select(loc => loc.Name))}");
//            }
//        }
//    }

//    public List<Trip> OptimizeTrips(List<Location> locations, double maxWeight, string alg = "first_bigger")
//    {
//        if (alg == "first_bigger")
//        {
//            var trips = new List<Trip>();
//            var selectedLocationIds = new List<int>();
//            var locationsWithIds = new List<Location>();
//            if (locations.All(loc => loc.Id == 0))
//            {

//                for (int i = 0; i < locations.Count; i++)
//                {
//                    locations[i].Id = i;
//                    locationsWithIds.Add(locations[i]);
//                }
//            }
//            else
//            {
//                locationsWithIds = locations;
//            }

//            foreach (var loc in locationsWithIds)
//            {
//                if (!selectedLocationIds.Contains(loc.Id))
//                {
//                    var unselectedLocations = locationsWithIds.Where(l => !selectedLocationIds.Contains(l.Id)).ToList();
//                    var tripLocations = FindBestFitBigger(unselectedLocations, maxWeight);
//                    tripLocations.ForEach(tl => selectedLocationIds.Add(tl.Id));
//                    trips.Add(new Trip { Locations = tripLocations, Rest = (maxWeight - tripLocations.Select(tl => tl.Weight).Sum()).Round(1) });
//                }
//            }

//            return trips.OrderBy(t => t.Rest).ToList();
//        }
//        else if (alg == "all_combinations")
//        {
//            var combs = CreateAllCombinations(locations, maxWeight);
//            var selectedCombs = new List<TripCombination>();

//            foreach (var c in combs)
//            {
//                var alreadyPresented = false;
//                foreach (var t in c.Combinations)
//                {
//                    if (selectedCombs.Select(sc => sc.Locations.Select(l => l.Name)).SelectMany(names => names).Contains(t.Name))
//                    {
//                        alreadyPresented = true;
//                    }
//                }

//                if (!alreadyPresented)
//                {
//                    selectedCombs.Add(new TripCombination { Locations = c.Combinations, Rest = (maxWeight - c.Combinations.Select(tl => tl.Weight).Sum()).Round(1) });
//                }
//            }

//            return selectedCombs.OrderBy(t => t.Rest).ToList();
//        }

//        return null;
//    }

//    public List<Location> FindBestFitBigger(List<Location> locations, double limit)
//    {
//        var selectedPackages = new List<Location>();

//        while (locations.Min(l => l.Weight) <= limit)
//        {
//            foreach (var location in locations)
//            {
//                if (location.Weight <= limit && !selectedPackages.Select(sp => sp.Id).Contains(location.Id))
//                {
//                    selectedPackages.Add(location);
//                    limit -= location.Weight;
//                }
//            }
//        }

//        return selectedPackages;
//    }

//    public List<TripCombination> CreateAllCombinations(List<Location> locations, double limit)
//    {
//        var combinations = new List<TripCombination>();

//        for (int s = 0; s <= locations.Count; s++)
//        {
//            foreach (var loc in locations.Combinations(s))
//            {
//                if (loc.Select(l => l.Weight).Sum() <= limit && loc.Count > 0)
//                {
//                    combinations.Add(new TripCombination { Rest = (limit - loc.Select(l => l.Weight).Sum()).Round(1), Combinations = loc.ToList() });
//                }
//            }
//        }

//        return combinations.OrderBy(t => t.Rest).ToList();
//    }
//}

//public class DroneData
//{
//    public string Name { get; set; }
//    public double MaxWeight { get; set; }
//    public List<Location> Locations { get; set; }
//    public List<Location> InvalidPackages { get; set; }
//}

//public class Location
//{
//    public string Name { get; set; }
//    public double Weight { get; set; }
//    public int Id { get; set; }
//}

//public class Trip
//{
//    public List<Location> Locations { get; set; }
//    public double Rest { get; set; }
//}

//public class TripCombination
//{
//    public List<Location> Combinations { get; set; }
//    public double Rest { get; set; }
//}