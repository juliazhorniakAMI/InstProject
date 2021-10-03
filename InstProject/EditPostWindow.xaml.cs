using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
using MongoDB.Bson;

namespace InstProject
{
    /// <summary>
    /// Interaction logic for EditPostWindow.xaml
    /// </summary>
    public partial class EditPostWindow : Window
    {
        private Post post;
        private PostServices postServices;

        public EditPostWindow(Post _post)
        {
            InitializeComponent();
            post=new Post();
            postServices = new PostServices();
            Text.Text = _post.Description;
            post = _post;
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            postServices.EditPost(Text.Text,post.Id);
            this.Close();
        }

        private void CLOSE(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
