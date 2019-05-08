﻿using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Beatmap.Properties.Level;
using System.IO;

namespace Rhythmic.Beatmap
{
    /// <summary>Contains information like: BG, <see cref="LevelMeta"/>, etc</summary>
    public class DatabasedBeatmap : BeatmapMeta
    {
        public Stream Logo { get; set; }

        public Stream Background { get; set; }

        public Track Song { get; set; }

        public int ID { get; set; }
    }
}
