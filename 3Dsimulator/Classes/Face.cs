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

        public NormalVector normalVector;

        public Face(NormalVector nv)
        {
            normalVector = nv;
        }

        public void AddVertex(Vertex v)
        {
            vertices.Add(v);
        }

    }
}
