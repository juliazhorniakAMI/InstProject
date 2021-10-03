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
    /// Interaction logic for AddPostWindow.xaml
    /// </summary>
    public partial class AddPostWindow : Window
    {
        PostServices services;
        public AddPostWindow()
        {
            InitializeComponent();
            services = new PostServices();
        }


        private void CLOSE(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ADD(object sender, RoutedEventArgs e)
        {
            services.InsertPost(Text.Text);
            this.Close();
        }
    }
}
