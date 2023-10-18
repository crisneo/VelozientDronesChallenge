using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velozient.DroneDelivery.Console.Entities;
using Velozient.DroneDelivery.Console.Utils;

namespace Velozient.DroneDelivery.Console
{
    public class DataLoader
    {
        private string _csvFileName = string.Empty;
        public DataLoader(string csvFileName)
        {
            _csvFileName = csvFileName;
        }

        private List<string> ReadFileLines(string csvFileName)
        {
            try
            {
                return File.ReadLines(csvFileName).Select(x => x.Trim()).ToList();
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        public List<Drone> LoadDrones()
        {
            var drones = new List<Drone>();
            var lines = ReadFileLines(_csvFileName);
            if (lines.Count > 0)
            {
                var dronesString = lines[0].Split(",");
                for (int i = 0; i < dronesString.Count(); i += 2)
                {
                    var drone = new Drone() { Name = StringUtils.TrimAndRemove(dronesString[i]), MaxWeight = Convert.ToDouble(StringUtils.TrimAndRemove(dronesString[i + 1])) };
                    drones.Add(drone);
                }
            }
            return drones;
        }

        public List<Location> LoadLocations()
        {
            var locations = new List<Location>();
            var lines = ReadFileLines(_csvFileName);
            if (lines.Count > 1)
            {
                for (int lineNumber = 1; lineNumber < lines.Count(); lineNumber++)
                {
                    var locationString = lines[lineNumber].Split(",");
                    var location = new Location() { Name = StringUtils.TrimAndRemove(locationString[0]), Weight = Convert.ToDouble(StringUtils.TrimAndRemove(locationString[1])) };
                    locations.Add(location);
                }
            }
            return locations;
        }
    }
}
