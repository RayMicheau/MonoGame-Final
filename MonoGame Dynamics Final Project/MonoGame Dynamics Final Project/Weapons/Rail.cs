#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame_Dynamics_Final_Project;
using MonoGame_Dynamics_Final_Project.Sprites;
#endregion

namespace MonoGame_Dynamics_Final_Project.Weapons
{
    class Rail : Weapon
    {
        public int orientation;
        SpriteEffects spriteEffect;
        float elapsedTime;

        public Rail(ContentManager content, Vector2 startPosition, float velocity, int Orientation) // 1 right, -1 left
            : base(content.Load<Texture2D>("Images/Animations/Plasma-Repeater-Shot"), startPosition, velocity, 1)
        {
            angle = 0;
            damage = 500;
            scale = 2f;
            orientation = Orientation;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;          
            if(elapsedTime > 0.2f)
            {
                if(spriteEffect == SpriteEffects.None)
                {
                    spriteEffect = SpriteEffects.FlipHorizontally;
                }
                else if (spriteEffect == SpriteEffects.FlipHorizontally)
                {
                    spriteEffect = SpriteEffects.None;
                }
                elapsedTime = 0f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureImage,
                             position,
                             null,
                             Color.White,
                             angle,
                             spriteOrigin,
                             scale,
                             spriteEffect,
                             1f);
        }
    }
}
