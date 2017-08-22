using HomeSnailHome.Model;
using System;
using Windows.UI.Xaml.Data;

namespace HomeSnailHome.Converter
{
    public class SubjectValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int subject = (int)value;

            if (subject == 0)   return "aucun";
            else                return subject;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
