using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace products
{
    class ListIngredient : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = null;
            ICollection<ProductsIngredient> productsIngredients = value as ICollection<ProductsIngredient>;
            foreach(ProductsIngredient productsIngredient in productsIngredients)
            {
                str += productsIngredient.Ingredient.TitleIngred + ",\n ";
            }
            return str?.Remove(str.Length - 3, 3);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
