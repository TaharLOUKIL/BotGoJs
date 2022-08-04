using System;

namespace BotGoJs.Models
{
    public class AudioModel
    {
        private string _id;
        private string _titre;
        private string _url;
        private DateTime _createdAt;
        private DateTime _modifiedAt;
        private string _type;

        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string Titre
        {
            get { return this._titre; }
            set { this._titre = value; }
        }

        public string Url
        {
            get { return this._url; }
            set { this._url = value; }
        }

        public DateTime CreatedAt
        {
            get { return this._createdAt; }
            set { this._createdAt = value; }
        }

        public DateTime ModifiedAt
        {
            get { return this._modifiedAt; }
            set { this._modifiedAt = value; }
        }

        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
    }
}