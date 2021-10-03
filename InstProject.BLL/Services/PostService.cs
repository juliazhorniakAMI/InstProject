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
    public class PostServices
    {
        PostRep postRep;
        UserRep userRep;
        UserService userService;

        public PostServices()
        {
            postRep = new PostRep();

            userRep = new UserRep();
            userService = new UserService();
        }

        //
        public void InsertPost(string text)
        {
            Post post = new Post();
            post.Description = text;
            post.OwnerPostId = userService.ReadCurrentId();
            post.Date = DateTime.Now.ToString();
            postRep.Add(post);
        }

        //
        public void EditPost(string newText, ObjectId postId)
        {
            try
            {
                postRep.UpdatePost(postId, newText);
            }
            catch
            {

            }

        }

        //
        public bool DeletePost(ObjectId postId)
        {
            try
            {
                postRep.Delete(postId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //
        public bool CheckIfUserDisLikePost(ObjectId id, ObjectId postId)
        {

            Post post = new Post();
            post = postRep.GetPost(postId);
            if (post != null)
            {
                if (post.PersonsWhoDisLike != null && post.PersonsWhoDisLike.Count > 0)
                {
                    foreach (var nick in post.PersonsWhoDisLike)
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

        public bool CheckIfUserLikePost(ObjectId id, ObjectId postId)
        {

            Post post = new Post();
            post = postRep.GetPost(postId);
            if (post != null)
            {
                if (post.PersonsWhoLike != null && post.PersonsWhoLike.Count > 0)
                {
                    foreach (var nick in post.PersonsWhoLike)
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

      
        public void AddDislike(ObjectId ID, ObjectId postId)
        {
            User user = userService.GetUser();
            try
            {
                postRep.AddDislike(user, postId);
            }
            catch
            {

            }

        }

        public void DismissDisLike(ObjectId ID, ObjectId postId)
        {
            User user = userService.GetUser();
            try
            {
                postRep.DismissDisLike(user, postId);
            }
            catch
            {

            }
        }

        //
        public void AddLike(ObjectId ID, ObjectId postId)
        {
            User user = userService.GetUser();
            try
            {
                postRep.AddLike(user, postId);
            }
            catch
            {

            }

        }

        public void DismissLike(ObjectId ID, ObjectId postId)
        {
            User user = userService.GetUser();
            try
            {
                postRep.DismissLike(user, postId);
            }
            catch
            {

            }
        }

      
        public int GetLikes(ObjectId postID)
        {
            try
            {
                return postRep.GetLike(postID);
            }
            catch
            {
                return 0;
            }

        }

        public int GetDislikes(ObjectId postID)
        {
            try
            {
                return postRep.GetDislike(postID);
            }
            catch
            {
                return 0;
            }

        }

        //
       

        //
        public List<Post> GetSortedPosts(ObjectId id,string TimeOfLastUserLogin, List<string> following)
        {
            List<ObjectId> ids = new List<ObjectId>();
            if (following != null)
            {
                foreach (var el in following)
                {
                    ids.Add(userRep.GetUserId(el));
                }

                return postRep.GetSortedPosts(id,TimeOfLastUserLogin, ids);
            }

            return new List<Post>();

        }

        public List<Post> GetPosts(ObjectId ID)
        {
            List<Post> posts = new List<Post>();
            try
            {
                posts = postRep.GetPosts(ID);
                return posts;
            }
            catch
            {
                return posts;
            }

        }

        public List<Post> GetPosts()
        {
            List<Post> posts = new List<Post>();
            try
            {
                posts = postRep.GetPosts(userService.ReadCurrentId());
                return posts;
            }
            catch
            {
                return posts;
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
        //

    }
}

