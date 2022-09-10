using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace BotGoJs.Models
{
    public class DataBaseViewModel
    {
        public class DataViewModel
        {
            [BsonRepresentation(BsonType.ObjectId)]
            public string _id { get; set; }

            public string name { get; set; }
            public ObjectViewModel @object { get; set; }

            [BsonRepresentation(BsonType.ObjectId)]
            public string BotID { get; set; }
        }

        public class LinkDataArrayViewModel
        {
            public string from { get; set; }
            public string to { get; set; }
            public string fromPort { get; set; }
            public string toPort { get; set; }
            public List<double> points { get; set; }
        }

        public class NodeDataArrayViewModel
        {
            public string key { get; set; }
            public string text { get; set; }
            public List<ActionviewModel> onRecieve { get; set; }

            public List<ActionviewModel> onEnter { get; set; }
            public List<ActionviewModel> transition { get; set; }
            public string location { get; set; }
        }

        public class ObjectViewModel
        {
            public string @class { get; set; }
            public string linkFromPortIdProperty { get; set; }
            public string linkToPortIdProperty { get; set; }
            public List<NodeDataArrayViewModel> nodeDataArray { get; set; }
            public List<LinkDataArrayViewModel> linkDataArray { get; set; }
        }

        public class ActionviewModel
        {
            public string type { get; set; }

            [BsonRepresentation(BsonType.ObjectId)]
            public string id { get; set; }
        }
    }
}