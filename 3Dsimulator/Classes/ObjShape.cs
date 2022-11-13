using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        private double _kd = 0.7;
        private double _ks = 0.5;
        private System.Windows.Media.Color _Il = Colors.White;
        private System.Windows.Media.Color _Io = Colors.Red;
        private double _m = 10;
        private Vertex _lightSource = new Vertex(1, 0, 4);
        private double _r = 1;


        public double kd { get { return _kd; } set { _kd = value; /*OnPropertyChanged();*/ } }
        public double ks { get { return _ks; } set { _ks = value; } }
        public double m { get { return _m; } set { _m = value; } }
        public System.Windows.Media.Color Il { get { return _Il; } set { _Il = value; } }
        public System.Windows.Media.Color Io { get { return _Io; } set { _Io = value; } }
        public Vertex lightSource { get { return _lightSource; } set { _lightSource = value; } }
        public double z { get { return _lightSource.Z; } set {  _lightSource.Z = value; } }
        public double r { get { return _r; } set { _r = value; } }



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
