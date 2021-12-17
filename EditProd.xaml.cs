using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
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
    /// Логика взаимодействия для EditProd.xaml
    /// </summary>
    public partial class EditProd : Window
    {
        public Products products { get; set; }
        private ImageHelp imageHelp { get; set; }

        class ImageHelp : INotifyPropertyChanged
        {
            public Products Products { get; set; }
            public string Image
            {
                get => Products.Image;
                set
                {
                    Products.Image = value;
                    OnPropertyChanged("Image");
                }
            }
            private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            public event PropertyChangedEventHandler PropertyChanged;
        }
        public EditProd(Products product)
        {
            InitializeComponent();
           
            DataContext = products;           
            categCmB.ItemsSource = DBClass.GetContext().Category.ToList();
            UpdateIngredientList();
            imageHelp = new ImageHelp() { Products = product };

        }
     
        private void UpdateIngredientList()
        {
            ingredCmB.ItemsSource = DBClass.GetContext().Ingredient.ToList().Except(products.ProductsIngredient.Select(p => p.Ingredient)).ToList(); //туть ошибка((
            ingredListView.ItemsSource = null;
            ingredListView.ItemsSource = products.ProductsIngredient;
        }

        private void exitBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void editBut_Click(object sender, RoutedEventArgs e)
        {
            DBClass.GetContext().SaveChanges();
            DBClass.ApplyDataBaseChange();
            MessageBox.Show("Информация успешно сохранена.");
            Close();
        }

        private void addIngredBut_Click(object sender, RoutedEventArgs e)
        {
            Ingredient ingredient = ingredCmB.SelectedItem as Ingredient;
            if (ingredient != null)
            {
                DBClass.GetContext().ProductsIngredient.Add(new ProductsIngredient()
                {
                    Ingredient = ingredient,
                    Products = products
                });
                UpdateIngredientList();
            }
        }

        private void removeIngredBut_Click(object sender, RoutedEventArgs e)
        {
            if(ingredListView.SelectedItem != null)
            {
                products.ProductsIngredient.Remove(ingredListView.SelectedItem as ProductsIngredient);
                UpdateIngredientList();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void editphotoBut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files| *.jpg; *.jpeg; *.png;";
            if(openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string path = "products/" + DateTime.Now.ToBinary().ToString() +
                        System.IO.Path.GetExtension(openFileDialog.FileName);
                    File.Copy(openFileDialog.FileName, System.IO.Path.GetFullPath(path));
                    MessageBox.Show(path);
                    imageHelp.Image = "/" + path;
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }    
}
