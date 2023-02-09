using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Media;
using System.IO;


namespace _3Dsimulator.Classes
{
    public class AppState : INotifyPropertyChanged
    {
        private string filePath = "";
        public string FilePath { get { return filePath; } set { filePath = value;  OnPropertyChanged(); } }

        public float translate = 0;

        private double time = 0;
        public double Time { get { return time; } set { time = value; } }

        //private List<string> colorsString = new List<string> { "Czerwony", "Niebieski", "Zielony", "Fioletowy", "Żółty", "Różowy", "Cyjan", "Biały", "Czarny", "Szary" };
        //public List<string> ColorsString { get { return colorsString; } set { colorsString = value; } }

        //private bool gridEnabled = true;
        //public bool GridEnabled { get { return gridEnabled; } set { gridEnabled = value; } }

        private bool colorInterpolation = true;
        public bool ColorInterpolation { get { return colorInterpolation; } set { colorInterpolation = value; OnPropertyChanged(); } }

        private bool equalInterpolation = false;
        public bool EqualInterpolation { get { return equalInterpolation; } set { equalInterpolation = value; OnPropertyChanged(); } }
        //private string texturePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources\texture.png")); // To add default
        //public string TexturePath { get { return texturePath; } set { texturePath = value; OnPropertyChanged();} }

        //private bool textureEnabled = false;
        //public bool TextureEnabled { get { return textureEnabled; } set { textureEnabled = value; OnPropertyChanged(); } }

        //private string normalMapPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources\MapaNormalna.png")); // To add default
        //public string NormalMapPath { get { return normalMapPath; } set { normalMapPath = value; OnPropertyChanged(); } }

        //private bool normalMapEnabled = false;
        //public bool NormalMapEnabled { get { return normalMapEnabled; } set { normalMapEnabled = value; OnPropertyChanged(); } }

        //private bool cloudAllowed = false;
        //public bool CloudAllowed { get { return cloudAllowed; } set { cloudAllowed = value; OnPropertyChanged();} }

        private bool paintingAllowed = true;
        public bool PaintingAllowed { get { return paintingAllowed; } set { paintingAllowed = value; OnPropertyChanged(); } }

        //private string nextObjPath = "";
        //public string NextObjPath { get { return nextObjPath; } set { nextObjPath = value; OnPropertyChanged(); } }

        private bool rotatingAllowed = true;
        public bool RotatingAllowed { get { return rotatingAllowed; } set { rotatingAllowed = value; OnPropertyChanged(); } }

        public float kat = 0;
        public float kat2 = 0;


        private float xC = 0.3f;
        public float XC { get { return xC; } set { xC = value; OnPropertyChanged();} }

        private float yC = 0.5f;
        public float YC { get { return yC; } set { yC = value; OnPropertyChanged(); } }

        private float zC = 0.2f;
        public float ZC { get { return zC; } set { zC = value; OnPropertyChanged(); } }

        private float fov = (float)(Math.PI / 2);
        public float Fov { get { return fov; } set { fov = value; OnPropertyChanged(); } }

        public WriteableBitmap Texture = null; // To add default
        public System.Windows.Media.Color[,] TextureColors;

        private int camera = 1;
        public int Camera { get { return camera; } set { camera = value; OnPropertyChanged(); } }

        private bool vibrations = false;
        public bool Vibrations { get { return vibrations; } set { vibrations = value; OnPropertyChanged();} }

        private bool fog = false;
        public bool Fog { get { return fog; } set { fog = value; OnPropertyChanged();} }

        private bool night = false;
        public bool Night { get { return night; } set { night = value; OnPropertyChanged(); } }

        public WriteableBitmap NormalMap = null; // To add default

        public float fogWspolczynnik = 0;
        public float nocWspolczynnik = 1;

        public float fogSign = 1;
        public float nocSign = 1;

        private float lightDirection = 0;
        public float LightDirection { get { return lightDirection; } set { lightDirection = value; OnPropertyChanged();} }

        public float lightX { get { return 0 ; } }
        public float lightY { get { return translate + (float)Math.Cos(kat) / 4; } }
        public float lightZ { get { return (float)Math.Sin(kat)*(-1); } }
        public Vertex lightVertex { 
            get 
            { 
                var v = new Vertex(lightX, lightY, lightZ);
                v.X -= 300;
                return v;
            } 
        }
        public NormalVector lightVector { get { return new NormalVector(0, Math.Cos(kat), -Math.Sin(kat)); } }
        public float Beta = 1.3f;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //public void openImage()
        //{
        //    Texture = new WriteableBitmap(new BitmapImage(new Uri(texturePath)));
        //    TextureColors = new System.Windows.Media.Color[Texture.PixelWidth + 1, Texture.PixelHeight + 1];
        //    for (int x = 0; x <= Texture.PixelWidth; x++)
        //    {
        //        for (int y = 0; y <= Texture.PixelHeight; y++)
        //        {
        //            TextureColors[x, y] = Texture.GetPixel(x, y);
        //        }
        //    }  
        //}

        //public void openNormalMap()
        //{
        //    NormalMap = new WriteableBitmap(new BitmapImage(new Uri(normalMapPath)));
        //}
    }
}
