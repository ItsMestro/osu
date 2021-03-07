// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Tournament.Models;
using osuTK;

namespace osu.Game.Tournament.Components
{
    public class DrawableTeamFlag : Container
    {
        private readonly TournamentTeam team;

        [UsedImplicitly]
        private Bindable<string> flag;
        private Bindable<string> flag2;

        private Sprite flagSprite;
        private Sprite flagSprite2;

        private FillFlowContainer flow;
        private FillFlowContainer flow2;

        public DrawableTeamFlag(TournamentTeam team)
        {
            this.team = team;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            if (team == null) return;


            InternalChildren = new Drawable[]
            {
                flow = new FillFlowContainer
                {
                    Size = new Vector2(38, 50),
                    Masking = true,
                    //CornerRadius = 5,
                    /*Position = new Vector2(-1,0),*/
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Child = flagSprite = new Sprite
                    {
                        Blending = BlendingParameters.Mixture,
                        Size = new Vector2(75, 50),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                    }
                },
                flow2 = new FillFlowContainer
                {
                    Size = new Vector2(38, 50),
                    Masking = true,
                    //CornerRadius = 5,
                    /*Position = new Vector2(10,0),*/
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Child = flagSprite2 = new Sprite
                    {
                        Blending = BlendingParameters.Mixture,
                        Size = new Vector2(75, 50),
                        Origin = Anchor.CentreRight,
                        Anchor = Anchor.CentreRight,
                    }
                },
            };

            /*Size = new Vector2(75, 50);
            Masking = true;
            CornerRadius = 5;
            Child = flagSprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fill
            };*/

            (flag = team.FlagName.GetBoundCopy()).BindValueChanged(acronym => flagSprite.Texture = textures.Get($@"Flags/{team.FlagName}"), true);
            (flag2 = team.FlagName2.GetBoundCopy()).BindValueChanged(acronym => flagSprite2.Texture = textures.Get($@"Flags/{team.FlagName2}"), true);
        }
    }
}
