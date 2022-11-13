using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;

namespace _3Dsimulator.Classes
{
    public class StringTOColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Color)value == Colors.Red) return "Czerwony";
            if ((Color)value == Colors.Blue) return "Niebieski";
            if ((Color)value == Colors.Green) return "Zielony";
            if ((Color)value == Colors.Purple) return "Fioletowy";
            if ((Color)value == Colors.Yellow) return "Żółty";
            if ((Color)value == Colors.Pink) return "Różowy";
            if ((Color)value == Colors.Cyan) return "Cyjan";
            if ((Color)value == Colors.White) return "Biały";
            if ((Color)value == Colors.Black) return "Czarny";
            return "Szary";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Czerwony") return Colors.Red;
            if ((string)value == "Niebieski") return Colors.Blue;
            if ((string)value == "Zielony") return Colors.Green;
            if ((string)value == "Fioletowy") return Colors.Purple;
            if ((string)value == "Żółty") return Colors.Yellow;
            if ((string)value == "Różowy") return Colors.Pink;
            if ((string)value == "Cyjan") return Colors.Cyan;
            if ((string)value == "Biały") return Colors.White;
            if ((string)value == "Czarny") return Colors.Black;
            return Colors.Gray;
        }
    }
}
