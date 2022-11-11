using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
