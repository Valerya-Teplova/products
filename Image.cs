using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace products
{
    class Image : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           try
            {
                string path = Path.GetFullPath(((string)value).Remove(0, 1));
                return File.Exists(path) ? path : "Resource/Image/picture.png";
            } catch(Exception)
            {
                return "Resource/Image/picture.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
