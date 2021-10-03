using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InstProject.DAL.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfNull]
        public string FirstName { get; set; }
        [BsonIgnoreIfNull]
        public string LastName { get; set; }
      
        [BsonIgnoreIfNull]
        public string Nick { get; set; }
     
        [BsonIgnoreIfNull]
        public List<string> Subscribers { get; set; }
        [BsonIgnoreIfNull]
        public List<string> Following { get; set; }
      
        [BsonIgnoreIfNull]
        public string Gender { get; set; }
        [BsonIgnoreIfNull]
        public string Password { get; set; }
        [BsonIgnoreIfNull]
        public string Date { get; set; }
    }
}
