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
    /// Interaction logic for PostWindow.xaml
    /// </summary>
    public partial class PostWindow : UserControl
    {
       
        PostServices postServices;
        UserService services;
        User user;
        List<Post> posts;
        bool isAnyPosts = false;
        bool tempLike = false;
        bool tempDislike = false;
        Post currentPost;
        CommentService comservices;
        int indexOfPost = 0;
        public PostWindow()
        {
            InitializeComponent();

            //
            comservices = new CommentService();
            services = new UserService();
            postServices = new PostServices();
     
            //
            user = new User();
            user = services.GetUser();

            //
            currentPost = new Post();
            posts = new List<Post>();
         
           posts = postServices.GetSortedPosts(user.Id,user.Date, user.Following).OrderByDescending(x => x.Date).ToList(); ;

            if (posts != null && posts.Count > 0)
            {
                currentPost = posts[indexOfPost];
                Main.Content = currentPost.Description;
                isAnyPosts = true;
            }
            else
            {
                Main.Content = "No new posts";
          


            }
            //
            if (postServices.CheckIfUserDisLikePost(services.ReadCurrentId(), currentPost.Id))
            {
            
                tempDislike = true;
            }
            if (postServices.CheckIfUserLikePost(services.ReadCurrentId(), currentPost.Id))
            {
           
                tempLike = true;
            }
            //
            textDisLike.Text = postServices.GetDislikes(currentPost.Id).ToString();
            textLike.Text = postServices.GetLikes(currentPost.Id).ToString();

        }

        private void PreviousPost(object sender, RoutedEventArgs e)
        {
            posts = postServices.GetSortedPosts(user.Id, user.Date, user.Following).OrderByDescending(x => x.Date).ToList(); ;
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
            ListOfCommentsWindow main = new ListOfCommentsWindow(currentPost.Id)
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

        private void NextPost(object sender, RoutedEventArgs e)
        {

            posts = postServices.GetSortedPosts(user.Id, user.Date, user.Following).OrderByDescending(x => x.Date).ToList(); ;
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
        }
        private void Person(object sender, RoutedEventArgs e)
        {
            if (isAnyPosts)
            {
                PersonPage main = new PersonPage(currentPost.OwnerPostId)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                main.ShowDialog();
            }

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
    }
}
