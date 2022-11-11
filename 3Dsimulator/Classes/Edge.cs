using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dsimulator.Classes
{
    public class Edge
    {
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }

        public double ymax { get { return Math.Max(V1.Y, V2.Y); } }
        public double ymin { get { return Math.Min(V1.Y, V2.Y); } }
        public double xmin { get { return Math.Min(V1.X, V2.X); } }
        public double xmax { get { return Math.Max(V1.X, V2.X); } }
        
        public double xminAlgorithm
        {
            get
            {
                if (V2.Y < V1.Y) return V2.X;
                return V1.X;
            }
        }

        public double dx_dy
        { 
            get 
            { 
                return (V2.X - V1.X) / (V2.Y - V1.Y); 
            } 
        }

        public Edge(Vertex v1, Vertex v2)
        {
            V1 = v1;
            V2 = v2;
        }

    }
}
