using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TicketingApp.Converters
{
    public class WebNavigatingEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var webNavigatingEventArgs = value as WebNavigatingEventArgs;
            if(webNavigatingEventArgs == null)
            {
                throw new ArgumentException("Expected value to be of type WebNavigatingEventArgs", nameof(value));
            }
            return webNavigatingEventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
