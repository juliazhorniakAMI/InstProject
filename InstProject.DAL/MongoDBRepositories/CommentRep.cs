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
   public class CommentRep
    {
        IMongoDatabase database;
        IMongoCollection<Comment> collection;
        public CommentRep()
        {
            database = ConfigManager.GetDefaultDatabase();
            collection = database.GetCollection<Comment>("comments");
        }

        public List<Comment> GetComments(ObjectId postId)
        {

            
         return collection.Find(p => p.postId == postId).ToList();
         
        }
        public int GetCommentLike(ObjectId id)
        {

            var filter = Builders<Comment>.Filter.Eq("Id", id);
            var like = collection.Find(filter).Project(x => x.AmountOfLikes).First();
            return like;
        }

        public int GetCommentDisLike(ObjectId id)
        {

            var filter = Builders<Comment>.Filter.Eq("Id", id);
            var like = collection.Find(filter).Project(x => x.AmountOfDislikes).First();
            return like;
        }
        public void Add(Comment comment) =>
            collection.InsertOne(comment);

        public void AddLike(User user, ObjectId comId)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", comId);
            var update = Builders<Comment>.Update.Inc("AmountOfLikes", +1);
            collection.UpdateOne(filter, update);

            update = Builders<Comment>.Update.Push("PersonsWhoLike", user.Nick);
            collection.UpdateOne(filter, update);

        }

        public void DismissLike(User user, ObjectId postId)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", postId);
            var update = Builders<Comment>.Update.Inc("AmountOfLikes", -1);
            collection.UpdateOne(filter, update);

            update = Builders<Comment>.Update.Pull("PersonsWhoLike", user.Nick);
            collection.UpdateOne(filter, update);
        }
        public void AddDislike(User user, ObjectId postId)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", postId);
            var update = Builders<Comment>.Update.Inc("AmountOfDislikes", +1);
            collection.UpdateOne(filter, update);

            update = Builders<Comment>.Update.Push("PersonsWhoDisLike", user.Nick);

            collection.UpdateOne(filter, update);

        }

        public void DismissDisLike(User user, ObjectId postId)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", postId);
            var update = Builders<Comment>.Update.Inc("AmountOfDislikes", -1);
            collection.UpdateOne(filter, update);

            update = Builders<Comment>.Update.Pull("PersonsWhoDisLike", user.Nick);
            collection.UpdateOne(filter, update);
        }
        public List<string> GetPersonsWhoLiked(ObjectId postId)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", postId);
            var people = collection.Find(filter).Project(x => x.PersonsWhoLike).First();
            return people;

        }
        public List<string> GetPersonsWhoDisLiked(ObjectId postId)
        {
            var filter = Builders<Comment>.Filter.Eq("Id", postId);
            var people = collection.Find(filter).Project(x => x.PersonsWhoDisLike).First();
            return people;

        }

    }
}
