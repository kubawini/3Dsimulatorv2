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

        private double time = 0;
        public double Time { get { return time; } set { time = value; } }

        private List<string> colorsString = new List<string> { "Czerwony", "Niebieski", "Zielony", "Fioletowy", "Żółty", "Różowy", "Cyjan", "Biały", "Czarny", "Szary" };
        public List<string> ColorsString { get { return colorsString; } set { colorsString = value; } }

        private bool gridEnabled = true;
        public bool GridEnabled { get { return gridEnabled; } set { gridEnabled = value; } }

        private bool colorInterpolation = true;
        public bool ColorInterpolation { get { return colorInterpolation; } set { colorInterpolation = value; OnPropertyChanged(); } }

        private string texturePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources\texture.png")); // To add default
        public string TexturePath { get { return texturePath; } set { texturePath = value; OnPropertyChanged();} }

        private bool textureEnabled = false;
        public bool TextureEnabled { get { return textureEnabled; } set { textureEnabled = value; OnPropertyChanged(); } }

        private string normalMapPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources\MapaNormalna.png")); // To add default
        public string NormalMapPath { get { return normalMapPath; } set { normalMapPath = value; OnPropertyChanged(); } }

        private bool normalMapEnabled = false;
        public bool NormalMapEnabled { get { return normalMapEnabled; } set { normalMapEnabled = value; OnPropertyChanged(); } }

        private bool cloudAllowed = false;
        public bool CloudAllowed { get { return cloudAllowed; } set { cloudAllowed = value; OnPropertyChanged();} }

        private bool paintingAllowed = false;
        public bool PaintingAllowed { get { return paintingAllowed; } set { paintingAllowed = value; OnPropertyChanged(); } }

        private string nextObjPath = "";
        public string NextObjPath { get { return nextObjPath; } set { nextObjPath = value; OnPropertyChanged(); } }

        public WriteableBitmap Texture = null; // To add default
        public System.Windows.Media.Color[,] TextureColors;

        public WriteableBitmap NormalMap = null; // To add default



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void openImage()
        {
            Texture = new WriteableBitmap(new BitmapImage(new Uri(texturePath)));
            TextureColors = new System.Windows.Media.Color[Texture.PixelWidth + 1, Texture.PixelHeight + 1];
            for (int x = 0; x <= Texture.PixelWidth; x++)
            {
                for (int y = 0; y <= Texture.PixelHeight; y++)
                {
                    TextureColors[x, y] = Texture.GetPixel(x, y);
                }
            }  
        }

        public void openNormalMap()
        {
            NormalMap = new WriteableBitmap(new BitmapImage(new Uri(normalMapPath)));
        }
    }
}
