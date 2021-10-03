using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using InstProject.DAL.Models;
using InstProject.DAL.Repositories;
using MongoDB.Bson;



namespace InstProject.BLL.Services
{
    public class UserService
    {
        UserRep rep;
        private string file;
        IdUserInfo curr= new IdUserInfo();
        public UserService()
        {
            rep = new UserRep();
            file = "CurrentId.json";
        }
        
        public bool CheckIfUserExists(string nickname, string password)
        {
           return rep.CheckIfUserExists(nickname, password);
      
        }

        public void UpdateDate()
        {
            try
            {
                rep.UpdateDate(GetUser().Nick);
            }
            catch
            {

            }

        }
        public bool CheckUniqueNick(string nick)
        {
        
            foreach (var elem in rep.GetUsers())
            {
                if (elem.Nick == nick)
                {
                    return false;
                }
            }

            return true;
        }
        //

        public List<User> GetUsers()
        {
            return rep.GetUsers();
        }



        public void WriteCurrentId(ObjectId id)
        {


            curr.Id = id;
            if (curr != null)
            {
                using (FileStream fs = new FileStream(file, FileMode.Create))
                {
                    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(IdUserInfo));
                    jsonFormatter.WriteObject(fs, curr);
                }
            }
        
          

        }

        public ObjectId ReadCurrentId()
        {
           
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(IdUserInfo));
                if (fs.Length != 0)
                {
                    curr = (IdUserInfo)jsonFormatter.ReadObject(fs);
                }

            }

            return curr.Id;
        }
        //
        public bool CheckAlreadyFollow(ObjectId id, string usernickname)
        {
            User user = new User();
            user = rep.GetUser(id);
            if (user != null && user.Following != null)
            {
                foreach (var el in user.Following)
                {
                    if (el == usernickname)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }

      
        //
       

       
        //
        public void AddUser(User user)
        {
            rep.Add(user);

        
        }
        //
        public User GetUser()
        {
            try
            {
                return rep.GetUser(ReadCurrentId());
            }
            catch
            {
                return new User();
            }

        }
        public User GetUser(ObjectId id)
        {
            try
            {
                return rep.GetUser(id);
            }
            catch
            {
                return new User();
            }

        }
        public User GetUserByNick(string nickname)
        {
            try
            {
                return rep.GetUserByNick(nickname);
            }
            catch
            {
                return new User();
            }

        }


        public List<string> GetFollowers(ObjectId id)
        {
            List<string> ls = new List<string>();
            try
            {
                ls = rep.GetFollowers(id);
                return ls;
            }
            catch
            {
                return ls;
            }
        }

        public List<string> GetFollowing(ObjectId id)
        {
            List<string> ls = new List<string>();
            try
            {
                ls = rep.GetFollowing(id);
                return ls;
            }
            catch
            {
                return ls;
            }
        }
        //

        public void AddFollower(string nickname, string newFollower)
        {
            rep.addFollower(nickname, newFollower);
        }

        public void UnFollow(string nickname, string follower)
        {
            rep.unFollow(nickname, follower);

        }

        public void AddFollowing(string nickname, string newFollowing)
        {
            rep.addFollowing(nickname, newFollowing);
        }


    }
}
