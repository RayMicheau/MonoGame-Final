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
using MonoGame_Dynamics_Final_Project;
using MonoGame_Dynamics_Final_Project.Weapons;

namespace MonoGame_Dynamics_Final_Project.Sprites
{
    public enum EnemyState
    {
        Default,
        Chase
    }

    class Stingray : Enemy
    {
        public EnemyState Ai;
        float elapsedShotTime;
        bool chasing;

        public Stingray(ContentManager content, GraphicsDevice Device, int spotinFormation, string formationType) :
            base(content, 80, 80, content.Load<Texture2D>("Images/Animations/Sting-Ray"), Device, spotinFormation, formationType, 0.5f)
        {
            mass = 1f;
            frameNum = 12;
            frameTime = 0.1f;
            Ai = EnemyState.Default;
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 400f);
            velocity = new Vector2(0, 50);
            enemyType = "stingRay";
            VectorSpeed = 2.0f;
            damage = 1f;
            score = 100;
        }

        public override void Update(ContentManager content, GameTime gameTime, Player player)
        {
            elapsedShotTime += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            
            
            foreach (Weapon shot in primary)
            {     
                shot.Update(gameTime, player);
            }

            if(elapsedShotTime > 2)
            {
                elapsedShotTime = 0f;
                shootPrimary(content);
            }

            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 400f);
            setAi(player);

            switch(Ai)
            {
                case EnemyState.Chase :
                    ChasePlayer(gameTime, player);
                    chasing = true;
                    break;
                case EnemyState.Default :
                    chasing = false;
                    base.Update(gameTime, player);
 
                    break;
            }
        }

        public void setAi(Player player)
        {
            if (collisionRange.Intersects(player.collisionRange))
            {
                Ai = EnemyState.Chase;
            }
            else
            {
                Ai = EnemyState.Default;            
            }
        }

        public override void shootPrimary(ContentManager content)
        {
            StingRayWeapon Stingshot = new StingRayWeapon(content, position + spriteOrigin);
            Stingshot.velocitySpeed = 100f;
            Stingshot = new StingRayWeapon(content, position + spriteOrigin + Stingshot.SpriteOrigin);     
            Stingshot.Velocity = getDirectionVector() * Stingshot.velocitySpeed;
            Stingshot.Angle = MathHelper.PiOver2 + (float)Math.Atan2(Stingshot.Velocity.Y, Stingshot.Velocity.X);
            primary.Add(Stingshot);
        }
    }
}
