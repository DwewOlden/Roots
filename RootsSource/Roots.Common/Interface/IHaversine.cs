using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Common.Interface
{
    public interface IHaversine
    {
        double CalculateHaversineDistance(double Latitude1, double Longtitude1, double Latitude2, double Longtitude2);
       
    }
}
