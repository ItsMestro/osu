// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Graphics;
using osu.Game.Rulesets;
using osu.Game.Screens.Menu;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Tournament.Components
{
    public class SongPanel : CompositeDrawable
    {
        private BeatmapInfo beatmap;

        public const float HEIGHT = 145 / 2f;

        [Resolved]
        private IBindable<RulesetInfo> ruleset { get; set; }

        public BeatmapInfo Beatmap
        {
            get => beatmap;
            set
            {
                if (beatmap == value)
                    return;

                beatmap = value;
                update();
            }
        }

        private LegacyMods mods;

        public LegacyMods Mods
        {
            get => mods;
            set
            {
                mods = value;
                update();
            }
        }

        private FillFlowContainer flow;

        // Todo: This is a hack for https://github.com/ppy/osu-framework/issues/3617 since this container is at the very edge of the screen and potentially initially masked away.
        protected override bool ComputeIsMaskedAway(RectangleF maskingBounds) => false;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            InternalChildren = new Drawable[]
            {
                flow = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
            };
        }

        private void update()
        {
            if (beatmap == null)
            {
                flow.Clear();
                return;
            }

            flow.Children = new Drawable[]
            {
                new TournamentBeatmapPanel(beatmap)
                {
                    RelativeSizeAxes = Axes.X,
                    Width = 0.5f,
                    Height = HEIGHT,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
            };
        }
    }
}
