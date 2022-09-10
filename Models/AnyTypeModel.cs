using System.Collections.Generic;

namespace BotGoJs.Models
{
    public class AnyTypeModel
    {
        public List<AudioModel> audio { get; set; }

        public List<FileModel> file { get; set; }

        public List<ImageModel> image { get; set; }

        public List<LocationModel> location { get; set; }

        public List<TextModel> text { get; set; }

        public List<VideoModel> video { get; set; }
    }
}