using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;

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

        public double kd = 0.7;
        public double ks = 0.5;
        public System.Windows.Media.Color Il = Colors.White;
        public System.Windows.Media.Color Io = Colors.Red;
        public double m = 10;

        

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
