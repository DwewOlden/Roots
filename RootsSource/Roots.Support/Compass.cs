using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Support
{
    public class Compass
    {
        public static string GetBearing(double Bearing)
        {
            if (Bearing >= 0 && Bearing <= 22.5)
                return "NORTH";
            else if (Bearing > 22.5 && Bearing <= 67.5)
                return "NORTH EAST";
            else if (Bearing > 67.5 && Bearing <= 112.5)
                return "EAST";
            else if (Bearing > 112.5 && Bearing <= 157.5)
                return "SOUTH EAST";
            else if (Bearing > 157.5 && Bearing <= 202.5)
                return "SOUTH";
            else if (Bearing > 202.5 && Bearing <= 247.5)
                return "SOUTH WEST";
            else if (Bearing > 247.5 && Bearing <= 292.5)
                return "WEST";
            else if (Bearing > 292.5 && Bearing <= 337.5)
                return "NORTH WEST";
            else
                return "NORTH";




            if (Bearing >= 0 && Bearing <= 30)
                return "NORTH";
            else if (Bearing > 30 && Bearing <= 90)
                return "NORTH EAST";



//            So 0 is north (and the degrees goes clockwise)
//So 90 would be East
//180 South
//270 West
        }
    }
}
