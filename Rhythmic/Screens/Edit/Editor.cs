﻿using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
using Rhythmic.Beatmap;
using Rhythmic.Beatmap.Properties;
using Rhythmic.Graphics.Sprites;
using Rhythmic.Screens.Backgrounds;
using Rhythmic.Screens.Edit.Componets.Overlays;
using System;

namespace Rhythmic.Screens.Edit
{
    public class Editor : RhythmicScreen
    {
        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap();

        public override bool HideOverlaysOnEnter => true;

        [Resolved]
        private BeatmapCollection collection { get; set; }

        private readonly BindableBeatDivisor beatDivisor = new BindableBeatDivisor();

        private EditorClock clock;

        private DependencyContainer dependencies;

        private Bindable<BeatmapMeta> beatmap => collection.CurrentBeatmap;

        private SpriteText trackTimer;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void load()
        {
            IAdjustableClock sourceClock = (IAdjustableClock)beatmap.Value.Song ?? new StopwatchClock();
            clock = new EditorClock(beatmap.Value, beatDivisor) { IsCoupled = false };
            clock.ChangeSource(sourceClock);

            beatmap.Value.Song.Looping = true;

            dependencies.CacheAs<IFrameBasedClock>(clock);
            dependencies.CacheAs<IAdjustableClock>(clock);
            dependencies.Cache(beatDivisor);

            BeatmapMetadataOverlay beatmapOverlay;

            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Name = "Screen container",
                    RelativeSizeAxes = Axes.Both,
                    Child = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Masking = true
                    }
                },
                trackTimer = new SpriteText
                {
                    Font = RhythmicFont.Default
                },
                beatmapOverlay = new BeatmapMetadataOverlay
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.6f, 0.8f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            };

            beatmapOverlay.ToggleVisibility();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    seek(e, -1);
                    return true;

                case Key.Right:
                    seek(e, 1);
                    return true;

                case Key.Space:
                    if (clock.IsRunning)
                        clock.Stop();
                    else
                        clock.Start();

                    return true;
            }

            return base.OnKeyDown(e);
        }

        private double scrollAccumulation;

        protected override bool OnScroll(ScrollEvent e)
        {
            scrollAccumulation += (e.ScrollDelta.X + e.ScrollDelta.Y) * (e.IsPrecise ? 0.1 : 1);

            const int precision = 1;

            while (Math.Abs(scrollAccumulation) > precision)
            {
                if (scrollAccumulation > 0)
                    seek(e, -1);
                else
                    seek(e, 1);

                scrollAccumulation = scrollAccumulation < 0 ? Math.Min(0, scrollAccumulation + precision) : Math.Max(0, scrollAccumulation - precision);
            }

            return true;
        }

        public override void OnResuming(IScreen last)
        {
            beatmap.Value.Song?.Stop();
            base.OnResuming(last);
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);
            Background.FadeColour(Color4.DarkGray, 500);
            beatmap.Value.Song?.Stop();
        }

        public override bool OnExiting(IScreen next)
        {
            Background.FadeColour(Color4.White, 500);

            if (beatmap.Value.Song != null)
            {
                beatmap.Value.Song.Tempo.Value = 1;
                beatmap.Value.Song.Start();
            }

            return base.OnExiting(next);
        }

        private void seek(UIEvent e, int direction)
        {
            double amount = e.ShiftPressed ? 2 : 1;

            Console.WriteLine(direction);

            if (direction < 1)
                clock.SeekBackward(amount);
            else
                clock.SeekForward(amount);
        }

        protected override void Update()
        {
            base.Update();

            trackTimer.Text = TimeSpan.FromMilliseconds(clock.CurrentTime).ToString(@"mm\:ss\:fff");
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            if (clock.IsRunning)
                clock.ProcessFrame();
        }
    }
}
