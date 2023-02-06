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
using Microsoft.Win32;
using System.IO;


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
        MainWindow mainWindow;

        //int spiralClickCounter = 0;
        //DispatcherTimer dispatcherTimer = new DispatcherTimer();
        //DispatcherTimer chmurkaTimer = new DispatcherTimer();
        DispatcherTimer rotateTimer = new DispatcherTimer();
        DispatcherTimer fogTimer = new DispatcherTimer();
        DispatcherTimer nocTimer = new DispatcherTimer();
       
        Face cloud = new Face();
        int roadInt = 1;
        //int tickerCounter = 0;
        //double cloudMove = 10;


        public SimulatorWindow(AppState aS)
        {
            InitializeComponent();
            appState = aS;
            objShape = ObjReader.read("../../../Resources/RealTorus.obj");
            shapeDisplay.BeginInit();
            drawer = new BitmapDrawer(objShape, shapeDisplay, appState, cloud);
            drawer.normalizeTorus(objShape);
            drawer.add4spheres();
            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);

            cloud.AddVertex(new Vertex(0.1, -0.1, 1.5));
            cloud.AddVertex(new Vertex(0.2, 0, 1.5));
            cloud.AddVertex(new Vertex(0.1, 0.1, 1.5));
            cloud.AddVertex(new Vertex(-0.1, 0.1, 1.5));
            cloud.AddVertex(new Vertex(-0.2, 0, 1.5));
            cloud.AddVertex(new Vertex(-0.1, -0.1, 1.5));
            cloud.CreateEdges();
            cloud.setAET();

            //chmurkaTimer.Tick += new EventHandler(chmurkaTimer_Tick);
            //chmurkaTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);
            //chmurkaTimer.Start();

            rotateTimer.Tick += new EventHandler(rotateTimer_Tick);
            rotateTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);


            fogTimer.Tick += new EventHandler(fogTimer_Tick);
            fogTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);

            nocTimer.Tick += new EventHandler(nocTimer_Tick);
            nocTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);

            // Bindings
            //var kdBinding = new Binding("kd");
            //kdBinding.Source = objShape;
            //kdBinding.Mode = BindingMode.TwoWay;
            //kdSlider.SetBinding(Slider.ValueProperty, kdBinding);

            //var ksBinding = new Binding("ks");
            //ksBinding.Source = objShape;
            //ksBinding.Mode = BindingMode.TwoWay;
            //ksSlider.SetBinding(Slider.ValueProperty, ksBinding);

            //var mBinding = new Binding("m");
            //mBinding.Source = objShape;
            //mBinding.Mode = BindingMode.TwoWay;
            //mSlider.SetBinding(Slider.ValueProperty, mBinding);

            //var zBinding = new Binding("z");
            //zBinding.Source = objShape;
            //zBinding.Mode = BindingMode.TwoWay;
            //zSlider.SetBinding(Slider.ValueProperty, zBinding);

            //var oColorBinding = new Binding("ColorsString");
            //oColorBinding.Source = appState;
            //objectColor.SetBinding(ComboBox.ItemsSourceProperty, oColorBinding);
            //lightColor.SetBinding(ComboBox.ItemsSourceProperty, oColorBinding);

            //var IoBinding = new Binding("Io");
            //var IlBinding = new Binding("Il");
            //IoBinding.Source = objShape;
            //IlBinding.Source = objShape;
            //IoBinding.Mode = BindingMode.TwoWay;
            //IlBinding.Mode = BindingMode.TwoWay;
            //IoBinding.Converter = new StringTOColorConverter();
            //IlBinding.Converter = new StringTOColorConverter();
            //objectColor.SetBinding(ComboBox.SelectedItemProperty, IoBinding);
            //lightColor.SetBinding(ComboBox.SelectedItemProperty, IlBinding);

            //var gridBinding = new Binding("GridEnabled");
            //gridBinding.Source = appState;
            //gridBinding.Mode = BindingMode.TwoWay;
            //gridCheckBox.SetBinding(CheckBox.IsCheckedProperty, gridBinding);

            var interpolationBinding = new Binding("ColorInterpolation");
            interpolationBinding.Source = appState;
            interpolationBinding.Mode = BindingMode.TwoWay;
            colorInterpolation.SetBinding(RadioButton.IsCheckedProperty, interpolationBinding);

            var equalInterpolationBinding = new Binding("EqualInterpolation");
            equalInterpolationBinding.Source = appState;
            equalInterpolationBinding.Mode = BindingMode.TwoWay;
            equalInterpolation.SetBinding(RadioButton.IsCheckedProperty, equalInterpolationBinding);

            var vibrationsBinding = new Binding("Vibrations");
            vibrationsBinding.Source = appState;
            vibrationsBinding.Mode = BindingMode.TwoWay;
            vibrationsCheckbox.SetBinding(CheckBox.IsCheckedProperty, vibrationsBinding);

            //var fogBinding = new Binding("Fog");
            //fogBinding.Source = appState;
            //fogBinding.Mode = BindingMode.TwoWay;
            //fogCheckBox.SetBinding(CheckBox.IsCheckedProperty, fogBinding);
            appState.Fog = true;

            //var nightBinding = new Binding("Night");
            //nightBinding.Source = appState;
            //nightBinding.Mode = BindingMode.TwoWay;
            //dayNightCheckbox.SetBinding(CheckBox.IsCheckedProperty, nightBinding);
            appState.Night = true;

            //var textureBinding = new Binding("TextureEnabled");
            //textureBinding.Source = appState;
            //textureBinding.Mode = BindingMode.TwoWay;
            //useTextureCheckbox.SetBinding(CheckBox.IsCheckedProperty, textureBinding);

            //var normalMapBinding = new Binding("NormalMapEnabled");
            //normalMapBinding.Source = appState;
            //normalMapBinding.Mode = BindingMode.TwoWay;
            //useNormalMapCheckbox.SetBinding(CheckBox.IsCheckedProperty, normalMapBinding);

            //var cloudBinding = new Binding("CloudAllowed");
            //cloudBinding.Source = appState;
            //cloudBinding.Mode = BindingMode.TwoWay;
            //allowCloud.SetBinding(CheckBox.IsCheckedProperty, cloudBinding);

            //var paintingBinding = new Binding("PaintingAllowed");
            //paintingBinding.Source = appState;
            //paintingBinding.Mode = BindingMode.TwoWay;
            //allowPaintingCheckbox.SetBinding(CheckBox.IsCheckedProperty, paintingBinding);

            //var rotatingBinding = new Binding("RotatingAllowed");
            //rotatingBinding.Source = appState;
            //rotatingBinding.Mode = BindingMode.TwoWay;
            //rotateCheckbox.SetBinding(CheckBox.IsCheckedProperty, rotatingBinding);

            //var fovBinding = new Binding("Fov");
            //fovBinding.Source = appState;
            //fovBinding.Mode = BindingMode.TwoWay;
            //FOVslider.SetBinding(Slider.ValueProperty, fovBinding);

            //var xcBinding = new Binding("XC");
            //xcBinding.Source = appState;
            //xcBinding.Mode = BindingMode.TwoWay;
            //xCameraSlider.SetBinding(Slider.ValueProperty, xcBinding);

            //var ycBinding = new Binding("YC");
            //ycBinding.Source = appState;
            //ycBinding.Mode = BindingMode.TwoWay;
            //yCameraSlider.SetBinding(Slider.ValueProperty, ycBinding);

            //var zcBinding = new Binding("ZC");
            //zcBinding.Source = appState;
            //zcBinding.Mode = BindingMode.TwoWay;
            //zCameraSlider.SetBinding(Slider.ValueProperty, zcBinding);
        }

        private void kdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => drawer.draw();
        private void ksSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => drawer.draw();
        private void mSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => drawer.draw();
        private void zSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {if(drawer != null) drawer.draw();}
        private void objectColor_SelectionChanged(object sender, SelectionChangedEventArgs e) => drawer.draw();
        private void vectorInterpolation_Checked(object sender, RoutedEventArgs e) => drawer?.draw();
        private void gridCheckBox_Checked(object sender, RoutedEventArgs e) => drawer.draw();
        private void gridCheckBox_Unchecked(object sender, RoutedEventArgs e) => drawer.draw();
        private void colorInterpolation_Checked(object sender, RoutedEventArgs e) => drawer.draw();
        private void allowCloud_Click(object sender, RoutedEventArgs e) => drawer.draw();
        private void allowPaintingCheckbox_Click(object sender, RoutedEventArgs e) => drawer.draw();

        private void fogTimer_Tick(object sender, EventArgs e)
        {
            appState.fogWspolczynnik += 0.1f * appState.fogSign;
            
            if(appState.fogWspolczynnik > 1 || appState.fogWspolczynnik < 0) fogTimer.Stop();
            if (appState.fogWspolczynnik < 0) appState.fogWspolczynnik = 0;
            drawer.draw();
        }

        private void nocTimer_Tick(object sender, EventArgs e)
        {
            appState.nocWspolczynnik -= appState.nocSign * 0.1f;
            if(appState.nocWspolczynnik < 0.3f)
            {
                nocTimer.Stop();
            }
            if(appState.nocWspolczynnik > 1)
            {
                appState.nocWspolczynnik = 1;
                nocTimer.Stop();
            }
            drawer.draw();
        }
            private void rotateTimer_Tick(object sender, EventArgs e)
        {
            appState.kat += 0.1f;
            appState.translate += 0.02f * roadInt;
            if (Math.Abs(appState.translate) >= 0.4)
                roadInt *= -1;
            drawer.draw();
        }

        //private void startTimerButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (spiralClickCounter % 2 == 0)
        //    {
        //        dispatcherTimer.Start();
        //        startTimerButton.Content = "Zatrzymaj spiralę";
        //    }
        //    else
        //    {
        //        dispatcherTimer.Stop();
        //        startTimerButton.Content = "Startuj spiralę";
        //    }
        //    spiralClickCounter++;
        //}

        //private void chmurkaTimer_Tick(object sender, EventArgs e)
        //{
        //    if(appState.CloudAllowed)
        //    {
        //        tickerCounter = (tickerCounter + 1) % 10;
        //        if (tickerCounter == 0) cloudMove *= -1;
        //        cloud.MoveCloudHorizontally(cloudMove);
        //        drawer.draw();
        //    }      
        //}

        //private void dispatcherTimer_Tick(object sender, EventArgs e)
        //{

        //    drawer.bitmap.FillEllipse((int)objShape.lightSource.X - 5, (int)objShape.lightSource.Y - 5,
        //        (int)objShape.lightSource.X + 5, (int)objShape.lightSource.Y + 5, Colors.White);
        //    appState.Time += 0.1;
        //    objShape.r += 0.01;
        //    if (objShape.r > 1.5) objShape.r = 0;
        //    objShape.lightSource.X = Geo.x2w(Math.Cos(appState.Time) * objShape.r);
        //    objShape.lightSource.Y = Geo.y2h(Math.Sin(appState.Time) * objShape.r);



        //    drawer.draw();
        //}

        //private void loadTextureButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var ofd = new OpenFileDialog();
        //    ofd.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources"));
        //    ofd.Filter = "Png files (*.png)|*.png|Jpg files (*.jpg)|*.jpg";
        //    ofd.Title = "Wybieranie pliku *.png lub *.jpg";
        //    ofd.ShowDialog();
        //    if(ofd.FileName!="")
        //        appState.TexturePath = ofd.FileName;
        //}

        //private void useTextureCheckbox_Click(object sender, RoutedEventArgs e)
        //{
        //    if (useTextureCheckbox.IsChecked == true)
        //    {
        //        appState.openImage();
        //        appState.TextureEnabled = true;
        //    }
        //    else 
        //    {
        //        appState.TextureEnabled = false;
        //    }
        //    drawer.draw();
        //}

        //private void loadNormalMapButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var ofd = new OpenFileDialog();
        //    ofd.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources"));
        //    ofd.Filter = "Png files (*.png)|*.png|Jpg files (*.jpg)|*.jpg";
        //    ofd.Title = "Wybieranie pliku *.png lub *.jpg";
        //    ofd.ShowDialog();
        //    if (ofd.FileName != "")
        //        appState.NormalMapPath = ofd.FileName;
        //}

        //private void useNormalMapCheckbox_Click(object sender, RoutedEventArgs e)
        //{
        //    if (useNormalMapCheckbox.IsChecked == true)
        //    {
        //        appState.openNormalMap();
        //        appState.NormalMapEnabled = true;
        //    }
        //    else
        //    {
        //        appState.NormalMapEnabled = false;
        //    }
        //    drawer.draw();
        //}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //mainWindow = new MainWindow();
            //Application.Current.MainWindow = mainWindow;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //mainWindow.ShowDialog();
        }

        //private void addNextObj_Click(object sender, RoutedEventArgs e)
        //{
        //    var ofd = new OpenFileDialog();
        //    ofd.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources"));
        //    ofd.Filter = "Png files (*.obj)|*.obj";
        //    ofd.Title = "Wybieranie pliku *.obj";
        //    ofd.ShowDialog();
        //    if (ofd.FileName != "")
        //    {
        //        appState.NormalMapPath = ofd.FileName;
        //        drawer.addNewObj(ofd.FileName, kdSlider, ksSlider, mSlider);
        //        drawer.draw();
        //    }
        //}

        //private void rotateCheckbox_Checked(object sender, RoutedEventArgs e)
        //{
            
        //}

        private void equalInterpolation_Checked(object sender, RoutedEventArgs e)
        {
            drawer.draw();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (startCounter % 2 == 0)
            {
                rotateTimer.Start();
                startButton.Content = "Stop";
            }
            else
            {
                rotateTimer.Stop();
                startButton.Content = "Start";
            }
            startCounter++;
        }

        int startCounter = 0;

        private void camera1RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void camera2RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void camera3RadioButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void camera1RadioButton_Click(object sender, RoutedEventArgs e)
        {
            appState.Camera = 1;
            appState.XC = 0.3f;
            appState.YC = 0.5f;
            appState.ZC = 0.2f;
            drawer.draw();
        }

        private void camera2RadioButton_Click(object sender, RoutedEventArgs e)
        {
            appState.Camera = 2;
            appState.XC = 0;
            appState.YC = 0.5f;
            appState.ZC = 0.2f;
            drawer.draw();
        }

        private void camera3RadioButton_Click(object sender, RoutedEventArgs e)
        {
            appState.Camera = 3;
            appState.XC = 0;
            appState.YC = appState.translate - 0.5f;
            appState.ZC = 1;
            drawer.draw();
        }

        private void fogCheckBox_Click(object sender, RoutedEventArgs e)
        {
            fogTimer.Start();
            if (fogCheckBox.IsChecked == true) appState.fogSign = 1;
            else appState.fogSign = -1;
            drawer.draw();
        }

        private void dayNightCheckbox_Click(object sender, RoutedEventArgs e)
        {
            nocTimer.Start();
            if (dayNightCheckbox.IsChecked == true) appState.nocSign = 1;
            else appState.nocSign= -1;
            drawer.draw();
        }
    }
}
