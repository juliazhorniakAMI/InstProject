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
    /// Interaction logic for FindWindow.xaml
    /// </summary>
    public partial class FindWindow : UserControl
    {
    
        private UserService s;
      
        public FindWindow()
        {
            InitializeComponent();
            s = new UserService();

        }
     
        private void Click(object sender, RoutedEventArgs e)
        {
            List<string> users = new List<string>();
            List<User> userList = new List<User>();
            userList = s.GetUsers().ToList();


            foreach (var x in userList)
            {
                users.Add(x.Nick);
            }
            List<string> usersnew = users.Where(x => x.StartsWith(PersonNick.Text)).ToList();
          

            if (usersnew.Count>0)
            {
                PersonsList main = new PersonsList(usersnew)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                main.Show();
       
            }
            else
            {
              MessageBox.Show("No such user");
              
            }
        }
    }
}
