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
            StringBuilder error = new StringBuilder();

            if (prNameTB.SelectedText == null)
                error.AppendLine("Вы не заполнили Наименование продукта");
            if (catCmB.SelectedItem == null)
                error.AppendLine("Вы не выбрали категорию продукта");
            if (desTB.SelectedText == null)
                error.AppendLine("Вы не вели Описание продукта");
            if (weightTB.SelectedText == null)
                error.AppendLine("Вы не вели вес продукта");
            if (priceTB.SelectedText == null)
                error.AppendLine("Вы не указали стоимость продукта");
            if (ingredCmB.SelectedItem == null)
               error.AppendLine("Вы не выбрали ингредиент");

            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }
            Products products = new Products()
            {
                TitleProd = prNameTB.Text,
                Description = descrTxB.Text,
                Weigth = double.Parse(weightTB.Text),
                Price = double.Parse(priceTB.Text),
                Image = null,
                Category = catCmB.SelectedItem as Category
            };
            DBClass.GetContext().Products.Add(products);
            ProductsIngredient productsIngredient = new ProductsIngredient()
            {
                Products = products,
                Ingredient = ingredCmB.SelectedItem as Ingredient
            };
            DBClass.GetContext().ProductsIngredient.Add(productsIngredient);


            DBClass.GetContext().SaveChanges();
            DBClass.ApplyDataBaseChange();
            MessageBox.Show("Продукт успешно добавлен");
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}