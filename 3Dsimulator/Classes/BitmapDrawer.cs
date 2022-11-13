using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace _3Dsimulator.Classes
{
    public class BitmapDrawer
    {
        private ObjShape loadedShape;
        public ObjShape LoadedShape { get { return loadedShape; } }

        private AppState appState;

        public WriteableBitmap bitmap;
        private Image image;

        private int width;
        private int height;

        public BitmapDrawer(ObjShape ls, Image im, AppState aS)
        { 
            appState = aS;
            loadedShape = ls;
            image = im;
            bitmap = new WriteableBitmap(600, 600, 96, 96, PixelFormats.Bgr32, null);
            image.Source = bitmap;
            width = (int)(image.Source.Width);
            height = (int)image.Source.Height;
            bitmap.FillRectangle(0, 0, (int)width, (int)height, Colors.Blue);
        }

        public List<ETitem>[] CreateET(Face f)
        {
            var ET = new List<ETitem>[height];
            
            foreach(var e in f.Edges)
            {
                if (e.ymin != e.ymax)
                {
                    if (ET[(int)e.ymin] == null) ET[(int)e.ymin] = new List<ETitem>();
                    ET[(int)e.ymin].Add(new ETitem(e.ymax, e.xminAlgorithm, e.dx_dy));
                }
            }
            return ET;
        }

        public void FillFace(Face f, Vertex LightSource)
        {
            var colors = new Color[f.Vertices.Count];
            for(int i = 0; i < f.Vertices.Count; i++)
            {
                colors[i] = Geo.getVertexColor(f.Vertices[i], LoadedShape);
            }

            int y = (int)f.ymin;
            var ET = CreateET(f);
            List<ETitem> AET = new List<ETitem>();
            while (y <= (int)f.ymax)
            {
                if(ET[y]!=null)
                    AET = new List<ETitem>(AET.Concat(ET[y]));

                AET.Sort((a,b)=> {
                    if (a.xmin < b.xmin) return -1;
                    else if(a.xmin > b.xmin) return 1;
                    return 0;});

                // Drawing
                bitmap.Lock();
                int i = 0;
                ETitem prev = null;
                foreach (var el in AET)
                {
                    if (i % 2 == 1)
                    {
                        var a = 0;
                        //bitmap.DrawLine((int)prev.xmin, y, (int)el.xmin, y, Colors.Red);
                        for(int x = (int)prev.xmin; x <= (int)el.xmin; x++)
                        {
                            
                            double mian = (f.Vertices[1].Y - f.Vertices[2].Y) * (f.Vertices[0].X - f.Vertices[2].X) +
                                (f.Vertices[2].X - f.Vertices[1].X) * (f.Vertices[0].Y - f.Vertices[2].Y);
                            double wv1 = ((f.Vertices[1].Y - f.Vertices[2].Y) * (x - f.Vertices[2].X) +
                                (f.Vertices[2].X - f.Vertices[1].X) * (y - f.Vertices[2].Y)) / mian;
                            double wv2 = ((f.Vertices[2].Y - f.Vertices[0].Y) * (x - f.Vertices[2].X) +
                                (f.Vertices[0].X - f.Vertices[2].X) * (y - f.Vertices[2].Y)) / mian;
                            double wv3 = 1 - wv1 - wv2;
                            byte R, G, B;
                            NormalVector nv;
                            Color c;
                            // Interpolation for triangle right now (colors interpolation)
                            if (appState.ColorInterpolation)
                            {
                                R = (byte)(wv1 * colors[0].R + wv2 * colors[1].R + wv3 * colors[2].R);
                                G = (byte)(wv1 * colors[0].G + wv2 * colors[1].G + wv3 * colors[2].G);
                                B = (byte)(wv1 * colors[0].B + wv2 * colors[1].B + wv3 * colors[2].B);
                                c = Color.FromRgb(R, G, B);
                            }
                            else // Vectors interpolation
                            {
                                nv = new NormalVector(wv1 * f.Vertices[0].normalVector.X + wv2 * f.Vertices[1].normalVector.X + wv3 * f.Vertices[2].normalVector.X,
                                    wv1 * f.Vertices[0].normalVector.Y + wv2 * f.Vertices[1].normalVector.Y + wv3 * f.Vertices[2].normalVector.Y,
                                    wv1 * f.Vertices[0].normalVector.Z + wv2 * f.Vertices[1].normalVector.Z + wv3 * f.Vertices[2].normalVector.Z);

                                Vertex v = new Vertex(Geo.w2x(x), Geo.h2y(y), wv1 * Geo.p2z(f.Vertices[0].Z) + wv2 * Geo.p2z(f.Vertices[1].Z) + wv3 * Geo.p2z(f.Vertices[2].Z)); // To change z
                                v.AddNormalVector(nv);
                                if (appState.TextureEnabled)
                                {
                                    c = Geo.getVertexColor(v,LoadedShape, appState.Texture.GetPixel(x, y));
                                }
                                else
                                    c = Geo.getVertexColor(v, LoadedShape);
                            }
                            
                            bitmap.SetPixel(x, y, c);
                        }
                    }
                    prev = el;
                    i++;
                }
                bitmap.Unlock();

                y++;
                for (int x = 0; x < AET.Count; x++)
                {
                    ETitem item = AET[x];
                    item.xmin += item.dx_dy;
                    if(item.ymax <= y)
                    {
                        AET.RemoveAt(x);
                    }
                }
                
            }
        }

        public void FillObject()
        {
            foreach (var f in loadedShape.Faces)
                FillFace(f, loadedShape.lightSource);
        }

        public void draw()
        {
            bitmap.Lock();
            FillObject();
            if (appState.GridEnabled)
            {
                foreach (var f in loadedShape.Faces)
                {
                    var vc = f.Vertices.Count;
                    for (int i = 0; i < vc; i++)
                    {
                        bitmap.DrawLine((int)f.Edges[i].V1.X, (int)(f.Edges[i].V1.Y),
                            (int)(f.Edges[i].V2.X), (int)(f.Edges[i].V2.Y), Colors.Black);
                    }
                }
            }
            bitmap.Unlock();
        }
    }
}
