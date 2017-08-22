using System;
using Windows.UI.Xaml.Data;

namespace HomeSnailHome.Converter
{
    public class HousingQuotationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var quotation = (float)value;
            return quotation.ToString("0.##%");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
