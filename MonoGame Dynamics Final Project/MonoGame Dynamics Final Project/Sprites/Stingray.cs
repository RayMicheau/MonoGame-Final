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

        public Stingray(ContentManager content, GraphicsDevice Device, int spotinFormation, string formationType) :
            base(80, 80, content.Load<Texture2D>("Images/Animations/Sting-Ray"), Device, spotinFormation, formationType, 0.5f)
        {
            Ai = EnemyState.Default;
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 400f);
        }

        public override void Update(GameTime gameTime, Player player)
        {
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 400f);
            setAi(player);
            switch(Ai)
            {
                case EnemyState.Chase :
                    ChasePlayer(player.Position);
                    break;
                case EnemyState.Default :
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

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime){
            Draw(12, 0.1f, spriteBatch, gameTime);
        }
    }
}
