// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Game.Graphics;
using osu.Game.Tournament.Components;
using osu.Game.Tournament.Models;
using osu.Game.Tournament.Screens.Drawings.Components;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Tournament.Screens.Drawings
{
    public class DrawingsScreen : TournamentScreen
    {
        private const string results_filename = "drawings_results.txt";

        private ScrollingTeamContainer teamsContainer;
        private GroupContainer groupsContainer;
        private TournamentSpriteText fullTeamNameText;
        private TournamentSpriteText playerOneText;
        private TournamentSpriteText playerTwoText;
        private TournamentSpriteText playerThreeText;
        private TournamentSpriteText playerFourText;
        private TournamentSpriteText playerFiveText;
        private TournamentSpriteText playerSixText;
        private DrawableTournamentHeaderLogo logo;

        private readonly List<TournamentTeam> allTeams = new List<TournamentTeam>();

        private DrawingsConfigManager drawingsConfig;

        private Task writeOp;

        private Storage storage;

        public ITeamList TeamList;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage storage)
        {
            RelativeSizeAxes = Axes.Both;

            this.storage = storage;

            TeamList ??= new StorageBackedTeamList(storage);

            if (!TeamList.Teams.Any())
            {
                return;
            }

            drawingsConfig = new DrawingsConfigManager(storage);

            InternalChildren = new Drawable[]
            {
                // Main container
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            FillMode = FillMode.Fill,
                            Texture = textures.Get(@"Backgrounds/Drawings/background.png")
                        },
                        logo = new DrawableTournamentHeaderLogo
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Alpha = 1,
                            Position = new Vector2(0, 35f)
                        },
                        // Visualiser
                        new VisualiserContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,

                            RelativeSizeAxes = Axes.X,
                            Size = new Vector2(1, 10),
                            Position = new Vector2(0, -80f),

                            Colour = new Color4(255, 255, 255, 255),

                            Lines = 6
                        },
                        // Groups
                        groupsContainer = new GroupContainer(drawingsConfig.Get<int>(DrawingsConfig.Groups), drawingsConfig.Get<int>(DrawingsConfig.TeamsPerGroup))
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,

                            RelativeSizeAxes = Axes.Y,
                            AutoSizeAxes = Axes.X,

                            Padding = new MarginPadding
                            {
                                Top = 380f,
                                Bottom = 35f
                            }
                        },
                        // Scrolling teams
                        teamsContainer = new ScrollingTeamContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Position = new Vector2(0, -80f),

                            RelativeSizeAxes = Axes.X,
                        },
                        // Scrolling team name
                        fullTeamNameText = new TournamentSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.TopCentre,

                            Position = new Vector2(0, -200f),

                            Colour = OsuColour.Gray(0.95f),

                            Alpha = 0,

                            Font = OsuFont.Torus.With(weight: FontWeight.SemiBold, size: 46),
                        },
                        playerOneText = new TournamentSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.TopCentre,

                            Position = new Vector2(0, -220f),

                            Colour = OsuColour.Gray(0.95f),

                            Alpha = 0,

                            Font = OsuFont.Torus.With(weight: FontWeight.Light, size: 36),
                        },
                        playerTwoText = new TournamentSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.TopCentre,

                            Position = new Vector2(0, -190f),

                            Colour = OsuColour.Gray(0.95f),

                            Alpha = 0,

                            Font = OsuFont.Torus.With(weight: FontWeight.Light, size: 36),
                        },
                        playerThreeText = new TournamentSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.TopCentre,

                            Position = new Vector2(0, -160f),

                            Colour = OsuColour.Gray(0.95f),

                            Alpha = 0,

                            Font = OsuFont.Torus.With(weight: FontWeight.Light, size: 36),
                        },
                        playerFourText = new TournamentSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.TopCentre,

                            Position = new Vector2(0, -130f),

                            Colour = OsuColour.Gray(0.95f),

                            Alpha = 0,

                            Font = OsuFont.Torus.With(weight: FontWeight.Light, size: 36),
                        },
                        playerFiveText = new TournamentSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.TopCentre,

                            Position = new Vector2(0, -100f),

                            Colour = OsuColour.Gray(0.95f),

                            Alpha = 0,

                            Font = OsuFont.Torus.With(weight: FontWeight.Light, size: 36),
                        },
                        playerSixText = new TournamentSpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.TopCentre,

                            Position = new Vector2(0, -70f),

                            Colour = OsuColour.Gray(0.95f),

                            Alpha = 0,

                            Font = OsuFont.Torus.With(weight: FontWeight.Light, size: 36),
                        }
                    }
                },
                // Control panel container
                new ControlPanel
                {
                    new TourneyButton
                    {
                        RelativeSizeAxes = Axes.X,

                        Text = "Begin random",
                        Action = teamsContainer.StartScrolling,
                    },
                    new TourneyButton
                    {
                        RelativeSizeAxes = Axes.X,

                        Text = "Stop random",
                        Action = teamsContainer.StopScrolling,
                    },
                    new TourneyButton
                    {
                        RelativeSizeAxes = Axes.X,

                        Text = "Reload",
                        Action = reloadTeams
                    },
                    new ControlPanel.Spacer(),
                    new TourneyButton
                    {
                        RelativeSizeAxes = Axes.X,

                        Text = "Reset",
                        Action = () => reset()
                    }
                }
            };

            teamsContainer.OnSelected += onTeamSelected;
            teamsContainer.OnScrollStarted += () => fullTeamNameText.FadeOut(200);
            teamsContainer.OnScrollStarted += () => playerOneText.FadeOut(200);
            teamsContainer.OnScrollStarted += () => playerTwoText.FadeOut(200);
            teamsContainer.OnScrollStarted += () => playerThreeText.FadeOut(200);
            teamsContainer.OnScrollStarted += () => playerFourText.FadeOut(200);
            teamsContainer.OnScrollStarted += () => playerFiveText.FadeOut(200);
            teamsContainer.OnScrollStarted += () => playerSixText.FadeOut(200);


            reset(true);
        }

        private void onTeamSelected(TournamentTeam team)
        {
            groupsContainer.AddTeam(team);

            fullTeamNameText.Text = team.FullName.Value;
            fullTeamNameText.FadeIn(200);
            playerOneText.Text = team.PlayerOne.Value;
            playerOneText.FadeIn(200);
            playerTwoText.Text = team.PlayerTwo.Value;
            playerTwoText.FadeIn(200);
            playerThreeText.Text = team.PlayerThree.Value;
            playerThreeText.FadeIn(200);
            playerFourText.Text = team.PlayerFour.Value;
            playerFourText.FadeIn(200);
            playerFiveText.Text = team.PlayerFive.Value;
            playerFiveText.FadeIn(200);
            playerSixText.Text = team.PlayerSix.Value;
            playerSixText.FadeIn(200);

            writeResults(groupsContainer.GetStringRepresentation());
        }

        private void writeResults(string text)
        {
            void writeAction()
            {
                try
                {
                    // Write to drawings_results
                    using (Stream stream = storage.GetStream(results_filename, FileAccess.Write, FileMode.Create))
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(text);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to write results.");
                }
            }

            writeOp = writeOp?.ContinueWith(t => { writeAction(); }) ?? Task.Run(writeAction);
        }

        private void reloadTeams()
        {
            teamsContainer.ClearTeams();
            allTeams.Clear();

            foreach (TournamentTeam t in TeamList.Teams)
            {
                if (groupsContainer.ContainsTeam(t.FullName.Value))
                    continue;

                allTeams.Add(t);
                teamsContainer.AddTeam(t);
            }
        }

        private void reset(bool loadLastResults = false)
        {
            groupsContainer.ClearTeams();

            reloadTeams();

            if (!storage.Exists(results_filename))
                return;

            if (loadLastResults)
            {
                try
                {
                    // Read from drawings_results
                    using (Stream stream = storage.GetStream(results_filename, FileAccess.Read, FileMode.Open))
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string line;

                        while ((line = sr.ReadLine()?.Trim()) != null)
                        {
                            if (string.IsNullOrEmpty(line))
                                continue;

                            if (line.ToUpperInvariant().StartsWith("GROUP", StringComparison.Ordinal))
                                continue;

                            // ReSharper disable once AccessToModifiedClosure
                            TournamentTeam teamToAdd = allTeams.FirstOrDefault(t => t.FullName.Value == line);

                            if (teamToAdd == null)
                                continue;

                            groupsContainer.AddTeam(teamToAdd);
                            teamsContainer.RemoveTeam(teamToAdd);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to read last drawings results.");
                }
            }
            else
            {
                writeResults(string.Empty);
            }
        }
    }
}
