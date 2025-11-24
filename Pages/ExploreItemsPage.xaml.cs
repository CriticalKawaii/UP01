using System.Linq;
using System.Windows.Controls;
using System.Windows;


namespace UP01.Pages
{
    /// <summary>
    /// Interaction logic for ExploreItemsPage.xaml
    /// </summary>
    public partial class ExploreItemsPage : Page
    {
        public ExploreItemsPage()
        {
            InitializeComponent();
            LoadPhotos();
            
        }
        private void LoadPhotos()
        {
            var Items = DBEntities.GetContext().Items.ToList();

            var currentItems = Items.Select(i => new
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
            }).ToList()
            .Select(i => new
            {
                i.Category,
                i.Type,
                i.Description,
                i.Manufacturer,
                i.Supplier,
                i.Unit,
                i.Stock,
                i.Discount,
                Price = i.Price.ToString("0"),
                FinalPrice = i.Discount > 0 ? (i.Price - (i.Price*i.Discount/100)).ToString("0") : i.Price.ToString("0"),
                Photo = string.IsNullOrWhiteSpace(i.Photo) ? null : $"/Resources/{i.Photo}",
                OldPriceVisibility = (i.Discount > 0) ? Visibility.Visible : Visibility.Collapsed,
                HasBigDiscount = i.Discount > 15 
            }).ToList();

            ListViewItems.ItemsSource = currentItems;
        }
    }
}
