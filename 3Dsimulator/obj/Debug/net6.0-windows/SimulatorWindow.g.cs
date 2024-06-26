﻿#pragma checksum "..\..\..\SimulatorWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A401E528658C27BDFD8A0D6C16EDB47017177C59"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using _3Dsimulator;


namespace _3Dsimulator {
    
    
    /// <summary>
    /// SimulatorWindow
    /// </summary>
    public partial class SimulatorWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 60 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton equalInterpolation;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton colorInterpolation;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton vectorInterpolation;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox fogCheckBox;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox dayNightCheckbox;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox vibrationsCheckbox;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton camera1RadioButton;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton camera2RadioButton;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton camera3RadioButton;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider lightSlider;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button startButton;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\SimulatorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image shapeDisplay;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.13.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/3Dsimulator;component/simulatorwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SimulatorWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.13.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\SimulatorWindow.xaml"
            ((_3Dsimulator.SimulatorWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\SimulatorWindow.xaml"
            ((_3Dsimulator.SimulatorWindow)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.equalInterpolation = ((System.Windows.Controls.RadioButton)(target));
            
            #line 60 "..\..\..\SimulatorWindow.xaml"
            this.equalInterpolation.Checked += new System.Windows.RoutedEventHandler(this.equalInterpolation_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.colorInterpolation = ((System.Windows.Controls.RadioButton)(target));
            
            #line 61 "..\..\..\SimulatorWindow.xaml"
            this.colorInterpolation.Checked += new System.Windows.RoutedEventHandler(this.colorInterpolation_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.vectorInterpolation = ((System.Windows.Controls.RadioButton)(target));
            
            #line 62 "..\..\..\SimulatorWindow.xaml"
            this.vectorInterpolation.Checked += new System.Windows.RoutedEventHandler(this.vectorInterpolation_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.fogCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 64 "..\..\..\SimulatorWindow.xaml"
            this.fogCheckBox.Click += new System.Windows.RoutedEventHandler(this.fogCheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.dayNightCheckbox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 65 "..\..\..\SimulatorWindow.xaml"
            this.dayNightCheckbox.Click += new System.Windows.RoutedEventHandler(this.dayNightCheckbox_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.vibrationsCheckbox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.camera1RadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 68 "..\..\..\SimulatorWindow.xaml"
            this.camera1RadioButton.Checked += new System.Windows.RoutedEventHandler(this.camera1RadioButton_Checked);
            
            #line default
            #line hidden
            
            #line 68 "..\..\..\SimulatorWindow.xaml"
            this.camera1RadioButton.Click += new System.Windows.RoutedEventHandler(this.camera1RadioButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.camera2RadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 69 "..\..\..\SimulatorWindow.xaml"
            this.camera2RadioButton.Checked += new System.Windows.RoutedEventHandler(this.camera2RadioButton_Checked);
            
            #line default
            #line hidden
            
            #line 69 "..\..\..\SimulatorWindow.xaml"
            this.camera2RadioButton.Click += new System.Windows.RoutedEventHandler(this.camera2RadioButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.camera3RadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 70 "..\..\..\SimulatorWindow.xaml"
            this.camera3RadioButton.Checked += new System.Windows.RoutedEventHandler(this.camera3RadioButton_Checked);
            
            #line default
            #line hidden
            
            #line 70 "..\..\..\SimulatorWindow.xaml"
            this.camera3RadioButton.Click += new System.Windows.RoutedEventHandler(this.camera3RadioButton_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.lightSlider = ((System.Windows.Controls.Slider)(target));
            return;
            case 12:
            this.startButton = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\..\SimulatorWindow.xaml"
            this.startButton.Click += new System.Windows.RoutedEventHandler(this.startButton_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.shapeDisplay = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

