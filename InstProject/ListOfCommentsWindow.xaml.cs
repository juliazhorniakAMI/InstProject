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
    /// Interaction logic for ListOfCommentsWindow.xaml
    /// </summary>
    public partial class ListOfCommentsWindow : Window
    {
        private UserService service;
        private PostServices postservice;
        private CommentService comservice;
    

        bool isAnyComments = false;
 
        bool tempLike = false;
        bool tempDislike = false;
        Comment currentComment;
        private List<Comment> comments;
     
        int indexOfComment = 0;
        private ObjectId _postid;
        public ListOfCommentsWindow( ObjectId postid)
        {
            InitializeComponent();
            service = new UserService();
            postservice = new PostServices();
            comservice = new CommentService();
            currentComment = new Comment();
            comments = new List<Comment>();
            comments = comservice.GetComments(postid);
            _postid = postid;
            if (comments != null && comments.Count > 0)
            {
               
                currentComment = comments[indexOfComment];
                User user = service.GetUser(currentComment.CommentOwnerId);
                Main.Content = user.Nick + "\n" + currentComment.Description + "\t" + currentComment.Date;
                isAnyComments = true;
            }
            else
            {
                Main.Content = "No comments";



            }
            //
            if (comservice.CheckIfUserDisLikeComment(service.ReadCurrentId(), currentComment))
            {

                tempDislike = true;
            }
            if (comservice.CheckIfUserLikeComment(service.ReadCurrentId(), currentComment))
            {

                tempLike = true;
            }
            //
            textDisLike.Text = comservice.GetCommentDislike(currentComment.Id).ToString();
            textLike.Text = comservice.GetCommentLike(currentComment.Id).ToString();


        }


        private void CLOSE(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void PreviousComment(object sender, RoutedEventArgs e)
        {
            comments = comservice.GetComments(_postid);
            User user = service.GetUser(currentComment.CommentOwnerId);

            if (isAnyComments)
            {
                if (indexOfComment > 0)
                {
                    indexOfComment--;
                    user = service.GetUser(currentComment.CommentOwnerId);
                    Main.Content = user.Nick + "\n" + currentComment.Description + "\t" + currentComment.Date;
                    if (comservice.CheckIfUserDisLikeComment(service.ReadCurrentId(), currentComment))
                    {

                        tempDislike = true;
                    }
                    else
                    {
                        tempDislike = false;

                    }

                    if (comservice.CheckIfUserLikeComment(service.ReadCurrentId(), currentComment))
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
                    user = service.GetUser(currentComment.CommentOwnerId);
                    Main.Content = user.Nick + "\n" + currentComment.Description + "\t" + currentComment.Date;
                    if (comservice.CheckIfUserDisLikeComment(service.ReadCurrentId(), currentComment))
                    {

                        tempDislike = true;
                    }
                    else
                    {
                        tempDislike = false;

                    }

                    if (comservice.CheckIfUserLikeComment(service.ReadCurrentId(), currentComment))
                    {

                        tempLike = true;
                    }
                    else
                    {
                        tempLike = false;

                    }

                }
                textDisLike.Text = comservice.GetCommentDislike(currentComment.Id).ToString();
                textLike.Text = comservice.GetCommentLike(currentComment.Id).ToString();

            }

        }

        private void AmountOfDislikes(object sender, RoutedEventArgs e)
        {

            if (currentComment != null)
            {
                if (tempDislike == false)
                {
                   
                    tempDislike = true;
                    comservice.AddDislike(service.ReadCurrentId(), currentComment.Id);
                    if (tempLike == true && textLike.Text!="0")
                    {
                        tempLike = false;
                        comservice.DismissLike(service.ReadCurrentId(), currentComment.Id);
                        textLike.Text = comservice.GetCommentLike(currentComment.Id).ToString();
                    }



                    textDisLike.Text = comservice.GetCommentDislike(currentComment.Id).ToString();
                }
                else
                {

                    tempDislike = false;
                    comservice.DismissDisLike(service.ReadCurrentId(), currentComment.Id);
                    textDisLike.Text = comservice.GetCommentDislike(currentComment.Id).ToString();

                }

            }
            else
            {
                MessageBox.Show("at first write post");

            }


        }

        private void AmountOfLikes(object sender, RoutedEventArgs e)
        {



            if (currentComment != null)
            {

                if (tempLike == false)
                {

                    tempLike = true;
                    comservice.AddLike(service.ReadCurrentId(), currentComment.Id);
                    if (tempDislike == true && textDisLike.Text != "0")
                    {
                        tempDislike = false;
                        comservice.DismissDisLike(service.ReadCurrentId(), currentComment.Id);
                        textDisLike.Text = comservice.GetCommentDislike(currentComment.Id).ToString();
                    }

                    textLike.Text = comservice.GetCommentLike(currentComment.Id).ToString();
                }
                else
                {
                    tempLike = false;
                    comservice.DismissLike(service.ReadCurrentId(), currentComment.Id);
                    textLike.Text = comservice.GetCommentLike(currentComment.Id).ToString();
                }
            }
            else
            {
                MessageBox.Show("at first write post");

            }
        }


      
        private void WhoLike(object sender, RoutedEventArgs e)
        {
            PersonsList main = new PersonsList(comservice.GetPersonsWhoLiked(currentComment.Id))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void WhoDislike(object sender, RoutedEventArgs e)
        {
            PersonsList main = new PersonsList(comservice.GetPersonsWhoDisLiked(currentComment.Id))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();

        }

        private void NextPost(object sender, RoutedEventArgs e)
        {
            comments = comservice.GetComments(_postid);

            User user = service.GetUser(currentComment.CommentOwnerId);

            if (isAnyComments)
            {
                indexOfComment++;
                if (indexOfComment < comments.Count)
                {
                    currentComment = comments[indexOfComment];
                    user = service.GetUser(currentComment.CommentOwnerId);
                    Main.Content = user.Nick + "\n" + currentComment.Description + "\t" + currentComment.Date;
                    if (comservice.CheckIfUserDisLikeComment(service.ReadCurrentId(), currentComment))
                    {

                        tempDislike = true;
                    }
                    else
                    {
                        tempDislike = false;

                    }

                    if (comservice.CheckIfUserLikeComment(service.ReadCurrentId(), currentComment))
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
                    indexOfComment--;

                    currentComment = comments[indexOfComment];
                    user = service.GetUser(currentComment.CommentOwnerId);
                    Main.Content = user.Nick + "\n" + currentComment.Description + "\t"+ currentComment.Date;
                    if (comservice.CheckIfUserDisLikeComment(service.ReadCurrentId(), currentComment))
                    {

                        tempDislike = true;
                    }
                    else
                    {
                        tempDislike = false;

                    }

                    if (comservice.CheckIfUserLikeComment(service.ReadCurrentId(), currentComment))
                    {

                        tempLike = true;
                    }
                    else
                    {
                        tempLike = false;

                    }

                }
                textDisLike.Text = comservice.GetCommentDislike(currentComment.Id).ToString();
                textLike.Text = comservice.GetCommentLike(currentComment.Id).ToString();
            }
        }
      
     
    private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = service.GetUser(currentComment.CommentOwnerId);
            PersonPage info = new PersonPage(user.Id);
            info.Show();


        }
      
    }
}

