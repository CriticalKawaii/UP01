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

namespace UP01.Pages
{
    /// <summary>
    /// Interaction logic for ExploreITemsAdminPage.xaml
    /// </summary>
    public partial class ExploreITemsAdminPage : Page
    {
        private IQueryable<dynamic> allItems;

        public ExploreITemsAdminPage(String name)
        {
            InitializeComponent();
            LoadProducts();
            LoadCategories();
            TitelTextBox.Text = name;
        }

        private void LoadProducts()
        {
            try
            {
                allItems = DBEntities.GetContext().Items.Select(i => new
                {
                    i.ID,
                    i.Article,
                    i.Types.Type,
                    i.Units.Unit,
                    i.Price,
                    i.Suppliers.Supplier,
                    i.Manufacturers.Manufacturer,
                    i.Categories.Category,
                    i.Stock,
                    i.Discount,
                    i.Description,
                    i.Photo
                });
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = DBEntities.GetContext().Categories.Select(c => new { c.ID, c.Category }).ToList();
                CategoryFilterComboBox.Items.Clear();
                CategoryFilterComboBox.Items.Add(new { ID = 0, Category = "Все категории" });
                foreach (var category in categories)
                {
                    CategoryFilterComboBox.Items.Add(category);
                }
                CategoryFilterComboBox.DisplayMemberPath = "Category";
                CategoryFilterComboBox.SelectedIndex = 0;   
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке категорий: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilters()
        {
            try
            {
                var filteredItems = allItems.AsEnumerable();

                if (!string.IsNullOrEmpty(SearchTextBox.Text))
                {
                    var searchText = SearchTextBox.Text.ToLower();
                    filteredItems = filteredItems.Where(x => x.Type.ToLower().Contains(searchText) || x.Description.ToLower().Contains(searchText));
                }

                if(CategoryFilterComboBox.SelectedItem != null && CategoryFilterComboBox.SelectedIndex > 0)
                {
                    var selectedCategory = (dynamic)CategoryFilterComboBox.SelectedItem;
                    filteredItems = filteredItems.Where(x => x.Category == selectedCategory.Category);
                }

                if (SortComboBox.SelectedItem != null)
                {
                    switch (SortComboBox.SelectedIndex)
                    {
                        case 0: // По названию (А-Я)
                            filteredItems = filteredItems.OrderBy(p => p.Type);
                            break;
                        case 1: // По названию (Я-А)
                            filteredItems = filteredItems.OrderByDescending(i => i.Type);
                            break;
                        case 2: // По цене (возрастание)
                            filteredItems = filteredItems.OrderBy(i => i.Price);
                            break;
                        case 3: // По цене (убывание)
                            filteredItems = filteredItems.OrderByDescending(i => i.Price);
                            break;
                        case 4: // По количеству
                            filteredItems = filteredItems.OrderByDescending(i => i.Stock);
                            break;
                    }
                }

                var finalProducts = filteredItems
                   .Select(i => new
                   {
                       i.ID,
                       i.Description,
                       i.Category,
                       i.Type,
                       i.Manufacturer,
                       i.Supplier,
                       i.Unit,
                       i.Stock,
                       i.Discount,
                       Price = i.Price + " ₽",
                       Photo = string.IsNullOrWhiteSpace(i.Photo) ? null : $"/Resources/{i.Photo}",
                   })
                   .ToList();

                ProductsListView.ItemsSource = finalProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при применении фильтров: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var itemId = (int)button.Tag;

            var result = MessageBox.Show($"Вы уверены, что хотите удалить товар ID: {itemId}?",
                                       "Подтверждение удаления",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var context = DBEntities.GetContext();
                    var item = context.Items.FirstOrDefault(i => i.ID == itemId);
                    if(item != null)
                    {
                        context.Items.Remove(item);
                        context.SaveChanges();
                        LoadProducts();
                        MessageBox.Show("Товар успешно удален", "Успех",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
