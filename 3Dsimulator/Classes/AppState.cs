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

        private string texturePath = ""; // To add default
        public string TexturePath { get { return texturePath; } set { texturePath = value; OnPropertyChanged();} }

        private bool textureEnabled = false;
        public bool TextureEnabled { get { return textureEnabled; } set { textureEnabled = value; OnPropertyChanged(); } }

        public WriteableBitmap Texture = null; // To add default


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void openImage()
        {
            Texture = new WriteableBitmap(new BitmapImage(new Uri(texturePath)));
        }
    }
}
