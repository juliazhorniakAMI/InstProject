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
using InstProject.DAL.Repositories;

namespace InstProject
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : UserControl
    {
        int indexOfPost = 0;
        Post currentPost;
        private bool tempDislike = false;
        private bool tempLike = false;
         private bool isAnyPosts = true;
        PostServices postServices;
        private User user;
     
        List<Post> posts;
        List<Post> tmpposts;
        UserService services;
        CommentService comservices;
        public ProfileWindow()
        {
            InitializeComponent();
            currentPost = new Post();
            postServices = new PostServices();
            services = new UserService();
            comservices = new CommentService();
            posts = new List<Post>();
            user = services.GetUser();
            Nam.Content = user.FirstName;
            Surname.Content = user.LastName;
            NickName.Content = user.Nick;
            posts = postServices.GetPosts().OrderByDescending(x=>x.Date).ToList();
          
            if (posts != null && posts.Count > 0)
            {
                currentPost = posts[indexOfPost];
                Main.Content = currentPost.Description;
                isAnyPosts = true;
            }
            else
            {
                Main.Content = "No posts yet";
                Main.Background = Brushes.Yellow;
            }
            if (postServices.CheckIfUserDisLikePost(services.ReadCurrentId(), currentPost.Id))
            {
                tempDislike = true;
            }
            if (postServices.CheckIfUserLikePost(services.ReadCurrentId(), currentPost.Id))
            {
            
                tempLike = true;
            }

            textDisLike.Text = postServices.GetDislikes(currentPost.Id).ToString();
            textLike.Text = postServices.GetLikes(currentPost.Id).ToString();
        }

        private void AddComment(object sender, RoutedEventArgs e)
        {
            if (isAnyPosts)
            {
                AddCommentWindow main = new AddCommentWindow(currentPost)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                main.ShowDialog();
            }

        }

        private void AmountOfDislikes(object sender, RoutedEventArgs e)
        {

            if (currentPost != null)
            {
                if (tempDislike == false)
                {

                    tempDislike = true;
                    postServices.AddDislike(services.ReadCurrentId(), currentPost.Id);
                    if (tempLike == true && textLike.Text != "0")
                    {
                        tempLike = false;
                        postServices.DismissLike(services.ReadCurrentId(), currentPost.Id);
                        textLike.Text = postServices.GetLikes(currentPost.Id).ToString();

                    }


                    textDisLike.Text = postServices.GetDislikes(currentPost.Id).ToString();

                }
                else
                {

                    tempDislike = false;
                    postServices.DismissDisLike(services.ReadCurrentId(), currentPost.Id);
                    textDisLike.Text = postServices.GetDislikes(currentPost.Id).ToString();

                }

            }
            else
            {

                MessageBox.Show("write a post at first");
            }

        }

        private void AmountOfLikes(object sender, RoutedEventArgs e)
        {

            if (currentPost != null)
            {
                if (tempLike == false)
                {

                    tempLike = true;
                    postServices.AddLike(services.ReadCurrentId(), currentPost.Id);
                    if (tempDislike == true && textDisLike.Text != "0")
                    {
                        tempDislike = false;
                        postServices.DismissDisLike(services.ReadCurrentId(), currentPost.Id);
                        textDisLike.Text = postServices.GetDislikes(currentPost.Id).ToString();
                    }

                    textLike.Text = postServices.GetLikes(currentPost.Id).ToString();

                }
                else
                {
                    tempLike = false;

                    postServices.DismissLike(services.ReadCurrentId(), currentPost.Id);
                    textLike.Text = postServices.GetLikes(currentPost.Id).ToString();
                }

            }
            else
            {

                MessageBox.Show("write a post at first");
            }


        }


        private void WhoComment(object sender, RoutedEventArgs e)
        {
            /*postServices.GetPersonWhoComment(currentPost.Id)*/
            ListOfCommentsWindow main = new ListOfCommentsWindow( currentPost.Id)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();

        }

        private void WhoLike(object sender, RoutedEventArgs e)
        {
            PersonsList main = new PersonsList(postServices.GetPersonsWhoLiked(currentPost.Id))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void WhoDislike(object sender, RoutedEventArgs e)
        {
            PersonsList main = new PersonsList(postServices.GetPersonsWhoDisLiked(currentPost.Id))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();

        }

      
        private void PreviousPost(object sender, RoutedEventArgs e)
        {
            posts = postServices.GetPosts().OrderByDescending(x => x.Date).ToList();
            if (isAnyPosts)
            {
                if (indexOfPost > 0)
                {
                    indexOfPost--;
                    currentPost = posts[indexOfPost];
                    Main.Content = currentPost.Description;
                    if (postServices.CheckIfUserDisLikePost(services.ReadCurrentId(), currentPost.Id))
                    {
                      
                        tempDislike = true;
                    }
                    else
                    {
                      
                        tempDislike = false;
                    }
                    if (postServices.CheckIfUserLikePost(services.ReadCurrentId(), currentPost.Id))
                    {
                       
                        tempLike = true;
                    }
                    else
                    {
                    
                        tempLike = false;
                    }

                }
                else
                {
                    currentPost = posts[indexOfPost];
                    Main.Content = currentPost.Description;
                    if (postServices.CheckIfUserDisLikePost(services.ReadCurrentId(), currentPost.Id))
                    {

                        tempDislike = true;
                    }
                    else
                    {

                        tempDislike = false;
                    }
                    if (postServices.CheckIfUserLikePost(services.ReadCurrentId(), currentPost.Id))
                    {

                        tempLike = true;
                    }
                    else
                    {

                        tempLike = false;
                    }

                }
                textDisLike.Text = postServices.GetDislikes(currentPost.Id).ToString();
                textLike.Text = postServices.GetLikes(currentPost.Id).ToString();
            }
            

                else
                {

                    MessageBox.Show("No posts there");
                }
            

        }

        private void NextPost(object sender, RoutedEventArgs e)
        {

            posts = postServices.GetPosts().OrderByDescending(x => x.Date).ToList();
            if (isAnyPosts)
            {
                indexOfPost++;
                if (indexOfPost < posts.Count)
                {
                    currentPost = posts[indexOfPost];
                    Main.Content = currentPost.Description;
                    if (postServices.CheckIfUserDisLikePost(services.ReadCurrentId(), currentPost.Id))
                    {

                        tempDislike = true;
                    }
                    else
                    {

                        tempDislike = false;
                    }
                    if (postServices.CheckIfUserLikePost(services.ReadCurrentId(), currentPost.Id))
                    {

                        tempLike = true;
                    }
                    else
                    {

                        tempLike = false;
                    }


                }
                else
                {
                 
                    indexOfPost--;
                    currentPost = posts[indexOfPost];
                    Main.Content = currentPost.Description;
                    if (postServices.CheckIfUserDisLikePost(services.ReadCurrentId(), currentPost.Id))
                    {

                        tempDislike = true;
                    }
                    else
                    {

                        tempDislike = false;
                    }
                    if (postServices.CheckIfUserLikePost(services.ReadCurrentId(), currentPost.Id))
                    {

                        tempLike = true;
                    }
                    else
                    {

                        tempLike = false;
                    }


                }
                textDisLike.Text = postServices.GetDislikes(currentPost.Id).ToString();
                textLike.Text = postServices.GetLikes(currentPost.Id).ToString();
            }
            else
            {

                MessageBox.Show("No posts there");
            }
        }

        private void AddNewPost(object sender, RoutedEventArgs e)
        {
            AddPostWindow window = new AddPostWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
        }

        private void EditPost(object sender, RoutedEventArgs e)
        {

            if (isAnyPosts)
            {
                EditPostWindow window = new EditPostWindow(currentPost);
                window.Show();
            }
        }

        private void Refresh(object sender, RoutedEventArgs e)

        { posts=postServices.GetPosts().OrderByDescending(x => x.Date).ToList();
            if (posts != null && posts.Count > 0)
            {
                currentPost = posts[indexOfPost];
                Main.Content = currentPost.Description;
                isAnyPosts = true;
            }
            else
            {
                Main.Content = "no posts yet";
            }
        }

        private void DeletePost(object sender, RoutedEventArgs e)
        {
            if (isAnyPosts)
            {
                bool t = postServices.DeletePost(currentPost.Id);
               
            }
        }

        private void ViewFollowing(object sender, RoutedEventArgs e)
        {
            PersonsList main = new PersonsList(services.GetFollowing(services.ReadCurrentId()))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void ViewFollowers(object sender, RoutedEventArgs e)
        {
            PersonsList main = new PersonsList(services.GetFollowers(services.ReadCurrentId()))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }
    }
}
