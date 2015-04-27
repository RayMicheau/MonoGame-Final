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

        public Stingray2(ContentManager content, GraphicsDevice Device, int spotinFormation, string formationType) :
            base(content, 80, 80, content.Load<Texture2D>("Images/Animations/Sting-Ray-2"), Device, spotinFormation, formationType, 0.5f, 100f, 500f)
        {
            frameNum = 12;
            frameTime = 0.1f;
            Ai = RayState.Default;
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 400f);
            velocity = new Vector2(0, 20);
            enemyType = "stingRay";
            //EnemyShot = content.Load<Texture2D>("Images/Animations/Sting-Ray-shot");
            VectorSpeed = 3.0f;
            damage = 3;
        }

        public override void Update(GameTime gameTime, Player player)
        {
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

        //public override void Draw(SpriteBatch spriteBatch, GameTime gameTime){
        //    Draw(spriteBatch, gameTime);
        //}
    }
}


