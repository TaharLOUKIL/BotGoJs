using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotGoJs.Models
{
    public class DataModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string name { get; set; }
        public Object @object { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string BotID { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LinkDataArray
    {
        public string from { get; set; }
        public string to { get; set; }
        public string fromPort { get; set; }
        public string toPort { get; set; }
        public List<double> points { get; set; }
    }

    public class NodeDataArray
    {
        public string key { get; set; }
        public string text { get; set; }
        public List<dynamic> onRecieve { get; set; }
        public List<dynamic> onEnter { get; set; }
        public List<dynamic> transition { get; set; }
        public string location { get; set; }
    }

    public class Object
    {
        public string @class { get; set; }
        public string linkFromPortIdProperty { get; set; }
        public string linkToPortIdProperty { get; set; }
        public List<NodeDataArray> nodeDataArray { get; set; }
        public List<LinkDataArray> linkDataArray { get; set; }
    }


}
