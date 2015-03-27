﻿#region Using Statements
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
    class Player
    {
        
        //IDEAL CLASS SET UP
        //Set up all player variables to be used
        //Create Player constructor
        //An Update function for keeping track of gametime
        //Another update for checking screenwidths
        //And a draw method
        //As well as collision and movement methods.

        #region Variables

        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Vector2 spriteOrigin;
        public Vector2 SpriteOrigin
        {
            get { return spriteOrigin; }
            set { spriteOrigin = value; }
        }

        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        protected Vector2 acceleration;
        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        protected Vector2 initialVelocity;
        public Vector2 InitialVelocity
        {
            get { return initialVelocity; }
            set { initialVelocity = value; }
        }

        public bool SetOrigin { get; set; }
        public float Scale { get; set; }

        protected SpriteEffects Spriteeffect { get; set; }
        
        //public float mass;
        public bool Alive { get; set; }

        // pixel collision
        public Color[] textureData;
        public bool collisionDetected;

        //Texture object and a collision rectangle
        public Texture2D TextureImage { get; set; }

        public virtual Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - SpriteOrigin.X * Scale), (int)(position.Y - SpriteOrigin.Y * Scale),
                   Convert.ToInt32(TextureImage.Width * Scale), Convert.ToInt32(TextureImage.Height * Scale));
            }
        }

        // weapons
        protected List<Weapon> primary;
        public List<Weapon> Primary
        {
            get { return primary; }
            set { primary = value; }
        }
        protected List<Weapon> secondary;
        public List<Weapon> Secondary
        {
            get { return secondary; }
            set { secondary = value; }
        }

        protected int primaryAmmo;
        public int PrimaryAmmo
        {
            get { return primaryAmmo; }
            set { primaryAmmo = value; }
        }

        protected int secondaryAmmo;
        public int SecondaryAmmo
        {
            get { return secondaryAmmo; }
            set { secondaryAmmo = value; }
        }

        protected string primaryType;
        protected string secondaryType;
        private bool hasShot;
        public bool HasShot
        {
            get { return hasShot; }
            set { hasShot = value; }
        }
        private bool forcePull;
        public bool ForcePull
        {
            get { return forcePull; }
            set { forcePull = value; }
        }
        #endregion

        public Player(Texture2D textureImage, Vector2 position, Vector2 velocity, bool setOrig, float scale)
        {
            Position = position;
            TextureImage = textureImage;
            InitialVelocity = velocity;
            Velocity = velocity;
            SetOrigin = setOrig;
            Acceleration = acceleration;
            if (SetOrigin)
            {
                SpriteOrigin = new Vector2(TextureImage.Width / 2, TextureImage.Height / 2);
            }
            Scale = scale;
            Alive = true;
            textureData = new Color[TextureImage.Width * TextureImage.Height];
            textureImage.GetData(textureData);
            primary =  new List<Weapon>();
            secondary = new List<Weapon>();
            setWeapon("gravityWell", 1);
            setWeapon("homingMissile", 2);
            hasShot = false;
            forcePull = false;
        }

        // Draws the ship and all projectiles currently in motion
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                spriteBatch.Draw(TextureImage,
                    position,
                    null,
                    Microsoft.Xna.Framework.Color.White,
                    0f,
                    SpriteOrigin,
                    Scale,
                    Spriteeffect,
                    0);

                foreach (Weapon shot in primary)
                {
                    shot.Draw(spriteBatch);
                }
                foreach (Weapon shot in secondary)
                {
                    shot.Draw(spriteBatch);
                }
            }
        }

        #region Update Methods
        //Update that doesn't keep sprites onscreen
        public virtual void Update(GameTime gameTime, List<Enemy> enemyWave)
        {
            if (Alive)
            {
                //Time between the frames
                float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);

                UpdateWeapon(primary, gameTime, enemyWave);
                UpdateWeapon(secondary, gameTime, enemyWave);

                //Move the sprite
                position += Velocity * timeLapse;
            }
        }
        public virtual void Update(Player player, Vector2 offset, GameTime gameTime)
        {
            offset = player.position + offset;
            float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);
            if (position !=  offset)
            {
                Vector2 vel = player.Position - position;
                vel.Normalize();
                position += vel * 250 * timeLapse;
            }
        }

        public virtual void Update(GameTime gameTime, GraphicsDevice Device, List<Enemy> enemyWave)
        {
            if (Alive)
            {
                //Keep the sprite onscreen
                Update(gameTime, enemyWave);

                if (Position.X > Device.Viewport.Width + SpriteOrigin.X * Scale - SpriteOrigin.X)
                {
                    position.X = 0 + SpriteOrigin.X * Scale;
                }
                else if (Position.X < SpriteOrigin.X * Scale - spriteOrigin.X)
                {
                    position.X = Device.Viewport.Width - SpriteOrigin.X * Scale;
                }

                if (Position.Y >= Device.Viewport.Height - SpriteOrigin.Y * Scale)
                {
                    position.Y = Device.Viewport.Height - SpriteOrigin.Y * Scale;
                    if (velocity.Y >= 0)
                    {
                        velocity.Y *= -0.95f;
                    }
                }
                else if (Position.Y < SpriteOrigin.Y * Scale)
                { 
                   velocity.Y *= -0.15f;
                }
                /*
                 If the velocity is less than 1000, increase it.
                 */
                 

                Console.WriteLine("{0}, {1}, {2}", position.Y, velocity.Y, velocity.X);
            }
        }

        // Updates ammo counts and removes weapons that are off screen
        public virtual void UpdateWeapon(List<Weapon> weapon, GameTime gameTime, List<Enemy> enemyWave)
        {
            for (int i = 0; i < weapon.Count; i++)
            {
                if (secondaryType == "homingMissile")
                {
                    weapon[i].Update(gameTime, enemyWave);
                }
                else
                {
                    weapon[i].Update(gameTime);
                }

                if (weapon[i].offScreen)
                {
                    // updates ammo count
                    if (weapon[i].WeaponType == 1)
                    {
                        primaryAmmo--;
                    }
                    else if (weapon[i].WeaponType == 2)
                    {
                        secondaryAmmo--;
                    }

                    weapon.RemoveAt(i);
                }
            }
        }
        #endregion

        #region Collision Detection Methods
        // Basic Collision Detection
        public bool CollisionSprite(Player sprite)
        {
            return CollisionRectangle.Intersects(sprite.CollisionRectangle);
        }

        // Pixel Collision Detection
        public bool IntersectsPixel(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
        {
            bool collision = false;
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color color1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                    Color color2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                    if (color1.A != 0 && color2.A != 0) // if both colors aren't transparent, collision detected
                    {
                        return true;
                    }
                }
            }
            return collision;
        }

        // Parses shot list for collisions
        public int CollisionShot(List<Weapon> shots)
        {
            for (int i = 0; i < shots.Count; i++)
            {
                if (IntersectsPixel(CollisionRectangle, textureData, shots[i].CollisionRectangle, shots[i].textureData))
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion

        #region Movement Methods
        /******* Movement methods *******/
        public void Up()
        {
            velocity.Y -= InitialVelocity.Y;;
        }

        public void Down()
        {
            velocity.Y += InitialVelocity.Y;;
        }

        public virtual void Right() 
        {
            if (velocity.X <= 1000)
            {
                velocity.X += InitialVelocity.X;
            }
        }

        public virtual void Left()
        {
            if (velocity.X >= -1000)
            {
                velocity.X -= InitialVelocity.X;
            }
            
        }
        public virtual void Idle()
        {
            Velocity = Velocity * .98f;
        }
        #endregion

        #region Weapon Methods
        public void setWeapon(string weaponType, int ammoCapacity)
        {
            // List of Primary Weapons


            // List of Secondary Weapons
            if (weaponType == "gravityWell")
            {
                secondaryType = weaponType;
                secondaryAmmo = ammoCapacity;
            }

            if (weaponType == "helixMissile")
            {
                secondaryType = weaponType;
                secondaryAmmo = ammoCapacity;
            }

            if (weaponType == "homingMissile")
            {
                secondaryType = weaponType;
                secondaryAmmo = ammoCapacity;
            }
        }

        // Chooses which primary weapon to shoot
        public virtual void shootPrimary(Texture2D weaponTexture)
        {
        }

        // Chooses which secondary weapon to shoot
        public virtual void shootSecondary(ContentManager content, Texture2D weaponTexture)
        {
            if (secondaryType == "gravityWell")
            {
                shootGravityWell(weaponTexture);
            }
            if (secondaryType == "helixMissile")
            {
                shootHelixMissile(weaponTexture);
            }
            if (secondaryType == "homingMissile")
            {
                shootHomingMissile(content, weaponTexture);
            }
        }
        #endregion

        #region Specific Weapon Methods
        /******* Weapon methods *******/
        public void shootGravityWell(Texture2D weaponTexture)
        {
            if (secondary.Count + 1 <= secondaryAmmo)
            {
                GravityWell shot = new GravityWell(weaponTexture, new Vector2(position.X, position.Y - spriteOrigin.Y), 100f);
                secondary.Add(shot);
                hasShot = true;
            }
        }

        public void shootHelixMissile(Texture2D weaponTexture)
        {
            if (primary.Count + 1 <= primaryAmmo)
            {
                //Weapon shot = new Weapon(weaponTexture, new Vector2(position.X, position.Y - spriteOrigin.Y), 600f); 
                Weapon left = new HelixMissile(weaponTexture, new Vector2(position.X, position.Y + 50f - spriteOrigin.Y), 10f, -1);
                primary.Add(left);
                Weapon right = new HelixMissile(weaponTexture, new Vector2(position.X, position.Y + 50f - spriteOrigin.Y), 10f, 1);
                primary.Add(right);
            }
        }

        public void shootHomingMissile(ContentManager content, Texture2D weaponTexture)
        {
            HomingMissile missile = new HomingMissile(content, weaponTexture, new Vector2(position.X, position.Y - spriteOrigin.Y), 500f);
            secondary.Add(missile);
        }
        #endregion
    }
}