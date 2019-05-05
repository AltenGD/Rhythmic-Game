using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Database;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Environment;

namespace Rhythmic
{
    public class RhythmicGameBase : Game, ICanAcceptFiles
    {
        private string mainResourceFile = "Rhythmic.Resources.dll";

        private DependencyContainer dependencies;
        private Storage storage;

        public BeatmapCollection beatmaps { get; set; }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public RhythmicGameBase()
        {
            storage = new NativeStorage("Rhythmic");
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/purista"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Neogrey Medium"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Neogrey"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Audiowide"));

            dependencies.Cache(this);
            dependencies.Cache(storage);
            dependencies.CacheAs(beatmaps);

            dependencies.CacheAs(new BeatmapAPI());

            Resources.AddStore(new DllResourceStore(mainResourceFile));
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            if (beatmaps == null)
                beatmaps = new BeatmapCollection();

            if (beatmaps.Beatmaps == null)
                beatmaps.Beatmaps = new List<BeatmapMeta>();

            CreateRequiredFiles();
            var config = new FrameworkConfigManager(storage);
        }

        private readonly List<ICanAcceptFiles> fileImporters = new List<ICanAcceptFiles>();

        public void Import(params string[] paths)
        {
            var extension = Path.GetExtension(paths.First())?.ToLowerInvariant();

            foreach (var importer in fileImporters)
                if (importer.HandledExtensions.Contains(extension))
                    importer.Import(paths);
        }

        public string[] HandledExtensions => fileImporters.SelectMany(i => i.HandledExtensions).ToArray();

        private void CreateRequiredFiles()
        {
            var path = GetFolderPath(SpecialFolder.ApplicationData) + @"\Rhythmic\Database\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string[] pathList = { "Beatmaps" };

            foreach (var folder in pathList)
            {
                if (!Directory.Exists(path + folder))
                    Directory.CreateDirectory(path + folder);
            }
        }
    }
}
