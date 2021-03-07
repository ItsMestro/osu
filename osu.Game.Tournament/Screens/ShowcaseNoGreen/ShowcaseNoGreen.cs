// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Tournament.Components;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Tournament.Screens.ShowcaseNoGreen
{
    public class ShowcaseNoGreen : BeatmapInfoScreen // IProvideVideo
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal(new Drawable[]
            {
                new TournamentLogo(),
                new TourneyVideo("showcase")
                {
                    Loop = true,
                    RelativeSizeAxes = Axes.Both,
                },
                new Container
                {
                    Padding = new MarginPadding { Bottom = SongBar.HEIGHT },
                    RelativeSizeAxes = Axes.Both,
                }
            });
        }
    }
}
