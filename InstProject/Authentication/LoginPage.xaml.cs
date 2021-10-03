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
using InstProject.BLL.Services;
using InstProject.DAL.Models;

namespace InstProject
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        private UserService userservice;
        public LoginPage()
        {
            InitializeComponent();
            userservice = new UserService();
        }

        private void LoginUser(object sender, RoutedEventArgs e)
        {
            string nick = NickLogin.Text.Trim();
            string pass = PasswordLogin.Password.Trim();
            if (userservice.CheckIfUserExists(nick,pass))
            {
                PasswordLogin.ToolTip = "";
                PasswordLogin.Background = Brushes.Transparent;
                User u = userservice.GetUserByNick(nick);
                userservice.WriteCurrentId(u.Id);
                MainPage main = new MainPage()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
               
              
                main.ShowDialog();
                this.Close();
            }
            else
            {
                PasswordLogin.ToolTip = "This field is not correct!";
                PasswordLogin.Background = Brushes.DarkRed;
            }

        }

        private void RegisterUser(object sender, RoutedEventArgs e)
        {
            RegisterPage main = new RegisterPage();
         
            main.Show();
            Hide();

        }
    }
}
