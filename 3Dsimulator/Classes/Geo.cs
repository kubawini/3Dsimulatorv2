using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

        public static int y2h(double y)
        {
            return (int)(height/2 * (y + 1));
        }

        public static int z2p(double y)
        {
            return (int)(height / 2 * y);
        }

        public static Color getVertexColor(Vertex v, Vertex LightSource, ObjShape os)
        {
            var xL = LightSource.X - v.X;
            var yL = LightSource.Y - v.Y;
            var zL = LightSource.Z - v.Z;
            var len = Math.Sqrt(xL*xL + yL*yL + zL*zL);

            NormalVector L = new NormalVector(xL / len, yL / len, zL / len);
            var cosNL = v.normalVector.X * L.X + v.normalVector.Y * L.Y + v.normalVector.Z * L.Z;
            
            var V = new NormalVector(0, 0, 1);
            var R = new NormalVector(-L.X, -L.Y, 2 * cosNL - L.Z);
            var lenR = Math.Sqrt(R.X*R.X + R.Y * R.Y + R.Z*R.Z);
            var cosVR = R.Z / lenR;
            if (cosNL < 0) cosNL = 0;
            if (cosVR < 0) cosVR = 0;
            var Red = ((os.Il.R * os.Io.R/ 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
            var bRed = (byte)Red;
            if(Red > 255) bRed = 255;
            var Green = ((os.Il.G * os.Io.G / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
            var bGreen = (byte)Green;
            if (Green > 255) bGreen = 255;
            var Blue = ((os.Il.B * os.Io.B / 255) * (os.kd * cosNL + os.ks * Math.Pow(cosVR, os.m)));
            var bBlue = (byte)Blue;
            if (Blue > 255) bBlue = 255;
            return Color.FromRgb(bRed, bGreen, bBlue);
        }
    }
}
