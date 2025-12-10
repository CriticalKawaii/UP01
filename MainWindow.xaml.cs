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
using UP01.Pages;

namespace UP01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame?.Navigate(new AuthorizationPage());

            if (MainFrame.CanGoBack)
                ButtonBack.Visibility = Visibility.Visible;
            else
                ButtonBack.Visibility = Visibility.Hidden;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame?.GoBack();
            else
                MessageBox.Show("Нет окна для возврата", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainFrame.CanGoBack)
                ButtonBack.Visibility = Visibility.Visible;
            else
                ButtonBack.Visibility = Visibility.Hidden;
        }
    }
}
