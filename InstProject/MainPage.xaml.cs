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

namespace InstProject
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public MainPage()
        {
            //foreach (UIElement el in GridPrincipal.Children)
            //{
            //    if (el is Button)
            //    {
            //        ((Button)el).Click += Button_Click;
            //    }
            //}

       
            InitializeComponent();
       
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           string str=(string)((Button)e.OriginalSource).Name;
           if (str == "postBut")
           {
               GridPrincipal.Children.Clear();
               GridPrincipal.Children.Add(new PostWindow());
            }
           else if (str == "accBut")
           {
               GridPrincipal.Children.Clear();
               GridPrincipal.Children.Add(new ProfileWindow());

            }
           else if (str== "searchBut")
           {

               GridPrincipal.Children.Clear();
               GridPrincipal.Children.Add(new FindWindow());
            }
        }

        private void CLOSE(object sender, RoutedEventArgs e)
        {
            UserService services = new UserService();
            services.UpdateDate();
            LoginPage main = new LoginPage();

            main.Show();
            Hide();
        }
    }
}
