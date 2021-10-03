using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        private UserService userservice;
        private User user;
        public RegisterPage()
        {
            InitializeComponent();
            userservice = new UserService();
        }

    
        

        private void Register(object sender, RoutedEventArgs e)
        {

            string firstName = FirstName.Text.Trim();
            string lastName = SecName.Text.Trim();
            string nick = NickName.Text.Trim();
            string status = Status.Text.Trim();
            string  pass= Password.Password.Trim();
            string confirmPass = ConfirmPassword.Password.Trim();

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(nick))
            {
                if (userservice.CheckUniqueNick(NickName.Text))
                {
                    if (pass == confirmPass)
                    {
                           userservice.AddUser(new User()
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Gender = status,
                                Nick = nick,
                                Password = pass
                            });
                            user = userservice.GetUserByNick(nick);
                            userservice.WriteCurrentId(user.Id);
                            MainPage main = new MainPage();
                          
                            main.Show();
                            Hide();
                    }
                    else
                    {
                        MessageBox.Show("Passwords are different ");
                  
                    }
                }
                else
                {
                    MessageBox.Show("Nick already taken");
                }

            }
            else
            {
                MessageBox.Show("Enter all fields");
            
               
            }



        }
    }
}
