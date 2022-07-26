using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotGoJs.Models
{
    public class VideoModel
    {
        public string _id { get; set; }

        public string Titre { get; set; }
        public string url { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime modifiedAt { get; set; }

        public string type { get; set; }
    }
}
