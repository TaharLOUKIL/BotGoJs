using BotGoJs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static BotGoJs.Models.DataBaseViewModel;

namespace BotGoJs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public DataController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult get([FromRoute] string  id)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase("Gojs").GetCollection<DataViewModel>("Data").AsQueryable().Where(c=> c.BotID==id).ToList();
            List<DataModel> list = new List<DataModel>();
            foreach(var i in dblist)
            {
                list.Add(this.ConvertToFrontModel(i));
            }
            return new JsonResult(list);
        }

        [HttpPost]
        public JsonResult Post(DataModel  data)
        {
            var res = this.convertTodatabaseModel(data);
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            data._id = ObjectId.GenerateNewId().ToString();
            dbclient.GetDatabase("Gojs").GetCollection<DataViewModel>("Data").InsertOne(res);
            return new JsonResult(data);
        }




        [HttpPut]
        public JsonResult Put(DataModel data)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var filter = Builders<DataViewModel>.Filter.Eq("_id", data._id);
            var res = this.convertTodatabaseModel(data); 
            dbclient.GetDatabase("Gojs").GetCollection<DataViewModel>("Data").ReplaceOne(filter, res);
            return new JsonResult(get(data.BotID));
        }

       public DataViewModel convertTodatabaseModel(DataModel  data)
        {
            DataViewModel Dvm = new DataViewModel();
            Dvm._id = data._id;
            Dvm.name = data.name;
            Dvm.BotID = data.BotID;
            Dvm.@object = new ObjectViewModel();
            
            Dvm.@object.linkDataArray = new List<LinkDataArrayViewModel>();
            foreach (var item in data.@object.linkDataArray)
            {
                LinkDataArrayViewModel lfavm = new LinkDataArrayViewModel();
                lfavm.from = item.from;
                lfavm.to = item.to;
                lfavm.fromPort = item.fromPort;
                lfavm.toPort = item.toPort;
                List<double> p = new List<double>();
                foreach (var point in item.points)
                {
                    p.Add(point);
                }
                lfavm.points = p;
                Dvm.@object.linkDataArray.Add(lfavm);

            }
            Dvm.@object.linkFromPortIdProperty = data.@object.linkFromPortIdProperty;
            Dvm.@object.linkToPortIdProperty = data.@object.linkToPortIdProperty;
            Dvm.@object.@class = data.@object.@class;

            Dvm.@object.nodeDataArray = new List<NodeDataArrayViewModel>();
            foreach (var item in data.@object.nodeDataArray)
            {
                NodeDataArrayViewModel ndavm = new NodeDataArrayViewModel();
                ndavm.key = item.key;
                ndavm.location = item.location;
                ndavm.text = item.text;
                ndavm.onEnter = new List<ActionviewModel>();
                foreach (var onenter in item.onEnter)
                {
                    ActionviewModel avm = new ActionviewModel();
                    avm.id  = onenter["_id"];
                    avm.type = onenter["type"];
                    ndavm.onEnter.Add(avm);
                }
                ndavm.onRecieve = new List<ActionviewModel>();
                foreach (var onrecieve in item.onRecieve)
                {
                    ActionviewModel avm = new ActionviewModel();
                    avm.id = onrecieve["_id"];
                    avm.type = onrecieve["type"];
                    ndavm.onRecieve.Add(avm);
                }
                ndavm.transition = new List<ActionviewModel>();
                foreach (var transition in item.transition)
                {
                    ActionviewModel avm = new ActionviewModel();
                    avm.id = transition["_id"];
                    avm.type = transition["type"];
                    ndavm.transition.Add(avm);
                }
                Dvm.@object.nodeDataArray.Add(ndavm);
            }
            return Dvm;
        }

    public  DataModel ConvertToFrontModel(DataViewModel data)
    {
        DataModel Dvm = new DataModel();
        Dvm._id = data._id;
        Dvm.name = data.name;
        Dvm.BotID = data.BotID;
        Dvm.@object = new Models.Object();

        Dvm.@object.linkDataArray = new List<LinkDataArray>();
        foreach (var item in data.@object.linkDataArray)
        {
            LinkDataArray lda = new LinkDataArray();
                lda.from = item.from;
                lda.to = item.to;
                lda.fromPort = item.fromPort;
                lda.toPort = item.toPort;
            List<double> p = new List<double>();
            foreach (var point in item.points)
            {
                p.Add(point);
            }
                lda.points = p;
            Dvm.@object.linkDataArray.Add(lda);

        }
        Dvm.@object.linkFromPortIdProperty = data.@object.linkFromPortIdProperty;
        Dvm.@object.linkToPortIdProperty = data.@object.linkToPortIdProperty;
        Dvm.@object.@class = data.@object.@class;

        Dvm.@object.nodeDataArray = new List<NodeDataArray>();
        foreach (var item in data.@object.nodeDataArray)
        {
            NodeDataArray ndavm = new NodeDataArray();
            ndavm.key = item.key;
            ndavm.location = item.location;
            ndavm.text = item.text;
            ndavm.onEnter = new List<dynamic>();
            foreach (var onenter in item.onEnter)
            {
                    MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                    if (onenter.type == "Text")
                    {  
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<TextModel>("Texte").Find(Builders<TextModel>.Filter.Eq("_id", onenter.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onenter.type;
                            ndavm.onEnter.Add(obj);
                        }
                        
                    }
                    else if (onenter.type == "Video")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<VideoModel>("Video").Find(Builders<VideoModel>.Filter.Eq("_id", onenter.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onenter.type;
                            ndavm.onEnter.Add(obj);
                        }
                        
                    }
                    else if (onenter.type == "Image")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<ImageModel>("Image").Find(Builders<ImageModel>.Filter.Eq("_id", onenter.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onenter.type;
                            ndavm.onEnter.Add(obj);
                        }
                       
                    }
                    else if (onenter.type == "Audio")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<AudioModel>("Audio").Find(Builders<AudioModel>.Filter.Eq("_id", onenter.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onenter.type;
                            ndavm.onEnter.Add(obj);
                        }
                        
                    }
                   else  if (onenter.type == "File")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<FileModel>("File").Find(Builders<FileModel>.Filter.Eq("_id", onenter.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onenter.type;
                            ndavm.onEnter.Add(obj);
                        }
                        
                    }
                    else if (onenter.type == "Location")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<LocationModel>("Localisation").Find(Builders<LocationModel>.Filter.Eq("_id", onenter.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onenter.type;
                            ndavm.onEnter.Add(obj);
                        }
                       
                    }
                }

            ndavm.onRecieve = new List<dynamic>();
            foreach (var onrecieve in item.onRecieve)
                {
                    MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                    if (onrecieve.type == "Text")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<TextModel>("Texte").Find(Builders<TextModel>.Filter.Eq("_id", onrecieve.id)).FirstOrDefault();
                        if(obj != null)
                        {
                            obj.type = onrecieve.type;
                            ndavm.onRecieve.Add(obj);
                        }
                        
                    }
                    else if (onrecieve.type == "Video")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<VideoModel>("Video").Find(Builders<VideoModel>.Filter.Eq("_id", onrecieve.id)).FirstOrDefault();
                       if(obj != null)
                        {
                            obj.type = onrecieve.type;
                            ndavm.onRecieve.Add(obj);
                        }    
                    }
                    else if (onrecieve.type == "Image")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<ImageModel>("Image").Find(Builders<ImageModel>.Filter.Eq("_id", onrecieve.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onrecieve.type;
                            ndavm.onRecieve.Add(obj);
                        }
                        
                    }
                    else if (onrecieve.type == "Audio")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<AudioModel>("Audio").Find(Builders<AudioModel>.Filter.Eq("_id", onrecieve.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onrecieve.type;
                            ndavm.onRecieve.Add(obj);
                        }
                    }
                    else if (onrecieve.type == "File")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<FileModel>("Fichier").Find(Builders<FileModel>.Filter.Eq("_id", onrecieve.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onrecieve.type;
                            ndavm.onRecieve.Add(obj);
                        }
                    }
                    else if (onrecieve.type == "Location")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<LocationModel>("Localisation").Find(Builders<LocationModel>.Filter.Eq("_id", onrecieve.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = onrecieve.type;
                            ndavm.onRecieve.Add(obj);
                        }
                    }
                }

            ndavm.transition = new List<dynamic>();
            foreach (var transition in item.transition)
                {
                    MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                    if (transition.type == "Text")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<TextModel>("Texte").Find(Builders<TextModel>.Filter.Eq("_id", transition.id)).FirstOrDefault();
                       if (obj != null)
                        {
                            obj.type = transition.type;
                            ndavm.transition.Add(obj);
                        }
                    }
                    else if (transition.type == "Video")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<VideoModel>("Video").Find(Builders<VideoModel>.Filter.Eq("_id", transition.id)).FirstOrDefault();
                       if (obj != null)
                        {
                            obj.type = transition.type;
                            ndavm.transition.Add(obj);
                        }
                    }
                    else if (transition.type == "Image")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<ImageModel>("Image").Find(Builders<ImageModel>.Filter.Eq("_id", transition.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = transition.type;
                            ndavm.transition.Add(obj);
                        }
                        
                    }
                    else if (transition.type == "Audio")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<AudioModel>("Audio").Find(Builders<AudioModel>.Filter.Eq("_id", transition.id)).FirstOrDefault();
                       if (obj != null)
                        {
                            obj.type = transition.type;
                            ndavm.transition.Add(obj);
                        }
                    }
                    else if (transition.type == "File")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<FileModel>("File").Find(Builders<FileModel>.Filter.Eq("_id", transition.id)).FirstOrDefault();
                       if (obj != null)
                        {
                            obj.type = transition.type;
                            ndavm.transition.Add(obj);
                        }
                    }
                    else if (transition.type == "Location")
                    {
                        var obj = dbclient.GetDatabase("Gojs").GetCollection<LocationModel>("Localisation").Find(Builders<LocationModel>.Filter.Eq("_id", transition.id)).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.type = transition.type;
                            ndavm.transition.Add(obj);
                        }
                    }
                }
            Dvm.@object.nodeDataArray.Add(ndavm);
        }
        return Dvm;
    }
}
}
