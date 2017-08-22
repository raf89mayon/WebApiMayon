using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace HomeSnailHome.Converter
{
    public class UserPictureToImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var link = (string)value;
            if (link != null && link != "")
                return new BitmapImage(new Uri(link));
            else
                return new BitmapImage(new Uri("ms-appx:/Images/user_def.png"));

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
