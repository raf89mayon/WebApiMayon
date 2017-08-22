using HomeSnailHome.Model;
using System;
using Windows.UI.Xaml.Data;

namespace HomeSnailHome.Converter
{
    public class StringAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var housing = (Housing)value;
            var returnString = housing.Number;
            if (housing.PostBox != 0) returnString += " " + housing.PostBox;
            return returnString + " " + housing.Street + "\n" + housing.ZipCode + " " + housing.City;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
