// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Tournament.Components;
using osu.Game.Tournament.IPC;
using osuTK;

namespace osu.Game.Tournament.Screens
{
    public abstract class TimerScreen : TournamentScreen
    {
        protected readonly SongPanel SongPanel;
        //private StopwatchClock timer;

        protected TimerScreen()
        {
            AddInternal(SongPanel = new SongPanel
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Depth = float.MinValue,
                Position = new Vector2(0, 0),
            });
        }

        [BackgroundDependencyLoader]
        private void load(MatchIPCInfo ipc)
        {
            ipc.Beatmap.BindValueChanged(beatmapChanged, true);
        }

        private void beatmapChanged(ValueChangedEvent<BeatmapInfo> beatmap)
        {
            SongPanel.FadeInFromZero(300, Easing.OutQuint);
            SongPanel.Beatmap = beatmap.NewValue;
        }
    }
}
