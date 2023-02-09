using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dsimulator.Classes
{
    public class Vertex
    {
        public double X;
        public double Y;
        public double Z;

        public float fogValue = 0;

        public NormalVector normalVector = new NormalVector(0,0,0);

        public Vertex(double x, double y, double z)
        {
            X = Geo.x2w(x); Y = Geo.y2h(y); Z = Geo.z2p(z);
        }

        public void AddNormalVector(NormalVector nv)
        {
            normalVector = nv;
        }

        public static NormalVector operator-(Vertex v1, Vertex v2)
        {
            return new(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }


    }
}
