using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace products
{
    /// <summary>
    /// Логика взаимодействия для AddProd.xaml
    /// </summary>
    public partial class AddProd : Window
    {
        private Products product;
        public AddProd( Products product)
        {
            InitializeComponent();

            DataContext = product;

            catCmB.ItemsSource = DBClass.GetContext().Category.ToList();
            ingredCmB.ItemsSource = DBClass.GetContext().Ingredient.ToList();
        }

        private void exitBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
