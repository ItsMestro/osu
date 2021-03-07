using System;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace osu.Game.Tournament.Screens.Countdown
{
    public class Visualizer : BeatSyncedContainer
    {
        private readonly Container logoBeatContainer;
        private readonly Container logoAmplitudeContainer;
        private readonly MenuLogoVisualisation visualizer;

        public bool BeatMatching = true;

        public Visualizer()
        {
            Children = new Drawable[]
            {
                logoAmplitudeContainer = new Container
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        logoBeatContainer = new Container
                                        {
                                            AutoSizeAxes = Axes.Both,
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
    },
},
            };
            }
    }
}
