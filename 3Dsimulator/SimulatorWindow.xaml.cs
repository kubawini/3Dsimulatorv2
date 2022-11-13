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
using System.Windows.Threading;
using System.Collections.ObjectModel;

using _3Dsimulator.Classes;

namespace _3Dsimulator
{
    /// <summary>
    /// Logika interakcji dla klasy SimulatorWindow.xaml
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        AppState appState;
        ObjShape objShape;
        BitmapDrawer drawer;

        int spiralClickCounter = 0;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        

        public SimulatorWindow(AppState aS)
        {
            InitializeComponent();
            appState = aS;
            objShape = ObjReader.read(aS.FilePath);
            shapeDisplay.BeginInit();
            drawer = new BitmapDrawer(objShape, shapeDisplay, appState);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            //drawer.FillObject();
            //drawer.draw(new Vertex(0, 0, 1), objShape);

            // Bindings
            var kdBinding = new Binding("kd");
            kdBinding.Source = objShape;
            kdBinding.Mode = BindingMode.TwoWay;
            kdSlider.SetBinding(Slider.ValueProperty, kdBinding);

            var ksBinding = new Binding("ks");
            ksBinding.Source = objShape;
            ksBinding.Mode = BindingMode.TwoWay;
            ksSlider.SetBinding(Slider.ValueProperty, ksBinding);

            var mBinding = new Binding("m");
            mBinding.Source = objShape;
            mBinding.Mode = BindingMode.TwoWay;
            mSlider.SetBinding(Slider.ValueProperty, mBinding);

            var zBinding = new Binding("z");
            zBinding.Source = objShape;
            zBinding.Mode = BindingMode.TwoWay;
            zSlider.SetBinding(Slider.ValueProperty, zBinding);

            var oColorBinding = new Binding("ColorsString");
            oColorBinding.Source = appState;
            objectColor.SetBinding(ComboBox.ItemsSourceProperty, oColorBinding);
            lightColor.SetBinding(ComboBox.ItemsSourceProperty, oColorBinding);

            var IoBinding = new Binding("Io");
            var IlBinding = new Binding("Il");
            IoBinding.Source = objShape;
            IlBinding.Source = objShape;
            IoBinding.Mode = BindingMode.TwoWay;
            IlBinding.Mode = BindingMode.TwoWay;
            IoBinding.Converter = new StringTOColorConverter();
            IlBinding.Converter = new StringTOColorConverter();
            objectColor.SetBinding(ComboBox.SelectedItemProperty, IoBinding);
            lightColor.SetBinding(ComboBox.SelectedItemProperty, IlBinding);

            var gridBinding = new Binding("GridEnabled");
            gridBinding.Source = appState;
            gridBinding.Mode = BindingMode.TwoWay;
            gridCheckBox.SetBinding(CheckBox.IsCheckedProperty, gridBinding);
        }

        private void kdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => drawer.draw();
        private void ksSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => drawer.draw();
        private void mSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => drawer.draw();
        private void zSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {if(drawer != null) drawer.draw();}
        private void objectColor_SelectionChanged(object sender, SelectionChangedEventArgs e) => drawer.draw();

        private void startTimerButton_Click(object sender, RoutedEventArgs e)
        {
            if (spiralClickCounter % 2 == 0)
            {
                dispatcherTimer.Start();
                startTimerButton.Content = "Zatrzymaj spiralę";
            }
            else
            {
                dispatcherTimer.Stop();
                startTimerButton.Content = "Startuj spiralę";
            }
            spiralClickCounter++;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            appState.Time += 0.1;
            objShape.r += 0.1;
            objShape.lightSource.X = Geo.x2w(Math.Cos(appState.Time) * objShape.r);
            objShape.lightSource.Y = Geo.y2h(Math.Sin(appState.Time) * objShape.r);
            drawer.draw();
        }


        private void gridCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            drawer.draw();
        }

        private void gridCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            drawer.draw();
        }
    }
}
