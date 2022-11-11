using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dsimulator.Classes
{
    public class Face
    {
        private List<Vertex> vertices = new List<Vertex>();
        public List<Vertex> Vertices { get { return vertices; } set { vertices = value; } }

        public List<Edge> Edges { get; set; } = new List<Edge>();

        public double ymin = double.MaxValue;
        public double ymax = double.MinValue;

        public void AddVertex(Vertex v)
        {
            vertices.Add(v);
            if(v.Y < ymin) ymin = v.Y;
            if(v.Y > ymax) ymax = v.Y;
        }

        public void CreateEdges()
        {
            for(int i = 0; i< Vertices.Count; i++)
            {
                Edges.Add(new Edge(vertices[i], vertices[(i+1)%vertices.Count]));
            }
        }

        public void draw()
        {

        }

    }
}
