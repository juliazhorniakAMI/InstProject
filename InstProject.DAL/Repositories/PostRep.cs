using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstProject.DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InstProject.DAL.Repositories
{
    public class PostRep
    {
     
        IMongoDatabase database;
        IMongoCollection<Post> collection;
        public PostRep()
        {
            database = ConfigManager.GetDefaultDatabase();
            collection = database.GetCollection<Post>("posts");
        }

     
        public void Add(Post post) =>
          collection.InsertOne(post);

     

        public void UpdatePost(ObjectId postId, string newTxt)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var update = Builders<Post>.Update.Set("Description", newTxt);
            collection.UpdateOne(filter, update);
        }

        public void AddLike(User user, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var update = Builders<Post>.Update.Inc("AmountOfLikes", +1);
            collection.UpdateOne(filter, update);

            update = Builders<Post>.Update.Push("PersonsWhoLike", user.Nick);
            collection.UpdateOne(filter, update);

        }
       
        public void DismissLike(User user, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var update = Builders<Post>.Update.Inc("AmountOfLikes", -1);
            collection.UpdateOne(filter, update);

            update = Builders<Post>.Update.Pull("PersonsWhoLike", user.Nick);
            collection.UpdateOne(filter, update);
        }
        //
    
        public void AddDislike(User user, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var update = Builders<Post>.Update.Inc("AmountOfDislikes", +1);
            collection.UpdateOne(filter, update);

            update = Builders<Post>.Update.Push("PersonsWhoDisLike", user.Nick);

            collection.UpdateOne(filter, update);

        }

        public void DismissDisLike(User user, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var update = Builders<Post>.Update.Inc("AmountOfDislikes", -1);
            collection.UpdateOne(filter, update);

            update = Builders<Post>.Update.Pull("PersonsWhoDisLike", user.Nick);
            collection.UpdateOne(filter, update);
        }
        //
        public List<string> GetPersonsWhoLiked(ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var people = collection.Find(filter).Project(x => x.PersonsWhoLike).First();
            return people;

        }
        public List<string> GetPersonsWhoDisLiked(ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var people = collection.Find(filter).Project(x => x.PersonsWhoDisLike).First();
            return people;

        }


      
        public int GetLike(ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var like = collection.Find(filter).Project(x => x.AmountOfLikes).First();
            return like;
        }

        public int GetDislike(ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("Id", postId);
            var dislike = collection.Find(filter).Project(x => x.AmountOfDislikes).First();
            return dislike;
        }
        //
      
        //
        public List<Post> GetSortedPosts(ObjectId id, string TimeOfLastUserLogin, List<ObjectId> following)
        {

            var filter =  collection.Find(Builders<Post>.Filter.Empty).ToList();
            List<Post> posts = new List<Post>();
            List<Post> filteredPosts = new List<Post>();
            
            foreach (var x in filter)
            {
                if (Convert.ToDateTime(x.Date) > Convert.ToDateTime(TimeOfLastUserLogin))
                {
                    posts.Add(x);

                }
            }

            foreach (var x in following)
            {

                foreach (var obj in posts)
                {
                    if (obj.OwnerPostId == x)
                    {
                        filteredPosts.Add(obj);

                    }

                }
            }
      
            return filteredPosts;
        }

        public List<Post> GetPosts(ObjectId OwnerId) =>
            collection.Find(p => p.OwnerPostId == OwnerId).ToList();
     
        public Post GetPost(ObjectId id) =>
          collection.Find(p => p.Id == id).FirstOrDefault();
        //
    

        public void Delete(ObjectId postId) =>
           collection.DeleteOne(p => p.Id == postId);
    }

}
