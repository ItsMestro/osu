// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Tournament.Components;
using osu.Game.Tournament.Models;
using osu.Game.Tournament.Screens.Ladder.Components;
using osuTK;

namespace osu.Game.Tournament.Screens.TeamIntro
{
    public class SeedingScreen : TournamentScreen, IProvideVideo
    {
        private Container mainContainer;

        private readonly Bindable<TournamentMatch> currentMatch = new Bindable<TournamentMatch>();

        private readonly Bindable<TournamentTeam> currentTeam = new Bindable<TournamentTeam>();

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            RelativeSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new TourneyVideo("seeding")
                {
                    RelativeSizeAxes = Axes.Both,
                    Loop = true,
                },
                mainContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new ControlPanel
                {
                    Children = new Drawable[]
                    {
                        new TourneyButton
                        {
                            RelativeSizeAxes = Axes.X,
                            Text = "Show first team",
                            Action = () => currentTeam.Value = currentMatch.Value.Team1.Value,
                        },
                        new TourneyButton
                        {
                            RelativeSizeAxes = Axes.X,
                            Text = "Show second team",
                            Action = () => currentTeam.Value = currentMatch.Value.Team2.Value,
                        },
                        new SettingsTeamDropdown(LadderInfo.Teams)
                        {
                            LabelText = "Show specific team",
                            Current = currentTeam,
                        }
                    }
                }
            };

            currentMatch.BindValueChanged(matchChanged);
            currentMatch.BindTo(LadderInfo.CurrentMatch);

            currentTeam.BindValueChanged(teamChanged, true);
        }

        private void teamChanged(ValueChangedEvent<TournamentTeam> team)
        {
            if (team.NewValue == null)
            {
                mainContainer.Clear();
                return;
            }

            showTeam(team.NewValue);
        }

        private void matchChanged(ValueChangedEvent<TournamentMatch> match) =>
            currentTeam.Value = currentMatch.Value.Team1.Value;

        private void showTeam(TournamentTeam team)
        {
            mainContainer.Children = new Drawable[]
            {
                new LeftInfo(team) { Position = new Vector2(55, 150), },
                new RightInfo(team) { Position = new Vector2(500, 150), },
            };
        }

        private class RightInfo : CompositeDrawable
        {
            public RightInfo(TournamentTeam team)
            {
                FillFlowContainer fill;

                Width = 400;

                InternalChildren = new Drawable[]
                {
                    fill = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                    },
                };

                foreach (var seeding in team.SeedingResults)
                {
                    fill.Add(new ModRow(seeding.Mod.Value, seeding.Seed.Value));
                    foreach (var beatmap in seeding.Beatmaps)
                        fill.Add(new BeatmapScoreRow(beatmap));
                }
            }

            private class BeatmapScoreRow : CompositeDrawable
            {
                public BeatmapScoreRow(SeedingBeatmap beatmap)
                {
                    RelativeSizeAxes = Axes.X;
                    AutoSizeAxes = Axes.Y;

                    InternalChildren = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(5),
                            Children = new Drawable[]
                            {
                                new TournamentSpriteText { Text = beatmap.BeatmapInfo.Metadata.Title, Colour = TournamentGame.TEXT_COLOUR, Font = OsuFont.Torus.With(weight: FontWeight.SemiBold, size: 26) },
                                new TournamentSpriteText { Text = "by", Colour = TournamentGame.TEXT_COLOUR, Font = OsuFont.Torus.With(weight: FontWeight.SemiBold, size: 26) },
                                new TournamentSpriteText { Text = beatmap.BeatmapInfo.Metadata.Artist, Colour = TournamentGame.TEXT_COLOUR, Font = OsuFont.Torus.With(weight: FontWeight.SemiBold, size: 26) },
                            }
                        },
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Y,
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Direction = FillDirection.Horizontal,
                            Padding = new MarginPadding {Left = 160},
                            Spacing = new Vector2(80),
                            Children = new Drawable[]
                            {
                                //new TournamentSpriteText { Text = beatmap.Score.ToString("#,0"), Colour = TournamentGame.TEXT_COLOUR, Width = 80, Font = OsuFont.Torus.With(weight: FontWeight.Regular, size: 28) },
                                new TournamentSpriteText { Text = beatmap.Score.ToString("#,0"), Colour = TournamentGame.TEXT_COLOUR, Width = 90, Font = OsuFont.Torus.With(weight: FontWeight.Medium, size: 26) },
                            }
                        },
                    };
                }
            }

            private class ModRow : CompositeDrawable
            {
                private readonly string mods;
                private readonly int seeding;

                public ModRow(string mods, int seeding)
                {
                    this.mods = mods;
                    this.seeding = seeding;

                    Padding = new MarginPadding { Vertical = 10 };

                    AutoSizeAxes = Axes.Y;
                }

                [BackgroundDependencyLoader]
                private void load(TextureStore textures)
                {
                    InternalChildren = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(5),
                            Children = new Drawable[]
                            {
                                new Sprite
                                {
                                    Texture = textures.Get($"mods/{mods.ToLower()}"),
                                    Scale = new Vector2(0.25f)
                                },
                                new Container
                                {
                                    Size = new Vector2(50, 24),
                                    CornerRadius = 10,
                                    Masking = true,
                                    Children = new Drawable[]
                                    {
                                        new Box
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Colour = TournamentGame.ELEMENT_BACKGROUND_COLOUR,
                                        },
                                        new TournamentSpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Text = seeding.ToString("#,0"),
                                            Colour = TournamentGame.ELEMENT_FOREGROUND_COLOUR,
                                            Font = OsuFont.Torus.With(weight: FontWeight.SemiBold, size: 22)
                                        },
                                    }
                                },
                            }
                        },
                    };
                }
            }
        }

        private class LeftInfo : CompositeDrawable
        {
            public LeftInfo(TournamentTeam team)
            {
                FillFlowContainer fill;

                Width = 200;


                if (team == null) return;

                InternalChildren = new Drawable[]
                {
                    fill = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                            new TeamDisplay(team) { Margin = new MarginPadding { Bottom = 30 } },
                            new RowDisplay("Seed:", team.Seed.Value) { Margin = new MarginPadding { Bottom = 6 } },
                            new RowDisplay("Last year's seed:", team.LastYearPlacing.Value > 0 ? $"{team.LastYearPlacing:#,0}" : "N/A") { Margin = new MarginPadding { Bottom = 26 } },
                            new RowDisplay("Global Rank:", $"#{team.AverageRank:#,0}"),
                            new RowDisplay("Performance:", $"{team.Performance:#,0}pp"),
                        }
                    },
                };
            }

            internal class RowDisplay : CompositeDrawable
            {
                public RowDisplay(string left, string right)
                {
                    AutoSizeAxes = Axes.Y;
                    RelativeSizeAxes = Axes.X;

                    InternalChildren = new Drawable[]
                    {
                        new TournamentSpriteText
                        {
                            Text = left,
                            Colour = TournamentGame.TEXT_COLOUR,
                            Font = OsuFont.Torus.With(size: 26, weight: FontWeight.SemiBold),
                        },
                        new TournamentSpriteText
                        {
                            Text = right,
                            Colour = TournamentGame.TEXT_COLOUR,
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopLeft,
                            Font = OsuFont.Torus.With(size: 26, weight: FontWeight.Medium),
                        },
                    };
                }
            }

            private class TeamDisplay : DrawableTournamentTeam
            {
                public TeamDisplay(TournamentTeam team)
                    : base(team)
                {
                    AutoSizeAxes = Axes.Both;

                    Flag.RelativeSizeAxes = Axes.None;
                    Flag.Scale = new Vector2(2f);

                    InternalChild = new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 5),
                        Children = new Drawable[]
                        {
                            Flag,
                            new OsuSpriteText
                            {
                                Text = team?.FullName.Value ?? "???",
                                Font = OsuFont.Torus.With(size: 32, weight: FontWeight.SemiBold),
                                Colour = TournamentGame.TEXT_COLOUR,
                            },
                        }
                    };
                }
            }
        }
    }
}
