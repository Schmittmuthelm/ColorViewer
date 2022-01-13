using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WpfColorView.Converters
{
    public sealed class SelectedColorInfoToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? ((MainWindow)Application.Current.MainWindow).ColorInfos?.FirstOrDefault(x => x == value).SampleBrush : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}