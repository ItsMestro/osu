// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Tournament.Components;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osuTK;
using osu.Framework.Timing;

namespace osu.Game.Tournament.Screens.Countdown
{
    public class CountdownScreen : TimerScreen // IProvideVideo
    {
        private DrawableTournamentHeaderLogo logo;

        private Bindable<double> time = new Bindable<double>();

        private StopwatchClock clock;

        private MenuLogoVisualisation visualizer;

        [BackgroundDependencyLoader]
        private void load()
        {
            clock = new StopwatchClock();
            clock.Seek(50);
            //time.BindValueChanged(_ => updateClock());
            //time.BindTo(clock.CurrentTime);
            AddRangeInternal(new Drawable[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Padding = new MarginPadding(20),
                    Spacing = new Vector2(5),
                    Children = new Drawable[]
                    {
                        logo = new DrawableTournamentHeaderLogo
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                        },
                        new DrawableTournamentHeaderText
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                        },
                    }
                },
                new TourneyVideo("showcase")
                {
                    Loop = true,
                    RelativeSizeAxes = Axes.Both,
                },
                new Container
                {
                    Padding = new MarginPadding { Bottom = SongPanel.HEIGHT },
                    RelativeSizeAxes = Axes.Both,
                },
                new Container
                {
                    Colour = new Color4(54, 54, 54, 255),
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    Alpha = 0.5f,
                    Size = new Vector2(50),
                    Children = new Drawable[]
                    {
                        visualizer = new MenuLogoVisualisation
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Origin = Anchor.Centre,
                                                    Anchor = Anchor.Centre,
                                                    Alpha = 0.5f,
                                                    Size = new Vector2(0.96f)
                                                },
                    },
                                                },
            });
        }
    }
}
