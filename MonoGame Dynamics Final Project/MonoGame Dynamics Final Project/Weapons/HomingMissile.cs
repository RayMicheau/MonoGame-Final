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
    class HomingMissile : Weapon
    {
        Vector2 closestEnemy;
        Vector2 startPosition;
        bool targeted;
        Texture2D crossHair;
        Enemy targetEnemy;
        float velocity;

        public HomingMissile(ContentManager content, Texture2D textureImage, Vector2 startPosition, float velocity)
            : base(textureImage, startPosition, velocity, 2)
        {
            this.startPosition = startPosition;
            targeted = false;            
            crossHair = content.Load<Texture2D>("Images/Animations/crossHair");
            this.velocity = velocity;
        } 

        public bool Target(List<Enemy> enemyWave)
        {
            if (enemyWave.Count != 0)
            {
                targetEnemy = enemyWave[0];
                closestEnemy = enemyWave[0].Position - startPosition;
                foreach (Enemy enemy in enemyWave)
                {
                    Vector2 length = enemy.Position - startPosition;
                    if (length.Length() < closestEnemy.Length())
                    {
                        closestEnemy = length;
                        targetEnemy = enemy;
                        targeted = true;
                    }
                }
            }

            return targeted;
        }

        public override void Update(GameTime gameTime, List<Enemy> enemyWave)
        {
            base.Update(gameTime);
            Target(enemyWave);
            if (targeted)
            {
                Vector2 newVelocity = targetEnemy.Position - position;
                float magnitude = newVelocity.Length();
                newVelocity /= magnitude;
                base.velocity = newVelocity * this.velocity;
            }
            if(targeted && !targetEnemy.Alive)
            {
                targeted = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(targeted)
            {
                spriteBatch.Draw(crossHair, 
                                 targetEnemy.Position, 
                                 null, 
                                 Color.White, 
                                 0f, 
                                 spriteOrigin, 
                                 1f, 
                                 SpriteEffects.None, 
                                 0f);
            }
        }
    }
}
