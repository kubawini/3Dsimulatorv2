using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Numerics;
using System.Windows.Data;

namespace _3Dsimulator.Classes
{
    public class BitmapDrawer
    {
        public int size = 600;

        private List<ObjShape> loadedShapes;
        public List<ObjShape> LoadedShapes { get { return loadedShapes; } }

        public int objCounter
        {
            get { return loadedShapes.Count; }
        }

        private AppState appState;

        public ObjShape mainShape;

        public WriteableBitmap bitmap;
        private Image image;

        private int width;
        private int height;

        public Face cloud;

        public double[,] zBuffer;

        public BitmapDrawer(ObjShape ls, Image im, AppState aS, Face c)
        {
            appState = aS;
            loadedShapes = new List<ObjShape>();
            mainShape = ls;
            loadedShapes.Add(ls);
            image = im;
            bitmap = new WriteableBitmap(size, size, 96, 96, PixelFormats.Bgr32, null);
            image.Source = bitmap;
            width = (int)(image.Source.Width);
            height = (int)image.Source.Height;
            bitmap.FillRectangle(0, 0, (int)width, (int)height, Colors.White);
            cloud = c;
            zBuffer = new double[width, height];
        }

        public void add4spheres()
        {
            for(int i = 0; i < 4; i++)
            {
                addSphere("../../../Resources/Sphere.obj", 0.5, 0.5, 50, Colors.Red, i);
            }
        }

        public void normalizeTorus(ObjShape torus)
        {
            foreach(var vertex in torus.Vertices)
            {
                vertex.X = (vertex.X - 0.5 * size) / 4 + 0.5 * size;
                vertex.Y = (vertex.Y - 0.5 * size) / 4 + 0.5 * size;
                vertex.Z = vertex.Z / 4;
                //vertex.Z -= vertex.Z / 8;
            }
        }

        public void addSphere(string filePath, double kd, double ks, int m, Color color, int nr)
        {
            var newObj = ObjReader.read(filePath);
            newObj.kd = kd;
            newObj.ks = ks;
            newObj.m = m;
            newObj.Io = color;

            for(int i=0; i < newObj.Vertices.Count; i++)
            {
                newObj.Vertices[i].X = (newObj.Vertices[i].X - 0.5f*size) / 4 + size * (2 *(nr<2?0:3)+1) / 8;
                newObj.Vertices[i].Y = (newObj.Vertices[i].Y - 0.5f*size) / 4 + (2 * (nr%2==0?0:3) + 1) * size / 8;
                newObj.Vertices[i].Z = newObj.Vertices[i].Z/4;
                //newObj.Vertices[i].X = (int)(newObj.Vertices[i].X / (4 + 1) + size * (nr) / (4 + 1));
                //newObj.Vertices[i].Y = (int)(newObj.Vertices[i].Y / (4 + 1) + size * (nr) / (4 + 1));
                //newObj.Vertices[i].Z = (int)(newObj.Vertices[i].Z / (4 + 1) + size * (nr) / (4 + 1));
            }

            loadedShapes.Add(newObj);
        }

        public void addNewObj(string filePath, Slider kdSlider, Slider ksSlider, Slider mSlider)
        {
            // I should add scaling in case two objects are of different dimensions
            var newObj = ObjReader.read(filePath);

            //for(int i=0; i<loadedShapes.Count;i++)
            //{
            //    for(int j = 0; j < loadedShapes[i].Vertices.Count; j++)
            //    {
            //        loadedShapes[i].Vertices[j].X = (int)((loadedShapes[i].Vertices[j].X - i * size / objCounter) * objCounter / (objCounter + 1) + size * i / (objCounter + 1));
            //        loadedShapes[i].Vertices[j].Y = (int)((loadedShapes[i].Vertices[j].Y - i * size / objCounter) * objCounter / (objCounter + 1) + size * i / (objCounter + 1));
            //        loadedShapes[i].Vertices[j].Z = (int)((loadedShapes[i].Vertices[j].Z - i * size / objCounter) * objCounter / (objCounter + 1) + size * i / (objCounter + 1));
            //    }
            //}
            //for(int i=0; i<newObj.Vertices.Count;i++)
            //{
            //    newObj.Vertices[i].X = (int)(newObj.Vertices[i].X / (objCounter + 1) + size * (objCounter) / (objCounter+1));
            //    newObj.Vertices[i].Y = (int)(newObj.Vertices[i].Y / (objCounter + 1) + size * (objCounter) / (objCounter + 1));
            //    newObj.Vertices[i].Z = (int)(newObj.Vertices[i].Z / (objCounter + 1) + size * (objCounter) / (objCounter + 1));
            //}

            var kdBinding = new Binding("kd");
            kdBinding.Source = newObj;
            kdBinding.Mode = BindingMode.TwoWay;
            kdSlider.SetBinding(Slider.ValueProperty, kdBinding);

            var ksBinding = new Binding("ks");
            ksBinding.Source = newObj;
            ksBinding.Mode = BindingMode.TwoWay;
            ksSlider.SetBinding(Slider.ValueProperty, ksBinding);

            var mBinding = new Binding("m");
            mBinding.Source = newObj;
            mBinding.Mode = BindingMode.TwoWay;
            mSlider.SetBinding(Slider.ValueProperty, mBinding);

            loadedShapes.Add(newObj);
        }


        public static List<ETitem>[] CreateET(Face f, int h)
        {
            var ET = new List<ETitem>[h + 1];


            foreach (var e in f.Edges)
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

        public Color countColorVectorInterpolation(Face f, Face realFace, int x, int y, Color c1, ObjShape objShape)
        {
            double wv1, wv2, wv3;
            (wv1, wv2, wv3) = countWVs(f, x, y);
            var nv = countNV(realFace, wv1, wv2, wv3);

            Vertex v = new Vertex(-Geo.w2x(x), Geo.h2y(y), wv1 * Geo.p2z(realFace.Vertices[0].Z) + wv2 * Geo.p2z(realFace.Vertices[1].Z) + wv3 * Geo.p2z(realFace.Vertices[2].Z)); // To change z
            v.AddNormalVector(nv);
            var c = Geo.getVertexColor(v, objShape, c1, appState);
            return c;
        }

        public Color countColorVectorInterpolationMapping(Face f, int x, int y, Color c1, ObjShape objShape)
        {
            double wv1, wv2, wv3;
            (wv1, wv2, wv3) = countWVs(f, x, y);
            var nv = countNV(f, wv1, wv2, wv3);

            // Modifying nv
            nv = Geo.modifyNormalVector(nv, appState, x, y, this);

            Vertex v = new Vertex(Geo.w2x(x), Geo.h2y(y), wv1 * Geo.p2z(f.Vertices[0].Z) + wv2 * Geo.p2z(f.Vertices[1].Z) + wv3 * Geo.p2z(f.Vertices[2].Z)); // To change z
            v.AddNormalVector(nv);
            var c = Geo.getVertexColor(v, objShape, c1, appState);
            return c;
        }


        public Color countColorVectorInterpolation(Face f, Face realFace, int x, int y, ObjShape objShape)
        {
            return countColorVectorInterpolation(f, realFace, x, y, objShape.Io, objShape);
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

        public void FillFace(Face f, Face realFace, ObjShape objShape, Color col = new Color(), float fogValue = 0)
        {
            var colors = new Color[realFace.Vertices.Count];
            int y = (int)f.ymin;
            int yBase = y;
            Color c;
            byte R, G, B;
            double mian, wv1, wv2, wv3;
            NormalVector nv;

            

            for (int i = 0; i < realFace.Vertices.Count; i++)
            {
            //    if (appState.TextureEnabled)
            //        if(appState.NormalMapEnabled)
            //            colors[i] = Geo.getVertexColor(f.Vertices[i], objShape, appState.TextureColors[(int)f.Vertices[i].X, (int)f.Vertices[i].Y],
            //                Geo.modifyNormalVector(f.Vertices[i].normalVector,appState,(int)f.Vertices[i].X,(int)f.Vertices[i].Y,this));
            //        else
            //            colors[i] = Geo.getVertexColor(f.Vertices[i], objShape, appState.TextureColors[(int)f.Vertices[i].X, (int)f.Vertices[i].Y]);
            //    else
            //        if (appState.NormalMapEnabled)
            //            colors[i] = Geo.getVertexColor(f.Vertices[i], objShape, objShape.Io,
            //                Geo.modifyNormalVector(f.Vertices[i].normalVector, appState, (int)f.Vertices[i].X, (int)f.Vertices[i].Y, this));
            //        else
                        colors[i] = Geo.getVertexColor(realFace.Vertices[i], objShape, appState);
            }

            var equalColor = new Color();
            if (realFace.Vertices.Count >= 3)
            {
                equalColor.R = (byte)(((uint)colors[0].R + (uint)colors[1].R + (uint)colors[2].R) / 3);
                equalColor.G = (byte)(((uint)colors[0].G + (uint)colors[1].G + (uint)colors[2].G) / 3);
                equalColor.B = (byte)(((uint)colors[0].B + (uint)colors[1].B + (uint)colors[2].B) / 3);
                equalColor.A = 255;
            }

            if (appState.RotatingAllowed)
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
                                double z = f.ZValue(x, y);
                                if (z <= zBuffer[x, y])
                                {
                                    if(appState.EqualInterpolation)
                                    {
                                        col = equalColor;
                                    }
                                    else if (appState.ColorInterpolation)
                                    {
                                        // Interpolate colors
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
                                        col = Color.FromRgb(R, G, B);
                                    }
                                    else
                                    {
                                        // Repair that shit
                                        col = countColorVectorInterpolation(f, realFace, x, y, objShape.Io, objShape);
                                    }

                                    if(appState.Fog)
                                    {
                                        mian = (f.Vertices[1].Y - f.Vertices[2].Y) * (f.Vertices[0].X - f.Vertices[2].X) +
                                            (f.Vertices[2].X - f.Vertices[1].X) * (f.Vertices[0].Y - f.Vertices[2].Y);
                                        wv1 = ((f.Vertices[1].Y - f.Vertices[2].Y) * (x - f.Vertices[2].X) +
                                            (f.Vertices[2].X - f.Vertices[1].X) * (y - f.Vertices[2].Y)) / mian;
                                        wv2 = ((f.Vertices[2].Y - f.Vertices[0].Y) * (x - f.Vertices[2].X) +
                                            (f.Vertices[0].X - f.Vertices[2].X) * (y - f.Vertices[2].Y)) / mian;
                                        wv3 = 1 - wv1 - wv2;

                                        int r = (int)(((int)col.R) + (wv1 * f.Vertices[0].fogValue +
                                            wv2 * f.Vertices[1].fogValue + wv3 * f.Vertices[2].fogValue)*255/4);
                                        if (r > 255) r = 255;
                                        int g = (int)(((int)col.G) + (wv1 * f.Vertices[0].fogValue +
                                            wv2 * f.Vertices[1].fogValue + wv3 * f.Vertices[2].fogValue) * 255/4);
                                        if (g > 255) g = 255;
                                        int b = (int)(((int)col.B) + (wv1 * f.Vertices[0].fogValue +
                                            wv2 * f.Vertices[1].fogValue + wv3 * f.Vertices[2].fogValue) * 255/4);
                                        if (b > 255) b = 255;
                                        col = Color.FromRgb((byte)r, (byte)g, (byte)b);
                                    }

                                    if(appState.Night)
                                    {
                                        col.A = (byte)(255 * appState.nocWspolczynnik);
                                    }

                                    bitmap.SetPixel(x, y, col);
                                    zBuffer[x, y] = z;
                                }
                            }
                        }
                        prev = el;
                        i++;
                    }
                    y++;
                }
                return;
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
                //if(appState.NormalMapEnabled)
                //{
                //    if (appState.TextureEnabled)
                //    {
                //        while (y <= (int)Math.Round(f.ymax))
                //        {
                //            // Drawing
                //            int i = 0;
                //            ETitem prev = null;
                //            foreach (var el in f.AET[y - yBase])
                //            {
                //                if (i % 2 == 1)
                //                {
                //                    var a = 0;
                //                    for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                //                    {
                //                        c = countColorVectorInterpolationMapping(f, x, y, appState.TextureColors[x, y], objShape);
                //                        bitmap.SetPixel(x, y, c);
                //                    }
                //                }
                //                prev = el;
                //                i++;
                //            }
                //            y++;
                //        }
                //    }
                //    else
                //    {
                //        while (y <= (int)Math.Round(f.ymax))
                //        {
                //            // Drawing
                //            int i = 0;
                //            ETitem prev = null;
                //            foreach (var el in f.AET[y - yBase])
                //            {
                //                if (i % 2 == 1)
                //                {
                //                    var a = 0;
                //                    for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                //                    {
                //                        c = countColorVectorInterpolationMapping(f, x, y, objShape.Io, objShape);
                //                        bitmap.SetPixel(x, y, c);
                //                    }
                //                }
                //                prev = el;
                //                i++;
                //            }
                //            y++;
                //        }
                //    }
                //}
                //else
                {
                    //if (appState.TextureEnabled)
                    //{
                    //    while (y <= (int)Math.Round(f.ymax))
                    //    {
                    //        // Drawing
                    //        int i = 0;
                    //        ETitem prev = null;
                    //        foreach (var el in f.AET[y - yBase])
                    //        {
                    //            if (i % 2 == 1)
                    //            {
                    //                var a = 0;
                    //                for (int x = (int)prev.xmin; x <= Math.Round(el.xmin); x++)
                    //                {
                    //                    c = countColorVectorInterpolation(f, x, y, appState.TextureColors[x,y], objShape);
                    //                    bitmap.SetPixel(x, y, c);
                    //                }
                    //            }
                    //            prev = el;
                    //            i++;
                    //        }
                    //        y++;
                    //    }
                    //}
                    //else
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
                                        c = countColorVectorInterpolation(f, realFace, x, y, objShape);
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
            foreach (var loadedShape in loadedShapes)
                foreach (var f in loadedShape.Faces)
                {
                    if (!appState.RotatingAllowed)
                        FillFace(f, f, loadedShape);
                }
        }

        public void draw()
        {
            bitmap.Lock();
            bitmap.FillRectangle(0, 0, width, height, Colors.White);
            //if(appState.PaintingAllowed)
            //FillObject();

            // czyszczenie bufora i ekranu!!!
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    zBuffer[x, y] = double.MaxValue;
                }
            }

            // Drgania
            var r = new Random();
            float xD = ((float)(r.Next() % 500)) / 10000 - 0.025f;
            float yD = ((float)(r.Next() % 500)) / 10000 - 0.025f;
            float zD = ((float)(r.Next() % 500)) / 10000 - 0.025f;

            int ind = 0;
            float fogValue = 0;
            foreach (var loadedShape in loadedShapes)
            {
                ind++;
                foreach (var f in loadedShape.Faces)
                {
                    Face curr_face = new Face();
                    Face real_face = new Face();
                    var vc = f.Vertices.Count;
                    
                    for (int i = 0; i < vc; i++)
                    {
                        if (appState.RotatingAllowed)
                        {
                            float v1x = (float)(f.Edges[i].V1.X - size / 2) * 2 / size;
                            float v1y = (float)(f.Edges[i].V1.Y - size / 2) * 2 / size;
                            float v1z = (float)f.Edges[i].V1.Z * 2 / size;
                            float v2x = (float)(f.Edges[i].V2.X - size / 2) * 2 / size;
                            float v2y = (float)(f.Edges[i].V2.Y - size / 2) * 2 / size;
                            float v2z = (float)f.Edges[i].V2.Z * 2 / size;

                                fogValue = (v1x - appState.XC) * (v1x - appState.XC) +
                                    (v1y - appState.YC) * (v1y - appState.YC) + (v1z - appState.ZC) * (v1z - appState.ZC);
                                fogValue *= appState.fogWspolczynnik;

                            


                            Vector4 v1Start = new Vector4(v1x, v1y, v1z, 1);
                            Vector4 v2Start = new Vector4(v2x, v2y, v2z, 1);
                            Vector3 normalVectorStart =
                                new Vector3((float)f.Vertices[i].normalVector.X, (float)f.Vertices[i].normalVector.Y,
                                (float)f.Vertices[i].normalVector.Z);
                            Matrix4x4 rotation = Matrix4x4.CreateRotationX(0);
                            if(loadedShape==mainShape)
                                rotation = Matrix4x4.CreateRotationX(appState.kat * (ind * (int)Math.Pow(-1, ind)));
                            Vector3 normalVectorEnd = Vector3.TransformNormal(normalVectorStart, rotation);

                            Matrix4x4 translate = Matrix4x4.CreateTranslation(new Vector3(0, appState.translate, 0));

                            if(appState.Vibrations)
                            {
                                translate = Matrix4x4.CreateTranslation(new Vector3(xD, appState.translate + yD, zD));
                            }

                            if (loadedShape == mainShape)
                            {
                                //Vector4 v1Real = new Vector4((float)f.Edges[i].V1.X, (float)f.Edges[i].V1.Y, (float)f.Edges[i].V1.Z, 1);
                                Vector4 v1Real = new Vector4(v1x, v1y, v1z, 1);
                                Vector4 v1RealN = new Vector4((float)f.Edges[i].V1.normalVector.X, (float)f.Edges[i].V1.normalVector.Y,
                                    (float)f.Edges[i].V1.normalVector.Z, 1);
                                v1Real = Vector4.Transform(Vector4.Transform(v1Real, rotation), translate);
                                v1RealN = Vector4.Transform(Vector4.Transform(v1RealN, rotation), translate);
                                var realVertex = new Vertex(v1Real.X, v1Real.Y, v1Real.Z);
                                realVertex.AddNormalVector(new NormalVector(v1RealN.X,v1RealN.Y,v1RealN.Z));
                                real_face.Vertices.Add(realVertex);
                            }
                            else
                            {
                                real_face = f;
                            }

                            Vector3 cameraPosition = new Vector3(appState.XC, appState.YC, appState.ZC);
                            Vector3 cameraTarget = new Vector3(0, 0, 0);
                            Vector3 cameraUp = new Vector3(0, 0, 1);
                            if(appState.Camera == 2)
                            {
                                cameraPosition = new Vector3(0, 0.5f, 0.2f);
                                cameraTarget = new Vector3(0, appState.translate, 0);
                            }
                            else if(appState.Camera == 3)
                            {
                                cameraPosition = new Vector3(0, appState.translate - 0.5f, 1);
                                cameraTarget = new Vector3(0, appState.translate + 0.5f, 0);
                            }
                            Matrix4x4 view = Matrix4x4.CreateLookAt(cameraPosition, cameraTarget, cameraUp);

                            Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(appState.Fov, 1, 100, 1500);

                            if (loadedShape == mainShape)
                            {
                                v1Start = Vector4.Transform(v1Start, translate);
                                v2Start = Vector4.Transform(v2Start, translate);
                            }

                            var v1End = Vector4.Transform(Vector4.Transform(Vector4.Transform(v1Start, rotation), view), projection);
                            var v2End = Vector4.Transform(Vector4.Transform(Vector4.Transform(v2Start, rotation), view), projection);

                            int v1_x = (int)(v1End.X * (size / 2) + (size / 2));
                            int v1_y = (int)(-v1End.Y * (size / 2) + (size / 2));
                            int v1_z = (int)(v1End.Z * (size / 2));
                            int v2_x = (int)(v2End.X * (size / 2) + (size / 2));
                            int v2_y = (int)(-v2End.Y * (size / 2) + (size / 2));


                            if (v1_x < 600 && v1_x > 0 && v1_y < 600 && v1_y > 0 && v2_x < 600 && v2_x > 0 && v2_y < 600 && v2_y > 0)
                            {
                                //if (!appState.PaintingAllowed)
                               //     bitmap.DrawLine(v1_x, v1_y, v2_x, v2_y, Colors.Black);
                                Vertex v = new Vertex(v1End.X, -v1End.Y, v1End.Z);
                                v.normalVector = new NormalVector(normalVectorEnd.X, normalVectorEnd.Y, normalVectorEnd.Z);
                                v.fogValue = fogValue;
                                curr_face.AddVertex(v);
                                //curr_face.AddVertex(new Vertex(v1_x, v1_y, v1_z));
                            }
                            else
                            {
                                // Dodać takie wczytywanie trójkątów żeby działało jak wyjdziemy za canvas
                                // .
                                // .
                                // .
                                // Albo i nie xD 
                            }

                        }
                        else
                        {
                            bitmap.DrawLine((int)f.Edges[i].V1.X, (int)(f.Edges[i].V1.Y),
                                (int)(f.Edges[i].V2.X), (int)(f.Edges[i].V2.Y), Colors.Black);
                        }
                    }


                    if (appState.PaintingAllowed && curr_face.Vertices.Count > 0)
                    {
                        curr_face.CreateEdges();
                        curr_face.setAET();
                        curr_face.interpolateVectors();
                        
                        FillFace(curr_face, real_face, loadedShape,
                            Color.FromRgb((byte)((ind * 50) % 256), (byte)((ind * 50) % 256), (byte)((ind * 50) % 256)), fogValue);
                    }
                
            
         
            }
        }

        //foreach (var loadedShape in LoadedShapes)
        //{
        //    if (appState.CloudAllowed && loadedShape.lightSource.Z > cloud.Vertices[0].Z)
        //    {
        //        Face shade = new Face();
        //        foreach (var v in cloud.Vertices)
        //        {
        //            if (loadedShape.lightSource.Z - v.Z != 0)
        //            {
        //                shade.AddVertex(new Vertex(Geo.w2x(loadedShape.lightSource.X + (v.X - loadedShape.lightSource.X) * loadedShape.lightSource.Z / (loadedShape.lightSource.Z - v.Z)),
        //                    Geo.h2y(loadedShape.lightSource.Y + (v.Y - loadedShape.lightSource.Y) * loadedShape.lightSource.Z / (loadedShape.lightSource.Z - v.Z)), 0));
        //            }
        //            else
        //            {
        //                shade.AddVertex(new Vertex(0, 0, 0));
        //            }
        //        }
        //        shade.CreateEdges();
        //        shade.setAET();
        //        FillCloud(shade, Color.FromRgb(5, 5, 5));
        //        FillCloud(cloud, Colors.White);
        //    }
        //}

        //bitmap.FillEllipse((int)loadedShapes[0].lightSource.X - 5, (int)loadedShapes[0].lightSource.Y - 5,
        //        (int)loadedShapes[0].lightSource.X + 5, (int)loadedShapes[0].lightSource.Y + 5, loadedShapes[0].Il);
        //    bitmap.DrawEllipse((int)loadedShapes[0].lightSource.X - 5, (int)loadedShapes[0].lightSource.Y - 5,
        //        (int)loadedShapes[0].lightSource.X + 5, (int)loadedShapes[0].lightSource.Y + 5, 0);
            //if(appState.CloudAllowed && loadedShapes[0].lightSource.Z <= cloud.Vertices[0].Z)
            //{
            //    FillCloud(cloud, Colors.White);
            //}
            bitmap.Unlock();
        }
    }
}
