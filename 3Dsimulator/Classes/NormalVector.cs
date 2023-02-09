using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dsimulator.Classes
{
    public class NormalVector
    {
        public double X;
        public double Y;
        public double Z;

        public NormalVector(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }

        public static double operator*(NormalVector v1, NormalVector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public static NormalVector operator/(NormalVector v, double d)
        {
            return new(v.X / d, v.Y / d, v.Z / d);
        }
    }
}
