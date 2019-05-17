using Newtonsoft.Json;

namespace Rhythmic.Beatmap.Properties.Metadata
{
    public class LevelMetadata
    {
        public string LevelName { get; set; }

        public ulong CreatorID { get; set; }

        [JsonIgnore]
        public string CreatorName { get; set; }
    }
}