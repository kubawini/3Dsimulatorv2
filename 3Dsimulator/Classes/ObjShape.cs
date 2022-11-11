using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dsimulator.Classes
{
    public class ObjShape
    {
        private List<Vertex> vertices = new List<Vertex>();
        public List<Vertex> Vertices { get { return vertices; } set { vertices = value; } }

        private List<NormalVector> normalVectors = new List<NormalVector>();
        public List<NormalVector> NormalVectors { get { return normalVectors; } set { normalVectors = value; } }

        private List<Face> faces = new List<Face>();
        public List<Face> Faces { get { return faces; } set { faces = value; } }

        

        public void AddVertex(Vertex v)
        {
            vertices.Add(v);
        }

        public void AddNormalVector(NormalVector vn)
        {
            normalVectors.Add(vn);
        }

        public void AddFace(Face f)
        {
            faces.Add(f);
        }

        

    }
}
