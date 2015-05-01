#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Dynamics_Final_Project.Weapons;
using MonoGame_Dynamics_Final_Project;
#endregion

namespace MonoGame_Dynamics_Final_Project.Sprites
{

    public enum RayState
    {
        Default,
        Chase
    }

    class Stingray2 : Enemy
    {
        public RayState Ai;
        float elapsedShotTime;
        bool chasing;
        public Stingray2(ContentManager content, GraphicsDevice Device, int spotinFormation, string formationType) :
            base(content, 80, 80, content.Load<Texture2D>("Images/Animations/Sting-Ray-2"), Device, spotinFormation, formationType, 0.5f)
        {
            frameNum = 12;
            frameTime = 0.1f;
            Ai = RayState.Default;
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 400f);
            velocity = new Vector2(0, 100);
            enemyType = "stingRay2";
            VectorSpeed = 5.0f;
            score = 600f;
            damage = 4f;
            health = 800f;
            mass = 2f;
        }

        public override void Update(ContentManager content, GameTime gameTime, Player player)
        {
            elapsedShotTime += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;


            foreach (Weapon shot in primary)
            {
                shot.Update(gameTime, player);
            }

            if (elapsedShotTime > 2)
            {
                elapsedShotTime = 0f;
                shootPrimary(content);
            }

            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 400f);
           
            setAi(player);
            switch (Ai)
            {
                case RayState.Chase:
                    ChasePlayer(gameTime, player);
                    break;
                case RayState.Default:

                    base.Update(gameTime, player);
                    break;
            }
        }
        public override void UpdateWeapon(GameTime gameTime, Player player, Vector2 directionShot, ContentManager content)
        {
            base.UpdateWeapon(gameTime, player, directionShot, content);
            StingRayWeapon weapon = new StingRayWeapon(content, player.Position);
            primary.Add(weapon);
        }
        public void setAi(Player player)
        {
            if (collisionRange.Intersects(player.collisionRange))
            {
                Ai = RayState.Chase;
            }
            else
            {
                Ai = RayState.Default;

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


