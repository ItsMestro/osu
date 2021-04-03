// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Tournament.Models;
using osu.Game.Users;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Tournament.Components
{
    public class DrawableTeamWithPlayers : CompositeDrawable
    {
        public DrawableTeamWithPlayers(TournamentTeam team, TeamColour colour)
        {
            AutoSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(30),
                    Children = new Drawable[]
                    {
                        new DrawableTeamTitleWithHeader(team, colour),
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Padding = new MarginPadding { Left = 10 },
                            Spacing = new Vector2(10),
                            Children = new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Vertical,
                                    AutoSizeAxes = Axes.Both,
                                    ChildrenEnumerable = team?.Players.Select(createPlayerText).Take(1) ?? Enumerable.Empty<Drawable>(),
                                },
                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Vertical,
                                    AutoSizeAxes = Axes.Both,
                                    ChildrenEnumerable = team?.Players.Select(createTextRank).Take(1) ?? Enumerable.Empty<Drawable>()
                                },
                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Vertical,
                                    AutoSizeAxes = Axes.Both,
                                    ChildrenEnumerable = team?.Players.Select(createTextpp).Take(1) ?? Enumerable.Empty<Drawable>()
                                },
                            }
                        },
                    }
                },
            };

            TournamentSpriteText createPlayerText(User p) =>
                new TournamentSpriteText
                {
                    Text = $"Qualifier Seed: {team.Seed}",
                    Font = OsuFont.Torus.With(size: 24, weight: FontWeight.SemiBold),
                    Colour = Color4.White,
                };
            TournamentSpriteText createTextRank(User p) =>
                new TournamentSpriteText
                {
                    Text = $"Global Rank: {p.Statistics.GlobalRank}",
                    Font = OsuFont.Torus.With(size: 24, weight: FontWeight.SemiBold),
                    Colour = Color4.White,
                };
            TournamentSpriteText createTextpp(User p) =>
                new TournamentSpriteText
                {
                    Text = $"Performance: {p.Statistics.PP:#,0}pp",
                    Font = OsuFont.Torus.With(size: 24, weight: FontWeight.SemiBold),
                    Colour = Color4.White,
                };
        }
    }
}
