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
