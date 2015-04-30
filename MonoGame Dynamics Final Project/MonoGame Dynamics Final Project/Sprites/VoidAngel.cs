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

namespace MonoGame_Dynamics_Final_Project.Sprites
{
    public enum AngelState
    {
        Idle,
        Chase,
        Attack
    }
    class VoidAngel : Enemy 
    {
       ContentManager Content; 
        public AngelState angelState;
        protected Vector2 distanceBetween;
        protected float elapsedTime;
        public float ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }
        public VoidAngel(ContentManager content, GraphicsDevice Device, int spotinFormation, string formationType) :
            base(content,100, 100, content.Load<Texture2D>("Images/Animations/Void-Angel"), Device, spotinFormation, formationType, 0.5f)
        {
           
            frameNum = 7;
            frameTime = 0.1f;
            Content = content;
            enemyType = "voidAngel";
            angelState = AngelState.Idle;
            velocity = new Vector2(0.0f, 100.0f);
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 300f);
            //EnemyShot = Content.Load<Texture2D>("Images/Animations/Void-angel-shot");
            score = 500;
            VectorSpeed = 3.0f;
            damage = 3f;
            health = 500;
            mass = 3f;
        }
        public override void Update(GameTime gameTime, Player player)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            Console.WriteLine("angel shot:" + elapsedTime);
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 300f);

            setAngel(player);
            switch (angelState)
            {
                case AngelState.Idle:
                    base.Update(gameTime, player);
                    TextureImage = Content.Load<Texture2D>("Images/Animations/Void-Angel");
                    break;
                case AngelState.Chase:
                    ChasePlayer(gameTime, player);
                    TextureImage = Content.Load<Texture2D>("Images/Animations/void-angel-Attack");
                    break;
                //case AngelState.Attack:
                //    base.Update(gameTime, player);
                //    TextureImage = Content.Load<Texture2D>("Images/Animations/void-angel-Attack");
                //    break;
            }
        }

        public void setAngel(Player player)
        {
            distanceBetween = player.Position - position;
            float distanceLength = distanceBetween.Length();
            if (collisionRange.Intersects(player.collisionRange))
            {
                angelState = AngelState.Chase;
            }
            else
            {
                angelState = AngelState.Idle;
            }
          //  if (elapsedTime > 5.0f)
          //  {
           //     angelState = AngelState.Attack;
           //     frameNum = 14;
           //     velocity = Vector2.Zero;
                //if (frameIndex == 13)
                //{

                //}
                
           // }
        }
             

    }
}
