using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

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
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
