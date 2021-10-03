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
    public class Post
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
     
        [BsonIgnoreIfNull]
        public string Description { get; set; }
        [BsonIgnoreIfNull]
        public string Date { get; set; }
        [BsonIgnoreIfNull]
        public int AmountOfLikes { get; set; }
        [BsonIgnoreIfNull]
        public List<string> PersonsWhoLike { get; set; }
        [BsonIgnoreIfDefault]
        public ObjectId OwnerPostId { get; set; }
        [BsonIgnoreIfNull]
        public int AmountOfDislikes { get; set; }
        [BsonIgnoreIfNull]
        public List<string> PersonsWhoDisLike { get; set; }
        [BsonIgnoreIfNull]
        public List<Comment> Comments { get; set; }

    }
}
