using System;
using System.Globalization;
using System.Windows.Data;

namespace HttpLauncher.Utils
{
    [ValueConversion(typeof(Version), typeof(string))]
    class VersionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "";

            Version version = value as Version;
            if (parameter != null)
            {
                int precision = int.Parse(parameter as string, CultureInfo.InvariantCulture);
                return version.ToString(precision);
            }
            else
            {
                return version.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
