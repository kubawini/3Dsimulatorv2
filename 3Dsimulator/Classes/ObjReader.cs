using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dsimulator.Classes
{
    public static class ObjReader
    {
        public static ObjShape read(string filePath)
        {
            var result = new ObjShape();
            foreach (string line in System.IO.File.ReadLines(filePath))
            {
                var scheme = line.Split(' ');
                switch (scheme[0])
                {
                    case "v":
                        result.AddVertex(new Vertex(s2dConv(scheme[1]), s2dConv(scheme[2]), s2dConv(scheme[3])));
                        break;
                    case "vn":
                        result.AddNormalVector(new NormalVector(s2dConv(scheme[1]), s2dConv(scheme[2]), s2dConv(scheme[3])));
                        break;
                    case "f":
                        Face? f = null;
                        for(int i = 1; i < scheme.Length; i++)
                        {
                            if(i==1)
                            {
                                f = new Face();
                            }
                            f!.AddVertex(result.Vertices[s2tiConv(scheme[i]).i1 - 1]);
                            result.Vertices[s2tiConv(scheme[i]).i1 - 1].normalVector = result.NormalVectors[s2tiConv(scheme[i]).i3 - 1];
                            // To add 3rd field
                        }
                        f!.CreateEdges();
                        result.AddFace(f);
                        break;
                    default:
                        // To add extra formats
                        break;
                }
            }
            return result;
        }

        // String to double converter
        public static double s2dConv(string str)
        {
            return double.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
        }

        // String to Tuple of ints converter
        public static (int i1, int? i2, int i3) s2tiConv(string str)
        {
            var ints = str.Split('/');
            int id1 = int.Parse(ints[0]);
            int? id2 = null;
            if(ints[1].Length > 0)
                id2 = int.Parse(ints[1]);
            int id3 = int.Parse(ints[2]);
            return(id1, id2, id3);
        }
    }
}
