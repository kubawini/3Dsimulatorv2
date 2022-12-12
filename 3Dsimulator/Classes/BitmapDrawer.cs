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
        public int size = 600;

        private ObjShape loadedShape;
        public ObjShape LoadedShape { get { return loadedShape; } }

        private AppState appState;

        public WriteableBitmap bitmap;
        private Image image;

        private int width;
        private int height;

        public Face cloud;

        public BitmapDrawer(ObjShape ls, Image im, AppState aS, Face c)
        { 
            appState = aS;
            loadedShape = ls;
            image = im;
            bitmap = new WriteableBitmap(size, size, 96, 96, PixelFormats.Bgr32, null);
            image.Source = bitmap;
            width = (int)(image.Source.Width);
            height = (int)image.Source.Height;
            bitmap.FillRectangle(0, 0, (int)width, (int)height, Colors.White);
            cloud = c;
        }

        public static List<ETitem>[] CreateET(Face f, int h)
        {
            var ET = new List<ETitem>[h + 1];
            

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

        public List<ETitem>[] CreateET(Face f)
        {
            return CreateET(f, height);
        }

        public (double wv1, double wv2, double wv3) countWVs(Face f, int x, int y)
        {
            var mian = (f.Vertices[1].Y - f.Vertices[2].Y) * (f.Vertices[0].X - f.Vertices[2].X) +
                                            (f.Vertices[2].X - f.Vertices[1].X) * (f.Vertices[0].Y - f.Vertices[2].Y);
            var wv1 = ((f.Vertices[1].Y - f.Vertices[2].Y) * (x - f.Vertices[2].X) +
                (f.Vertices[2].X - f.Vertices[1].X) * (y - f.Vertices[2].Y)) / mian;
            var wv2 = ((f.Vertices[2].Y - f.Vertices[0].Y) * (x - f.Vertices[2].X) +
                (f.Vertices[0].X - f.Vertices[2].X) * (y - f.Vertices[2].Y)) / mian;
            var wv3 = 1 - wv1 - wv2;
            return (wv1, wv2, wv3);
        }

        public NormalVector countNV(Face f, double wv1, double wv2, double wv3)
        {
            return new NormalVector(wv1 * f.Vertices[0].normalVector.X + wv2 * f.Vertices[1].normalVector.X + wv3 * f.Vertices[2].normalVector.X,
                wv1 * f.Vertices[0].normalVector.Y + wv2 * f.Vertices[1].normalVector.Y + wv3 * f.Vertices[2].normalVector.Y,
                wv1 * f.Vertices[0].normalVector.Z + wv2 * f.Vertices[1].normalVector.Z + wv3 * f.Vertices[2].normalVector.Z);
        }

        public Color countColorVectorInterpolation(Face f, int x, int y, Color c1)
        {
            double wv1, wv2, wv3;
            (wv1, wv2, wv3) = countWVs(f, x, y);
            var nv = countNV(f, wv1, wv2, wv3);

            Vertex v = new Vertex(Geo.w2x(x), Geo.h2y(y), wv1 * Geo.p2z(f.Vertices[0].Z) + wv2 * Geo.p2z(f.Vertices[1].Z) + wv3 * Geo.p2z(f.Vertices[2].Z)); // To change z
            v.AddNormalVector(nv);
            var c = Geo.getVertexColor(v, LoadedShape, c1);
            return c;
        }

        public Color countColorVectorInterpolationMapping(Face f, int x, int y, Color c1)
        {
            double wv1, wv2, wv3;
            (wv1, wv2, wv3) = countWVs(f, x, y);
            var nv = countNV(f, wv1, wv2, wv3);

            // Modifying nv
            nv = Geo.modifyNormalVector(nv, appState, x, y, this);

            Vertex v = new Vertex(Geo.w2x(x), Geo.h2y(y), wv1 * Geo.p2z(f.Vertices[0].Z) + wv2 * Geo.p2z(f.Vertices[1].Z) + wv3 * Geo.p2z(f.Vertices[2].Z)); // To change z
            v.AddNormalVector(nv);
            var c = Geo.getVertexColor(v, LoadedShape, c1);
            return c;
        }


        public Color countColorVectorInterpolation(Face f, int x, int y)
        {
            return countColorVectorInterpolation(f, x, y, loadedShape.Io);
        }


        public void FillCloud(Face f, Color c)
        {
            int y = (int)f.ymin;
            int yBase = y;

            while (y <= (int)Math.Round(f.ymax))
            {
                // Drawing
                int i = 0;
                ETitem prev = null;
                foreach (var el in f.AET[y - yBase])
                {
                    if (i % 2 == 1)
                    {
                        var a = 0;
                        for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                        {
                            bitmap.SetPixel(x, y, c);
                        }
                    }
                    prev = el;
                    i++;
                }
                y++;
            }
        }

        public void FillFace(Face f, Vertex LightSource)
        {
            var colors = new Color[f.Vertices.Count];
            int y = (int)f.ymin;
            int yBase = y;
            Color c;
            byte R, G, B;
            double mian, wv1, wv2, wv3;
            NormalVector nv;

            for (int i = 0; i < f.Vertices.Count; i++)
            {
                if (appState.TextureEnabled)
                    if(appState.NormalMapEnabled)
                        colors[i] = Geo.getVertexColor(f.Vertices[i], LoadedShape, appState.TextureColors[(int)f.Vertices[i].X, (int)f.Vertices[i].Y],
                            Geo.modifyNormalVector(f.Vertices[i].normalVector,appState,(int)f.Vertices[i].X,(int)f.Vertices[i].Y,this));
                    else
                        colors[i] = Geo.getVertexColor(f.Vertices[i], LoadedShape, appState.TextureColors[(int)f.Vertices[i].X, (int)f.Vertices[i].Y]);
                else
                    if (appState.NormalMapEnabled)
                        colors[i] = Geo.getVertexColor(f.Vertices[i], LoadedShape, LoadedShape.Io,
                            Geo.modifyNormalVector(f.Vertices[i].normalVector, appState, (int)f.Vertices[i].X, (int)f.Vertices[i].Y, this));
                    else
                        colors[i] = Geo.getVertexColor(f.Vertices[i], LoadedShape);
            }

            if (appState.ColorInterpolation)
            {
                    while (y <= (int)Math.Round(f.ymax))
                    {
                        // Drawing
                        int i = 0;
                        ETitem prev = null;
                        foreach (var el in f.AET[y - yBase])
                        {
                            if (i % 2 == 1)
                            {
                                var a = 0;
                                for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                                {
                                    mian = (f.Vertices[1].Y - f.Vertices[2].Y) * (f.Vertices[0].X - f.Vertices[2].X) +
                                        (f.Vertices[2].X - f.Vertices[1].X) * (f.Vertices[0].Y - f.Vertices[2].Y);
                                    wv1 = ((f.Vertices[1].Y - f.Vertices[2].Y) * (x - f.Vertices[2].X) +
                                        (f.Vertices[2].X - f.Vertices[1].X) * (y - f.Vertices[2].Y)) / mian;
                                    wv2 = ((f.Vertices[2].Y - f.Vertices[0].Y) * (x - f.Vertices[2].X) +
                                        (f.Vertices[0].X - f.Vertices[2].X) * (y - f.Vertices[2].Y)) / mian;
                                    wv3 = 1 - wv1 - wv2;

                                    R = (byte)(wv1 * colors[0].R + wv2 * colors[1].R + wv3 * colors[2].R);
                                    G = (byte)(wv1 * colors[0].G + wv2 * colors[1].G + wv3 * colors[2].G);
                                    B = (byte)(wv1 * colors[0].B + wv2 * colors[1].B + wv3 * colors[2].B);
                                    c = Color.FromRgb(R, G, B);
                                
                                    bitmap.SetPixel(x, y, c);
                                }
                            }
                            prev = el;
                            i++;
                        }
                        y++;
                    }
            }
            else // VECTOR INTERPOLATION
            {
                if(appState.NormalMapEnabled)
                {
                    if (appState.TextureEnabled)
                    {
                        while (y <= (int)Math.Round(f.ymax))
                        {
                            // Drawing
                            int i = 0;
                            ETitem prev = null;
                            foreach (var el in f.AET[y - yBase])
                            {
                                if (i % 2 == 1)
                                {
                                    var a = 0;
                                    for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                                    {
                                        c = countColorVectorInterpolationMapping(f, x, y, appState.TextureColors[x, y]);
                                        bitmap.SetPixel(x, y, c);
                                    }
                                }
                                prev = el;
                                i++;
                            }
                            y++;
                        }
                    }
                    else
                    {
                        while (y <= (int)Math.Round(f.ymax))
                        {
                            // Drawing
                            int i = 0;
                            ETitem prev = null;
                            foreach (var el in f.AET[y - yBase])
                            {
                                if (i % 2 == 1)
                                {
                                    var a = 0;
                                    for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                                    {
                                        c = countColorVectorInterpolationMapping(f, x, y, loadedShape.Io);
                                        bitmap.SetPixel(x, y, c);
                                    }
                                }
                                prev = el;
                                i++;
                            }
                            y++;
                        }
                    }
                }
                else
                {
                    if (appState.TextureEnabled)
                    {
                        while (y <= (int)Math.Round(f.ymax))
                        {
                            // Drawing
                            int i = 0;
                            ETitem prev = null;
                            foreach (var el in f.AET[y - yBase])
                            {
                                if (i % 2 == 1)
                                {
                                    var a = 0;
                                    for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                                    {
                                        c = countColorVectorInterpolation(f, x, y, appState.TextureColors[x,y]);
                                        bitmap.SetPixel(x, y, c);
                                    }
                                }
                                prev = el;
                                i++;
                            }
                            y++;
                        }
                    }
                    else
                    {
                        while (y <= (int)Math.Round(f.ymax))
                        {
                            // Drawing
                            int i = 0;
                            ETitem prev = null;
                            foreach (var el in f.AET[y - yBase])
                            {
                                if (i % 2 == 1)
                                {
                                    var a = 0;
                                    for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                                    {
                                        c = countColorVectorInterpolation(f, x, y);
                                        bitmap.SetPixel(x, y, c);
                                    }
                                }
                                prev = el;
                                i++;
                            }
                            y++;
                        }
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
            bitmap.FillRectangle(0, 0, width, height, Colors.White);
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


            if (appState.CloudAllowed && loadedShape.lightSource.Z > cloud.Vertices[0].Z)
            {
                Face shade = new Face();
                foreach (var v in cloud.Vertices)
                {
                    if (loadedShape.lightSource.Z - v.Z != 0)
                    {
                        shade.AddVertex(new Vertex(Geo.w2x(loadedShape.lightSource.X + (v.X - loadedShape.lightSource.X) * loadedShape.lightSource.Z / (loadedShape.lightSource.Z - v.Z)),
                            Geo.h2y(loadedShape.lightSource.Y + (v.Y - loadedShape.lightSource.Y) * loadedShape.lightSource.Z / (loadedShape.lightSource.Z - v.Z)), 0));
                    }
                    else
                    {
                        shade.AddVertex(new Vertex(0, 0, 0));
                    }
                }
                shade.CreateEdges();
                shade.setAET();
                FillCloud(shade, Color.FromRgb(5,5,5));
                FillCloud(cloud, Colors.White);
            }

            bitmap.FillEllipse((int)loadedShape.lightSource.X - 5, (int)loadedShape.lightSource.Y - 5,
                (int)loadedShape.lightSource.X + 5, (int)loadedShape.lightSource.Y + 5, loadedShape.Il);
            bitmap.DrawEllipse((int)loadedShape.lightSource.X - 5, (int)loadedShape.lightSource.Y - 5,
                (int)loadedShape.lightSource.X + 5, (int)loadedShape.lightSource.Y + 5, 0);
            if(appState.CloudAllowed && loadedShape.lightSource.Z <= cloud.Vertices[0].Z)
            {
                FillCloud(cloud, Colors.White);
            }
            bitmap.Unlock();
        }
    }
}
