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
using MongoDB.Bson;

namespace InstProject
{
    /// <summary>
    /// Interaction logic for PersonPage.xaml
    /// </summary>
    public partial class PersonPage : Window
    {
        ObjectId id;
      
        PostServices postServices;
        UserService services;
        User user;
        List<Post> posts;
        bool isAnyPosts = false;
        bool tempLike = false;
        bool tempDislike = false;
        Post currentPost;
        int indexOfPost = 0;
        CommentService comservices;
        public PersonPage(ObjectId _id)
        {
            InitializeComponent();

            id = _id;
         
            services = new UserService();
            postServices = new PostServices();
            comservices = new CommentService();
       
            //
            user = new User();
            user = services.GetUser(id);
            btnConnection.Content = services.GetShortestPathNumber(user.Nick);
            if (btnConnection.Content.ToString() == " ")
            {
                btnConnection.Content = "No connection";
            }
            Nam.Content = user.FirstName;
            Surname.Content = user.LastName;
            NickName.Content = user.Nick;

            currentPost = new Post();
            posts = new List<Post>();
            posts = postServices.GetPosts(user.Id).OrderByDescending(x=>x.Date).ToList();
            if (posts != null && posts.Count > 0)
            {
                currentPost = posts[indexOfPost];
                Main.Content = currentPost.Description;
                isAnyPosts = true;
            }
            else
            {
                Main.Content = "No posts yet";
              

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
          
            textDisLike.Text = postServices.GetDislikes(currentPost.Id).ToString();
            textLike.Text = postServices.GetLikes(currentPost.Id).ToString();


        }

        private void Follow(object sender, RoutedEventArgs e)
        {
            User currUser = services.GetUser();
            if (!services.CheckAlreadyFollow(currUser.Id, user.Nick))
            {

                services.AddFollowing(currUser.Nick, user.Nick);
                services.AddFollower(user.Nick, currUser.Nick);
                buttonFollow.Background = Brushes.Gray;
                buttonUnfollow.Background = Brushes.DarkSlateBlue;
            }
            else
            {

                MessageBox.Show("you already follow this user");
            }
            btnConnection.Content = services.GetShortestPathNumber(user.Nick);
        }

        private void Unfollow(object sender, RoutedEventArgs e)
        {
            User currUser = services.GetUser();
            if (services.CheckAlreadyFollow(currUser.Id, user.Nick))
            {

                services.UnFollow(currUser.Nick, user.Nick);
           
                buttonUnfollow.Background = Brushes.Gray;
                buttonFollow.Background = Brushes.DarkSlateBlue;
            }
        
            else
            {

                MessageBox.Show("you already unfollow this user");
            }

            btnConnection.Content = services.GetShortestPathNumber(user.Nick);
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
            if (comservices.GetPersonsWhoDisLiked(currentPost.Id) != null || comservices.GetPersonsWhoLiked(currentPost.Id) != null)
            {
                ListOfCommentsWindow main = new ListOfCommentsWindow(currentPost.Id)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                main.ShowDialog();
            }


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
            posts = postServices.GetPosts(user.Id).OrderByDescending(x => x.Date).ToList();
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

            posts = postServices.GetPosts(user.Id).OrderByDescending(x => x.Date).ToList();
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



        private void FollowingInfo(object sender, RoutedEventArgs e)
        {
            PersonsList main = new PersonsList(services.GetFollowing(user.Id))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();

        }

        private void FollowersInfo(object sender, RoutedEventArgs e)
        {
            PersonsList main = new PersonsList(services.GetFollowers(user.Id))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();

        }
    }
}
