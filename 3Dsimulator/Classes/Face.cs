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

        public ETitem[][] AET;
        public NormalVector[,] normalVectors;
        public NormalVector[,] normalModifiedVectors;

        public void MoveCloudHorizontally(double x)
        {
            foreach(Vertex v in Vertices)
            {
                v.X += x;
            }
            setAET();
        }

        // COME BACK THERE - currently useless
        public void interpolateVectors()
        {
            int y = (int)ymin;
            int yBase = y;
            double mian, wv1, wv2, wv3;
            normalVectors = new NormalVector[600, 600];

            while (y <= (int)Math.Round(ymax))
            {
                int i = 0;
                ETitem prev = null;
                foreach (var el in AET[y - yBase])
                {
                    if (i % 2 == 1)
                    {
                        var a = 0;
                        for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                        {

                            mian = (Vertices[1].Y - Vertices[2].Y) * (Vertices[0].X - Vertices[2].X) +
                                            (Vertices[2].X - Vertices[1].X) * (Vertices[0].Y - Vertices[2].Y);
                            wv1 = ((Vertices[1].Y - Vertices[2].Y) * (x - Vertices[2].X) +
                                (Vertices[2].X - Vertices[1].X) * (y - Vertices[2].Y)) / mian;
                            wv2 = ((Vertices[2].Y - Vertices[0].Y) * (x - Vertices[2].X) +
                                (Vertices[0].X - Vertices[2].X) * (y - Vertices[2].Y)) / mian;
                            wv3 = 1 - wv1 - wv2;
                            normalVectors[x,y] = new NormalVector(wv1 * Vertices[0].normalVector.X + wv2 * Vertices[1].normalVector.X + wv3 * Vertices[2].normalVector.X,
                                wv1 * Vertices[0].normalVector.Y + wv2 * Vertices[1].normalVector.Y + wv3 * Vertices[2].normalVector.Y,
                                wv1 * Vertices[0].normalVector.Z + wv2 * Vertices[1].normalVector.Z + wv3 * Vertices[2].normalVector.Z);
                        }
                    }
                    prev = el;
                    i++;
                }
                y++;
            }
        }

        public double ZValue(int x, int y)
        {
            var mian = (Vertices[1].Y - Vertices[2].Y) * (Vertices[0].X - Vertices[2].X) +
                                            (Vertices[2].X - Vertices[1].X) * (Vertices[0].Y - Vertices[2].Y);
            var wv1 = ((Vertices[1].Y - Vertices[2].Y) * (x - Vertices[2].X) +
                (Vertices[2].X - Vertices[1].X) * (y - Vertices[2].Y)) / mian;
            var wv2 = ((Vertices[2].Y - Vertices[0].Y) * (x - Vertices[2].X) +
                (Vertices[0].X - Vertices[2].X) * (y - Vertices[2].Y)) / mian;
            var wv3 = 1 - wv1 - wv2;
            double z = wv1 * Vertices[0].Z + wv2 * Vertices[1].Z + wv3 * Vertices[2].Z;
            return z;
        }
        
        public void setAET()
        {
            int AETsize = (int)Math.Round(ymax) - (int)ymin + 1;
            this.AET = new ETitem[AETsize][];
            var ET = BitmapDrawer.CreateET(this, 600); // HARDCODE
            int y = (int)ymin;
            int yBase = y;
            List<ETitem> AETnow = new List<ETitem>();
            while (y <= (int)Math.Round(this.ymax))
            {
                if (ET[y] != null)
                    AETnow = new List<ETitem>(AETnow.Concat(ET[y]));

                AETnow.Sort((a, b) => {
                    if (a.xmin < b.xmin) return -1;
                    else if (a.xmin > b.xmin) return 1;
                    return 0;
                });

                AET[y - yBase] = new ETitem[AETnow.Count];

                for(int i=0;i<AETnow.Count;i++)
                {
                    AET[y - yBase][i] = new ETitem(AETnow[i].ymax,AETnow[i].xmin,AETnow[i].dx_dy);
                }

                y++;
                for (int x = 0; x < AETnow.Count; x++)
                {
                    ETitem item = AETnow[x];
                    item.xmin += item.dx_dy;
                    if (item.ymax <= y)
                    {
                        AETnow.RemoveAt(x);
                    }
                }
            }
        }

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

    }
}
