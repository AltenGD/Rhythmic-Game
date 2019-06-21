using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Database;
using Rhythmic.Graphics.Containers;
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

        private Container content;

        protected override Container<Drawable> Content => content;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public RhythmicGameBase()
        {
            storage = new NativeStorage("Rhythmic");
        }

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager frameworkConfig)
        {
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista-Italic"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista-Thin"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista-ThinItalic"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista-Light"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista-LightItalic"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista-SemiBold"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista-SemiBoldItalic"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista-Bold"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista-BoldItalic"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Digitall/Digitall"));
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Audiowide/Audiowide"));

            dependencies.Cache(this);
            dependencies.Cache(storage);

            dependencies.CacheAs(new BeatmapAPI());
            dependencies.CacheAs(new RhythmicStore());

            Resources.AddStore(new DllResourceStore(mainResourceFile));

            frameworkConfig.Set(FrameworkSetting.FrameSync, FrameSync.Unlimited);

            var container = new GlobalActionContainer(this)
            {
                RelativeSizeAxes = Axes.Both,
                Child = content = new Container { RelativeSizeAxes = Axes.Both }
            };

            base.Content.Add(new Container { RelativeSizeAxes = Axes.Both, Child = container });
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            CreateRequiredFiles();
        }

        public readonly List<ICanAcceptFiles> fileImporters = new List<ICanAcceptFiles>();

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
