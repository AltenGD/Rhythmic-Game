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

        //Most song have the average BPM of 120
        public int BPM { get; set; } = 120;

        public List<string> Tags { get; set; }

        [JsonIgnore]
        public string CreatorName { get; set; }
    }
}