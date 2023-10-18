using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velozient.DroneDelivery.Console.Utils
{
    public static class StringUtils
    {
        public static string TrimAndRemove(string str)
        {
            return str.Trim().Replace("[", "").Replace("]", "");
        }
    }
}
