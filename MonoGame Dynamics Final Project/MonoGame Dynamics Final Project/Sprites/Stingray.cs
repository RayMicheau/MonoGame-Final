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
            base(content, 80, 80, content.Load<Texture2D>("Images/Animations/Sting-Ray"), Device, spotinFormation, formationType, 0.5f, 100f, 300f)
        {
            mass = 1f;
            frameNum = 12;
            frameTime = 0.1f;
            Ai = EnemyState.Default;
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 400f);
            velocity = new Vector2(0, 50);
            enemyType = "stingRay";
            //EnemyShot = content.Load<Texture2D>("Images/Animations/Sting-Ray-shot");
            VectorSpeed = 3.0f;
            damage = 1;
            score = 100;
        }

        public override void Update(ContentManager content, GameTime gameTime, Player player)
        {
            elapsedShotTime += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            
            foreach (Weapon shot in primary)
            {     
                shot.Update(gameTime, player);
            }

            if(elapsedShotTime > 2 && !chasing)
            {
                elapsedShotTime = 0f;
                StingRayWeapon Stingshot = new StingRayWeapon(content, position);
                primary.Add(Stingshot);
            }
            if(elapsedShotTime > 2 && chasing)
            {
                elapsedShotTime = 0f;
                StingRayWeapon Stingshot = new StingRayWeapon(content, position);
                Stingshot.Velocity = getDirectionVector() * Stingshot.velocitySpeed;
                primary.Add(Stingshot);
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
        public override void UpdateWeapon(GameTime gameTime, Player player, Vector2 directionShot, ContentManager content)
        {
            base.UpdateWeapon(gameTime, player, directionShot, content);
            
            foreach(Weapon weapon in primary)
            {
                weapon.Update(gameTime);
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

        //public override void Draw(SpriteBatch spriteBatch, GameTime gameTime){
        //    Draw(spriteBatch, gameTime);
        //}
    }
}
