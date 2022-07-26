﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotGoJs.Models
{
    /// <summary>
    /// Modèle qui servira à la persistance des Bots créés depuis le front
    /// </summary>
    public class BotModel  
    {
        [BsonRepresentation(BsonType.ObjectId)]
        //Id du bot
        public string _id { get; set; }
        
        //Nom du bot
        public string name { get; set; }
    }
}
