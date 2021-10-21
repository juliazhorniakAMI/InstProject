using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using InstProject.DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InstProject.DAL.Repositories
{
    public class UserRep
    {
        IMongoDatabase database;
        IMongoCollection<User> collection;
        public UserRep()
        {
            database = ConfigManager.GetDefaultDatabase();
            collection = database.GetCollection<User>("persons");
        }

      public bool CheckIfUserExists(string nickname, string password)
        {
            User user = new User();
            user = GetUserByNick(nickname);
            if (user != null)
            {
                if (user.Password == GetHashStringSHA256(password))
                {
                    return true;
                }
             
            }

            return false;


        }

      public string GetHashStringSHA256(string str)
      {
          SHA256 sha256 = SHA256.Create();
          byte[] hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
          string result = "";
          foreach (byte b in hashPassword)
          {
              result += b.ToString();
          }
          return result;
      }

        // 
        public void Add(User user)
        {

            user.Password = GetHashStringSHA256(user.Password);
            collection.InsertOne(user);
        }


        public void Delete(ObjectId userId) =>
             collection.DeleteOne(u => u.Id == userId);


        public void UpdateField(string nickname, string FieldToEdit, string FieldValue)
        {
            var filter = Builders<User>.Filter.Eq("Nick", nickname);
            var update = Builders<User>.Update.Set(FieldToEdit, FieldValue);
            collection.UpdateOne(filter, update);
        }

        public void UpdateDate(string nickname)
        {
            var filter = Builders<User>.Filter.Eq("Nick", nickname);
            var update = Builders<User>.Update.Set("Date", DateTime.Now.ToString());
            collection.UpdateOne(filter, update);
        }
        //
        public void addFollower(string nickname, string newFollower)
        {
            var filter = Builders<User>.Filter.Eq("Nick", nickname);
            var update = Builders<User>.Update.Push("Subscribers", newFollower);
            collection.UpdateOne(filter, update);



        }
        public void unFollow(string nickname, string follower)
        {
            var filter = Builders<User>.Filter.Eq("Nick", nickname);
            var update = Builders<User>.Update.Pull("Following", follower);
            collection.UpdateOne(filter, update);


            filter = Builders<User>.Filter.Eq("Nick", follower);
            update = Builders<User>.Update.Pull("Subscribers", nickname);
            collection.UpdateOne(filter, update);
        }

        public void addFollowing(string nickname, string newFollowing)
        {
            var filter = Builders<User>.Filter.Eq("Nick", nickname);
            var update = Builders<User>.Update.Push("Following", newFollowing);
            collection.UpdateOne(filter, update);

        }
        //
        public List<string> GetFollowers(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            var people = collection.Find(filter).Project(x => x.Subscribers).First();
            return people;
        }

        public List<string> GetFollowing(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq("Id", id);
            var people = collection.Find(filter).Project(x => x.Following).First();
            return people;
        }
        //
      
        //
        public List<User> GetUsers() =>
            collection.Find(entity => true).ToList();

        public User GetUserByNick(string nickname) =>
          collection.Find(entity => entity.Nick == nickname).FirstOrDefault();

        public User GetUser(ObjectId id) =>
         collection.Find(entity => entity.Id == id).FirstOrDefault();
        //
        public ObjectId GetUserId(string nickname)
        {
            var user = collection.Find(entity => entity.Nick == nickname).FirstOrDefault();
            return user.Id;
        }
    

     

    }
}

