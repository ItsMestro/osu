// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;

namespace osu.Game.Tournament.Components
{
    public class DrawableTournamentHeaderText : TournamentSpriteText
    {
        public DrawableTournamentHeaderText()
        {
        Text = "4 Digit Mania World Cup 2";
        Font = OsuFont.Torus.With(size: 26, weight: FontWeight.SemiBold);
        }
    }
}
