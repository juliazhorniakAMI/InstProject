using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace InstProject.BLL.Services
{
    [DataContract]
    public class IdUserInfo
    {
        [DataMember]
        public ObjectId Id { get; set; }

        
    }
}
