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
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBoxPassword.Password))
            {
                MessageBox.Show("Введите логин и пароль", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using(var db = new DBEntities())
            {
                var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Login.Trim() == TextBoxLogin.Text.Trim() && u.Password.Trim() == PasswordBoxPassword.Password.Trim());
                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден");

                }
                else
                {
                    MessageBox.Show($"Добро пожаловать, {user.FirstName}!");

                    var mainWindow = Application.Current.MainWindow as MainWindow;

                    if (mainWindow != null)
                    {
                        if (user.RoleID == 1) 
                        {
                            mainWindow.MainFrame.Navigate(new ExploreITemsAdminPage($"{user.LastName} {user.FirstName} {user.PaternityName}"));
                        }
                        else if (user.RoleID == 2) 
                        {
                            mainWindow.MainFrame.Navigate(new ExploreItemsPage());
                        }
                        else
                        {
                            mainWindow.MainFrame.Navigate(new ExploreItemsPage());
                        }
                    }
                }
            }
        }

        private void ButtonGuest_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new ExploreItemsPage());
        }
    }
}
