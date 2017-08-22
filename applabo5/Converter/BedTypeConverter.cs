using HomeSnailHome.Model;
using HomeSnailHome.ViewModel;
using System;
using Windows.UI.Xaml.Data;

namespace HomeSnailHome.Converter
{
    public class BedTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bedType = (int)value;

            foreach(Bed bed in ViewModelLocator.BedsList)
                if (bedType == bed.Code)
                    return bed.Name;

            return "non communiqué";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
