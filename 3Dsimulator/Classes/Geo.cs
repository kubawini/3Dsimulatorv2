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

        public static Color getVertexColor(Vertex v, ObjShape os, Color c, NormalVector nv, AppState appState)
        {
            double Red = 0;
            double Green = 0;
            double Blue = 0;
            byte bRed = 0;
            byte bGreen = 0;
            byte bBlue = 0;
            
            foreach (var ls in os.lightSources)
            {
                var xL = ls.X - v.X;
                var yL = ls.Y - v.Y;
                var zL = ls.Z - v.Z;
                var len = Math.Sqrt(xL * xL + yL * yL + zL * zL);

                NormalVector L = new NormalVector(xL / len, yL / len, zL / len);
                var cosNL = nv.X * L.X + nv.Y * L.Y + nv.Z * L.Z;

                if (cosNL < 0) cosNL = 0;

                var V = new NormalVector(0, 0, 1);
                //var vx = appState.XC - v.X;
                //var vy = appState.YC - v.Y;
                //var vz = appState.ZC - v.Z;
                //var norm = vx*vx+ vy*vy + vz*vz;


                //var V = new NormalVector(vx / norm, vy / norm, vz / norm);
                var R = new NormalVector(2 * cosNL * nv.X - L.X, 2 * cosNL * nv.Y - L.Y, 2 * cosNL * nv.Z - L.Z); // I change
                var lenR = Math.Sqrt(R.X * R.X + R.Y * R.Y + R.Z * R.Z);
                var cosVR = R.Z / lenR;
                //var cosVR = v.X * R.X + v.Y * L.Y + v.Z * R.Z;

                if (cosVR < 0) cosVR = 0;
                Red += ((os.Il.R * c.R / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
                if (Red > 255) Red = 255;
                Green += ((os.Il.G * c.G / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
                if (Green > 255) Green = 255;
                Blue += ((os.Il.B * c.B / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
                if (Blue > 255) Blue = 255;
            }

            // Moving light section
            if(((appState.lightVector * (v-appState.lightVertex))/((v-appState.lightVertex).Length*appState.lightVector.Length))
                >=0)
            {
                var xL = appState.lightX - v.X;
                var yL = appState.lightY - v.Y;
                var zL = appState.lightZ - v.Z;
                var len = Math.Sqrt(xL * xL + yL * yL + zL * zL);

                NormalVector L = new NormalVector(xL / len, yL / len, zL / len);
                var cosNL = nv.X * L.X + nv.Y * L.Y + nv.Z * L.Z;

                if (cosNL < 0) cosNL = 0;

                var V = new NormalVector(0, 0, 1);
                //var vx = appState.XC - v.X;
                //var vy = appState.YC - v.Y;
                //var vz = appState.ZC - v.Z;
                //var norm = vx*vx+ vy*vy + vz*vz;


                //var V = new NormalVector(vx / norm, vy / norm, vz / norm);
                var R = new NormalVector(2 * cosNL * nv.X - L.X, 2 * cosNL * nv.Y - L.Y, 2 * cosNL * nv.Z - L.Z); // I change
                var lenR = Math.Sqrt(R.X * R.X + R.Y * R.Y + R.Z * R.Z);
                var cosVR = R.Z / lenR;
                //var cosVR = v.X * R.X + v.Y * L.Y + v.Z * R.Z;

                if (cosVR < 0) cosVR = 0;
                Red += ((os.Il.R * c.R / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
                if (Red > 255) Red = 255;
                Green += ((os.Il.G * c.G / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
                if (Green > 255) Green = 255;
                Blue += ((os.Il.B * c.B / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
                if (Blue > 255) Blue = 255;
            }

            bRed = (byte)Red;
            bGreen = (byte)Green;
            bBlue = (byte)Blue;
            return Color.FromRgb(bRed, bGreen, bBlue);
        }

        public static Color getVertexColor(Vertex v, ObjShape os, NormalVector nv, AppState appState)
        {
            return getVertexColor(v, os, os.Io, nv, appState);
        }

        public static Color getVertexColor(Vertex v, ObjShape os, Color c, AppState appState)
        {
            return getVertexColor(v, os, c, v.normalVector, appState);
        }

        public static Color getVertexColor(Vertex v, ObjShape os, AppState appState)
        {
            return getVertexColor(v, os, os.Io, v.normalVector, appState);
        }
    }
}
