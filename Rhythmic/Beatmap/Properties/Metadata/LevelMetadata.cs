using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rhythmic.Beatmap.Properties.Metadata
{
    public class LevelMetadata
    {
        public string LevelName { get; set; }

        public string LevelNameUnicode { get; set; }

        public ulong CreatorID { get; set; }

        public string Difficulty { get; set; }

        public string Source { get; set; }

        public List<string> Tags { get; set; }

        [JsonIgnore]
        public string CreatorName { get; set; }
    }
}