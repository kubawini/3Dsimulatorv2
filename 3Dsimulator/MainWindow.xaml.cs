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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

using _3Dsimulator.Classes;
using _3Dsimulator.Converters;

namespace _3Dsimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppState appState = new AppState();

        public MainWindow()
        {
            InitializeComponent();

            // Bindings
            var pathBinding = new Binding("FilePath");
            pathBinding.Source = appState;
            selectedPathTextbox.SetBinding(TextBox.TextProperty, pathBinding);
            var pathBindingEnabledButton = new Binding("FilePath");

            pathBindingEnabledButton.Source = appState;
            pathBindingEnabledButton.Converter = new FileChosenConverter();
            openSimulatorButton.SetBinding(Button.IsEnabledProperty, pathBindingEnabledButton);
        }

        private void selectPathButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources"));
            ofd.Filter = "Obj files (*.obj)|*.obj";
            ofd.Title = "Wybieranie pliku *.obj";
            ofd.ShowDialog();
            appState.FilePath = ofd.FileName;
        }

        private void openSimulatorButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new SimulatorWindow(appState);
            Application.Current.MainWindow = newWindow;
            this.Close();
            newWindow.ShowDialog();      
        }

        private void sphereButton_Click(object sender, RoutedEventArgs e)
        {
            appState.FilePath = "../../../Resources/Sphere.obj";
        }
    }
}
