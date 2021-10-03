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
    /// Interaction logic for AddCommentWindow.xaml
    /// </summary>
    public partial class AddCommentWindow : Window
    {
        private Post post;
        private CommentService COMServices;
        public AddCommentWindow(Post _post)
        {
            InitializeComponent();
            post = new Post();
            COMServices = new CommentService();
          
            post = _post;
        }
        private void CLOSE(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ADD(object sender, RoutedEventArgs e)
        {
            COMServices.AddComment(Text.Text, post.Id);
            this.Close();

        }
    }
}
