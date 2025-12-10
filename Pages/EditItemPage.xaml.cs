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
    /// Interaction logic for EditItemPage.xaml
    /// </summary>
    public partial class EditItemPage : Page
    {
        private Items _currentItem = new Items();
        private bool _isNewItem = false;

        public EditItemPage(Items currentItem)
        {
            InitializeComponent();

            if (currentItem != null)
            {
                _currentItem = currentItem;
                _isNewItem = false;
            }
            else
            {
                _currentItem = new Items();
                _isNewItem = true;
            }

            DataContext = _currentItem;
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            try
            {
                var context = DBEntities.GetContext();

                var types = context.Types.ToList();
                cmbName.ItemsSource = types;
                cmbName.DisplayMemberPath = "Type";
                cmbName.SelectedValuePath = "ID";
                if (!_isNewItem)
                    cmbName.SelectedValue = _currentItem.TypeID;

                var categories = context.Categories.ToList();
                cmbCategory.ItemsSource = categories;
                cmbCategory.DisplayMemberPath = "Category";
                cmbCategory.SelectedValuePath = "ID";
                if (!_isNewItem)
                    cmbCategory.SelectedValue = _currentItem.CategoryID;

                var manufacturers = context.Manufacturers.ToList();
                cmbManufacturer.ItemsSource = manufacturers;
                cmbManufacturer.DisplayMemberPath = "Manufacturer";
                cmbManufacturer.SelectedValuePath = "ID";
                if (!_isNewItem)
                    cmbManufacturer.SelectedValue = _currentItem.ManufacturerID;

                var suppliers = context.Suppliers.ToList();
                cmbSupplier.ItemsSource = suppliers;
                cmbSupplier.DisplayMemberPath = "Supplier";
                cmbSupplier.SelectedValuePath = "ID";
                if (!_isNewItem)
                    cmbSupplier.SelectedValue = _currentItem.SupplierID;

                var units = context.Units.ToList();
                cmbUnit.ItemsSource = units;
                cmbUnit.DisplayMemberPath = "Unit";
                cmbUnit.SelectedValuePath = "ID";
                if (!_isNewItem)
                    cmbUnit.SelectedValue = _currentItem.UnitID;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация обязательных полей
                if (string.IsNullOrWhiteSpace(txtArticle.Text))
                {
                    MessageBox.Show("Введите артикул товара", "Предупреждение",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtArticle.Focus();
                    return;
                }

                if (cmbName.SelectedItem == null)
                {
                    MessageBox.Show("Выберите наименование товара", "Предупреждение",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbName.Focus();
                    return;
                }

                if (cmbCategory.SelectedItem == null)
                {
                    MessageBox.Show("Выберите категорию товара", "Предупреждение",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbCategory.Focus();
                    return;
                }

                if (cmbManufacturer.SelectedItem == null)
                {
                    MessageBox.Show("Выберите производителя", "Предупреждение",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbManufacturer.Focus();
                    return;
                }

                if (cmbSupplier.SelectedItem == null)
                {
                    MessageBox.Show("Выберите поставщика", "Предупреждение",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbSupplier.Focus();
                    return;
                }

                if (cmbUnit.SelectedItem == null)
                {
                    MessageBox.Show("Выберите единицу измерения", "Предупреждение",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbUnit.Focus();
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Введите корректную цену (число больше или равно 0)", "Предупреждение",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPrice.Focus();
                    return;
                }

                if (!decimal.TryParse(txtDiscount.Text, out decimal discount) || discount < 0 || discount > 100)
                {
                    MessageBox.Show("Введите корректную скидку (от 0 до 100)", "Предупреждение",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtDiscount.Focus();
                    return;
                }

                if (!int.TryParse(txtQuantity.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("Введите корректное количество на складе (целое число >= 0)", "Предупреждение",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtQuantity.Focus();
                    return;
                }

                _currentItem.Article = txtArticle.Text.Trim();
                _currentItem.TypeID = (int)cmbName.SelectedValue;
                _currentItem.CategoryID = (int)cmbCategory.SelectedValue;
                _currentItem.ManufacturerID = (int)cmbManufacturer.SelectedValue;
                _currentItem.SupplierID = (int)cmbSupplier.SelectedValue;
                _currentItem.UnitID = (int)cmbUnit.SelectedValue;
                _currentItem.Price = price;
                _currentItem.Discount = discount;
                _currentItem.Stock = stock;
                _currentItem.Description = txtDescription.Text.Trim();
                _currentItem.Photo = txtPhoto.Text.Trim();

                var context = DBEntities.GetContext();

                if (_isNewItem)
                {
                    context.Items.Add(_currentItem);
                }

                context.SaveChanges();

                MessageBox.Show("Товар успешно сохранен!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении товара: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
