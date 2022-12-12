using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _3Dsimulator.Classes
{
    public static class Geo
    {
        public static int width = 600;
        public static int height = 600;

        public static int x2w(double x)
        {
            return (int)(width / 2 * (x+1));
        }

        public static double w2x(double w)
        {
            if (2 * w / width - 1 > 1) return 1;
            else if (2 * w / width - 1 < -1) return -1;
            return 2 * w / width - 1;
        }

        public static int y2h(double y)
        {
            return (int)(height/2 * (y + 1));
        }

        public static double h2y(double h)
        {
            if (2 * h / height - 1 < -1) return -1;
            else if (2 * h / height - 1 > 1) return 1;
            return 2 * h / height - 1;
        }

        public static int z2p(double z)
        {
            return (int)(height / 2 * z);
        }

        public static double p2z(double p)
        {
            return (p / height * 2);
        }

        public static NormalVector modifyNormalVector(NormalVector nv, AppState appState, int x, int y, BitmapDrawer bm)
        {
            var normalMapSizeX = appState.NormalMap.PixelWidth;
            var normalMapSizeY = appState.NormalMap.PixelHeight;
            var xM = ((double)x) / ((double)bm.size) * normalMapSizeX;
            if (xM > normalMapSizeX) xM = normalMapSizeX;
            var yM = ((double)y) / ((double)bm.size) * normalMapSizeY;
            if (yM > normalMapSizeX) yM = normalMapSizeY;

            var colorNormalMap = appState.NormalMap.GetPixel((int)xM, (int)yM);

            var Bv = new NormalVector(0, 0, 1); //nv.Z?
            if (nv.Z == 1 && nv.X == 0 && nv.Y == 0) Bv = new NormalVector(0, 1, 0);
            var Tv = new NormalVector(Bv.X * nv.X, Bv.Y * nv.Y, Bv.Z * nv.Z);
            var Ntv = new NormalVector(((double)colorNormalMap.R) / 128 - 1, ((double)colorNormalMap.G) / 128 - 1, ((double)colorNormalMap.B) / 255);
            nv = new NormalVector(Tv.X * Ntv.X + Bv.X * Ntv.Y + nv.X * Ntv.Z,
                Tv.Y * Ntv.X + Bv.Y * Ntv.Y + nv.Y * Ntv.Z,
                Tv.Z * Ntv.X + Bv.Z * Ntv.Y + nv.Z * Ntv.Z);
            var length = Math.Sqrt(nv.X * nv.X + nv.Y * nv.Y + nv.Z * nv.Z);
            nv.X /= length;
            nv.Y /= length;
            nv.Z /= length;

            return nv;
        }

        public static Color getVertexColor(Vertex v, ObjShape os, Color c, NormalVector nv)
        {
            var xL = os.lightSource.X - v.X;
            var yL = os.lightSource.Y - v.Y;
            var zL = os.lightSource.Z - v.Z;
            var len = Math.Sqrt(xL*xL + yL*yL + zL*zL);

            NormalVector L = new NormalVector(xL / len, yL / len, zL / len);
            var cosNL = nv.X * L.X + nv.Y * L.Y + nv.Z * L.Z;

            if (cosNL < 0) cosNL = 0;

            var V = new NormalVector(0, 0, 1);
            var R = new NormalVector(2 * cosNL * nv.X -L.X,2 * cosNL * nv.Y -L.Y, 2 * cosNL * nv.Z - L.Z); // I change
            var lenR = Math.Sqrt(R.X*R.X + R.Y * R.Y + R.Z*R.Z);
            var cosVR = R.Z / lenR;
            
            if (cosVR < 0) cosVR = 0;
            var Red = ((os.Il.R * c.R/ 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
            var bRed = (byte)Red;
            if(Red > 255) bRed = 255;
            var Green = ((os.Il.G * c.G / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
            var bGreen = (byte)Green;
            if (Green > 255) bGreen = 255;
            var Blue = ((os.Il.B * c.B / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
            var bBlue = (byte)Blue;
            if (Blue > 255) bBlue = 255;
            return Color.FromRgb(bRed, bGreen, bBlue);
        }

        public static Color getVertexColor(Vertex v, ObjShape os, NormalVector nv)
        {
            return getVertexColor(v, os, os.Io, nv);
        }

        public static Color getVertexColor(Vertex v, ObjShape os, Color c)
        {
            return getVertexColor(v, os, c, v.normalVector);
        }

        public static Color getVertexColor(Vertex v, ObjShape os)
        {
            return getVertexColor(v, os, os.Io, v.normalVector);
        }
    }
}
