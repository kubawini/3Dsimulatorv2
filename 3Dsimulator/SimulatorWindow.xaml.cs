using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using _3Dsimulator.Classes;

namespace _3Dsimulator
{
    /// <summary>
    /// Logika interakcji dla klasy SimulatorWindow.xaml
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        AppState appState;

        public SimulatorWindow(AppState aS)
        {
            InitializeComponent();
            appState = aS;
        }


    }
}
