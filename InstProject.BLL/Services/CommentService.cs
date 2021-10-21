using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstProject.DAL.Models;
using InstProject.DAL.Repositories;
using MongoDB.Bson;

namespace InstProject.BLL.Services
{
   public class CommentService
    {
        CommentRep postRep;
        UserRep userRep;
        UserService userService;

        public CommentService()
        {
            postRep = new CommentRep();

            userRep = new UserRep();
            userService = new UserService();
        }


        public bool CheckIfUserLikeComment(ObjectId id, Comment comment)
        {


            if (comment != null)
            {
                if (comment.PersonsWhoLike != null && comment.PersonsWhoLike.Count > 0)
                {
                    foreach (var nick in comment.PersonsWhoLike)
                    {
                        if (id == userService.GetUserByNick(nick).Id)
                        {
                            return true;
                        }
                    }
                }
            }



            return false;
        }
        public bool CheckIfUserDisLikeComment(ObjectId id, Comment comment)
        {


            if (comment != null)
            {
                if (comment.PersonsWhoDisLike != null && comment.PersonsWhoDisLike.Count > 0)
                {
                    foreach (var nick in comment.PersonsWhoDisLike)
                    {
                        if (id == userService.GetUserByNick(nick).Id)
                        {
                            return true;
                        }
                    }
                }
            }



            return false;
        }
        public int GetCommentLike(ObjectId id)
        {


            return postRep.GetCommentLike(id);

        }

        public int GetCommentDislike(ObjectId id)
        {
            return postRep.GetCommentDisLike(id);
        }
        public List<Comment> GetComments(ObjectId postId)
        {
            return postRep.GetComments(postId);

        }
        public bool AddComment(string text, ObjectId postId)
        {

            Comment comment = new Comment();
            comment.Description = text;
            comment.CommentOwnerId = userService.ReadCurrentId();
            comment.postId = postId;
            comment.Date = DateTime.Now.ToString();
            try
            {
                postRep.Add(comment);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public void AddDislike(ObjectId ID, ObjectId comId)
        {
            User user = userService.GetUser();
            try
            {
                postRep.AddDislike(user, comId);
            }
            catch
            {

            }

        }

        public void DismissDisLike(ObjectId ID, ObjectId comId)
        {
            User user = userService.GetUser();
            try
            {
                postRep.DismissDisLike(user, comId);
            }
            catch
            {

            }
        }

        //
        public void AddLike(ObjectId ID, ObjectId comId)
        {
            User user = userService.GetUser();
            try
            {
                postRep.AddLike(user, comId);
            }
            catch
            {

            }

        }

        public void DismissLike(ObjectId ID, ObjectId comId)
        {
            User user = userService.GetUser();
            try
            {
                postRep.DismissLike(user, comId);
            }
            catch
            {

            }
        }
        public List<string> GetPersonsWhoLiked(ObjectId postId)
        {
            List<string> ls = new List<string>();
            try
            {
                ls = postRep.GetPersonsWhoLiked(postId);
                return ls;
            }
            catch
            {
                return ls;
            }

        }

        public List<string> GetPersonsWhoDisLiked(ObjectId postId)
        {
            List<string> ls = new List<string>();
            try
            {
                ls = postRep.GetPersonsWhoDisLiked(postId);
                return ls;
            }
            catch
            {
                return ls;
            }

        }

    }

}
