using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velozient.DroneDelivery.Console.Utils
{
    public static class PathUtils
    {
        public static string GetExecutingAssemblyFolder()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        }
    }
}
