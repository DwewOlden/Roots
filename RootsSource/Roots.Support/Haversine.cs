using Roots.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Support
{
    public class Haversine : IHaversine
    {
        /// <summary>
        /// Calculate the distance between two geographic points
        /// </summary>
        /// <param name="Latitude1">The Latitude of the first location</param>
        /// <param name="Longtitude1">The Longtitude of the first location</param>
        /// <param name="Latitude2">The Latitude of the second location</param>
        /// <param name="Longtitude2">The Longtitude of the second location</param>
        /// <returns></returns>
        public double CalculateHaversineDistance(double Latitude1, double Longtitude1, double Latitude2, double Longtitude2)
        {
            double dDistance = Double.MinValue;
            double dLat1InRad = Latitude1 * (Math.PI / 180.0);
            double dLong1InRad = Longtitude1 * (Math.PI / 180.0);
            double dLat2InRad = Latitude2 * (Math.PI / 180.0);
            double dLong2InRad = Longtitude2 * (Math.PI / 180.0);

            double dLongitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                       Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                       Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Asin(Math.Sqrt(a));

            // Distance.
            // const Double kEarthRadiusMiles = 3956.0;
            const Double kEarthRadiusKms = 6376.5;
            dDistance = kEarthRadiusKms * c;

            return dDistance;
        }
    }
}
