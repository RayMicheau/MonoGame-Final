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
        protected Vector2 closestEnemy;
        protected Vector2 startPosition;
        protected bool targeted;
        protected Texture2D crossHair;
        protected Enemy targetEnemy;
        protected float homingSpeed;

        public HomingMissile(ContentManager content, Vector2 startPosition, float velocity)
            : base(content.Load<Texture2D>("Images/Animations/rocket"), startPosition, velocity, 2)
        {
            this.startPosition = startPosition;
            targeted = false;            
            crossHair = content.Load<Texture2D>("Images/Animations/crossHair");
            homingSpeed = velocity;
        } 

        // Helper method: cycles through enemy list and finds the closest one, returns false if no enemies on screen
        public bool Target(List<Enemy> enemyWave)
        {
            targeted = false;

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

        // updates missile velocity to home in on targetted enemy
        public override void Update(GameTime gameTime, List<Enemy> enemyWave)
        {
            base.Update(gameTime);

            if (Target(enemyWave))
            {
                Vector2 newVelocity = targetEnemy.Position - position;
                newVelocity /= newVelocity.Length();
                velocity = newVelocity * homingSpeed;
            }
            if(targeted && !targetEnemy.Alive)
            {
                targeted = false;
            }
        }

        // Draws the crosshairs for targetted enemies
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
                                 1f);
            }
        }
    }
}
