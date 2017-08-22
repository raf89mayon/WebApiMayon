using HomeSnailHome.Model;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace HomeSnailHome.Converter
{
    public class HousingOptionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var option = (Boolean)value;

            if (option) return new BitmapImage(new Uri("ms-appx:/Images/ok.png"));
            else        return new BitmapImage(new Uri("ms-appx:/Images/no.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
