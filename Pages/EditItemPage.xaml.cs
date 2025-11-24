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

        public EditItemPage(Items currentItem)
        {
            InitializeComponent();
            if(currentItem != null)
                _currentItem = currentItem;

            DataContext = _currentItem;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
