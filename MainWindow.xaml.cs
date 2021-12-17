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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace products
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int currentPage;
        int countPage;
        int countElements;
        public SortElement SortEl { get; set; }
        ICollection<Products> products;
        Category categoryAll;
        List<Category> filterList = new List<Category>();
        List<SortElement> sortElements;
       

        public MainWindow()
        {
            InitializeComponent();

            categoryAll = new Category();
            categoryAll.TitleCat = "Все категории";
            sortElements = new List<SortElement>()
            {
                new SortElement(){Id = 1, Name = "Наименование"},
                new SortElement(){Id = 2, Name = "Стоимость"},
                new SortElement(){Id = 3, Name = "Вес"}
            };
            sortCmB.ItemsSource = sortElements;

            List<Category> filterCategory = new List<Category>();
            filterCategory.Add(categoryAll);
            filterCategory.AddRange(DBClass.GetContext().Category.ToList());
            filterCmB.ItemsSource = filterCategory;
            MyDB();

        }

        public void MyDB()
        {
            
            wrapPanel.Children.Clear();
            products = DBClass.GetContext().Products.ToList();
            

            //Поиск по наименованию продукта или по его описанию
            if (!String.IsNullOrEmpty(searchTxB.Text.Trim()))
                products = products.Where(p => p.TitleProd.Contains(searchTxB.Text) ||
                (String.IsNullOrEmpty(p.Description) &&
                p.Description.Contains(searchTxB.Text))).ToList();

            if (filterList.Count > 0)
                products = products.Where(p => filterList.Contains(p.Category)).ToList();

            //Сортировка
            if (SortEl != null)
            {
                switch (SortEl.Id)
                {
                    case 1:
                        if (SortEl.Direction)
                            products = products.OrderBy(p => p.TitleProd).ToList();
                        else products = products.OrderByDescending(p => p.TitleProd).ToList();
                        break;
                    case 2:
                        if (SortEl.Direction)
                            products = products.OrderBy(p => p.Price).ToList();
                        else products = products.OrderByDescending(p => p.Price).ToList();
                        break;
                    case 3:
                        if (SortEl.Direction)
                            products = products.OrderBy(p => p.Weigth).ToList();
                        else products = products.OrderByDescending(p => p.Weigth).ToList();
                        break;
                }
            }
            countElements = products.Count;

            //Переход по страницам, если элементов больше 20
            if (countElements > 20)
            {
                navigationStackPanel.Visibility = Visibility.Visible;
                countPage = countElements / 20;
                for (int i = 0; i < countPage; i++)
                {
                    Button btn = new Button();
                    btn.Content = (i + 1).ToString();
                    btn.Tag = i + 1;
                    btn.Click += nextBtn_Click;
                    wrapPanel.Children.Add(btn);
                }
                currentPage = 1;
                MyDB();
            }
            else
            {
                navigationStackPanel.Visibility = Visibility.Hidden;
                listbox.ItemsSource = products;
            }
        }

        
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (((Category)checkBox.DataContext) != categoryAll)
                filterList.Add((Category)checkBox.DataContext);
            else
            {
                filterList.Clear();
                filterCmB.ItemsSource = null;
                List<Category> filterCategory = new List<Category>();
                filterCategory.Add(categoryAll);
                filterCategory.AddRange(DBClass.GetContext().Category.ToList());
                filterCmB.ItemsSource = filterCategory;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            filterList.Remove((Category)checkBox.DataContext);
        }

        private void filterCmB_DropDownClosed(object sender, EventArgs e)
        {
            MyDB();
        }

        private void searchTxB_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyDB();
        }

        private void sortCmB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SortWin sortWin = new SortWin((SortElement)comboBox.SelectedItem);
            sortWin.Owner = this;
            sortWin.Show();
        }

        //Метод для обновления страниц
        public void UpdatePage()
        {
            int index = 0;
            Products[] pr = new Products[20];
            if (currentPage != 1)
                index = ((currentPage - 1) * 20) - 1;
            products.ToList().CopyTo(index, pr, 0, 20);
            listbox.ItemsSource = pr;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage != 1)
            {
                currentPage -= 1;
                UpdatePage();
            }
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            currentPage = (int)(sender as Button).Tag;
            UpdatePage();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void edProd_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            EditProd editProd = new EditProd((Products)button.Tag);
            editProd.Owner = this;
            editProd.Show();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            AddProd addProd = new AddProd((Products)but.Tag);
            addProd.Owner = this;
            addProd.Show();
        }

        private void listbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        { }       

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                Products product = (sender as StackPanel).DataContext as Products;
                EditProd editProd = new EditProd(product);
                editProd.Owner = this;
                editProd.Show();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
