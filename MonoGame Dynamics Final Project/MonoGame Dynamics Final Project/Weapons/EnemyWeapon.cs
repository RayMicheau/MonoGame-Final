using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGame_Dynamics_Final_Project.Weapons
{
    class EnemyWeapon : Weapon
    {
        protected int shotFrameIndex = 1;
        protected int shotX = 0;
        protected float shotTime, shotSpeed;
        public int shotFrameWidth, shotFrameHeight, shotFrames;
        protected Rectangle shotRectangle;
        new protected float scale;
        Texture2D shotTexture;

        public EnemyWeapon(Texture2D ShotTexture, Vector2 startPosition, float velocity)
            :base(ShotTexture, startPosition, velocity, 1)
        {
            shotTexture = ShotTexture;
            scale = 1f;
        }
        public override void Update(GameTime gameTime, Sprites.Player player)
        {
            base.Update(gameTime, player);
            float time = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            shotRectangle = animatedSprite(shotFrames, shotSpeed, shotFrameWidth, shotFrameHeight, shotTexture, time);
            
            
        }
        public Rectangle animatedSprite(int frames, float frameTime, int frameWidth, int frameHeight, Texture2D image, float timeLapse)
        {
            if (shotFrameIndex == frames + 1)
            {
                shotFrameIndex = 1;
                shotX = 0;
            }
            // Calculate the source rectangle of the current frame.

            Rectangle source = new Rectangle(shotX * frameWidth, 0, frameWidth, frameHeight);

            shotTime += timeLapse;
            while (shotTime > frameTime)
            {
                // Play the next frame in the SpriteSheet
                shotFrameIndex++;
                shotX++;

                // reset elapsed time
                shotTime = 0f;
            }
            return source;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureImage,
                position,
                shotRectangle,
                Microsoft.Xna.Framework.Color.White,
                angle,
                SpriteOrigin,
                scale, // scale
                SpriteEffects.None,
                1f);
        }
        
    }
}
