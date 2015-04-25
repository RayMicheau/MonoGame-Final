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
    class Player:animateSprite
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

        protected float rotation;

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
                   Convert.ToInt32(frameWidth * Scale), Convert.ToInt32(frameHeight * Scale));
            }
        }

        public BoundingSphere collisionRange;

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
        protected int currentPrimaryAmmo;
        public int CurrentPrimaryAmmo
        {
            get { return currentPrimaryAmmo; }
            set { currentPrimaryAmmo = value; }
        }

        protected int currentSecondaryAmmo;
        public int CurrentSecondaryAmmo
        {
            get { return currentSecondaryAmmo; }
            set { currentSecondaryAmmo = value; }
        }  

        protected string primaryType;
        protected string secondaryType;
        private bool hasShot;
        public bool HasShot
        {
            get { return hasShot; }
            set { hasShot = value; }
        }
        private bool hasShotPrim;
        public bool HasShotPrim
        {
            get { return hasShotPrim; }
            set { hasShotPrim = value; }
        }
        private bool forcePull;
        public bool ForcePull
        {
            get { return forcePull; }
            set { forcePull = value; }
        }

        protected float health;
        public float Health
        {
            get { return health; }
            set { health = value; }
        }
        protected float damage;
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public float timer;
        public int frameWidth;
        public int frameHeight;

        public bool isMoving;

        #endregion

        public Player(int FrameWidth, int FrameHeight, Texture2D textureImage, Vector2 position, Vector2 velocity, bool setOrig, float scale, float damage, float health)
        {
            frameWidth = FrameWidth;
            frameHeight = FrameHeight;
            Position = position;
            TextureImage = textureImage;
            InitialVelocity = velocity;
            Velocity = velocity;
            SetOrigin = setOrig;
            Acceleration = acceleration;
            Health = health;
            Damage = damage;
      
            if (SetOrigin)
            {
                SpriteOrigin = new Vector2(FrameWidth / 2, FrameHeight / 2);
            }
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), frameWidth / 2);
            Scale = scale;
            rotation = 0f;
            Alive = true;
            isMoving = false;
            textureData = new Color[TextureImage.Width * TextureImage.Height];
            textureImage.GetData(textureData);
            primary =  new List<Weapon>();
            secondary = new List<Weapon>();
            
            // Set Primary Weapons

            // Set Secondary Weapons
            setWeapon("gravityWell", 1);
            //setWeapon("helixMissile", 2);
            setWeapon("homingMissile", 2);
            setWeapon("laser", 1);
            //setWeapon("rail", 4);
            hasShot = false;
            hasShotPrim = false;
            forcePull = false;
            health = 100;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {}
        
        // Draws the ship and all projectiles currently in motion
        public virtual void Draw(int framenum, float frameTime,SpriteBatch spriteBatch, GameTime gameTime)
        {
            float timeLapse = (gameTime.ElapsedGameTime.Milliseconds/1000f);
            if (Alive)
            {
                spriteBatch.Draw(TextureImage,
                    Position,
                    animatedSprite(framenum, frameTime, frameWidth, frameHeight, TextureImage, timeLapse),
                    Microsoft.Xna.Framework.Color.White,
                    rotation,
                    SpriteOrigin,
                    Scale*2,
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
        // Main update method
        public virtual void Update(GameTime gameTime, Player player)
        {

            float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);
            position += Velocity * timeLapse;
        }
        public virtual void Update(GameTime gameTime, List<Enemy> enemyWave)
        {
            if (Alive)
            {
                collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 100f);
                //Time between the frames
                float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);

                UpdateWeapon(primary, gameTime, enemyWave);
                UpdateWeapon(secondary, gameTime, enemyWave);

                //Move the sprite
                position += Velocity * timeLapse;
            }
        }

        // Follower Update. Confirmed. 
        public virtual void Update(Player player, GameTime gameTime)
        {
            //float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);

            //position.Y = player.position.Y;
            //position.X = player.position.X;
            //position += Velocity * timeLapse;
        }

        // Update methods for bounds checks
        public virtual void Update(GameTime gameTime, GraphicsDevice Device, List<Enemy> enemyWave)
        {
            if (Alive)
            {
                //Keep the sprite onscreen
                Update(gameTime, enemyWave);

                if (Position.X > Device.Viewport.Width + frameWidth)
                {
                    position.X = frameWidth * -1;
                }
                else if (Position.X < frameWidth*-1)
                {
                    position.X = Device.Viewport.Width + frameWidth;
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

        // Update helper: updates weapons, ammo counts and removes weapons that are off screen
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

                switch (primaryType)
                {
                    case "laser":
                        weapon[i].Update(gameTime);
                        break;

                    case "rail":
                        weapon[i].Update(gameTime);
                        break;

                    default:
                        weapon[i].Update(gameTime);
                        break;
                }

                if (weapon[i].offScreen)
                {
                    // updates ammo count
                    if (weapon[i].WeaponType == 1)
                    {
                        currentPrimaryAmmo++;
                    }
                    else if (weapon[i].WeaponType == 2)
                    {
                        currentSecondaryAmmo++;
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
                if (CollisionRectangle.Intersects(shots[i].CollisionRectangle))
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
            isMoving = true;
            velocity.Y -= InitialVelocity.Y;;
        }

        public void Down()
        {
            isMoving = true;
            velocity.Y += InitialVelocity.Y;;
        }

        public virtual void Right() 
        {
            isMoving = true;
            if (velocity.X <= 1000)
            {
                velocity.X += InitialVelocity.X;
            }
        }

        public virtual void Left()
        {
            isMoving = true;
            if (velocity.X >= -1000)
            {
                velocity.X -= InitialVelocity.X;
            }
            
        }
        public virtual void Idle()
        {
            isMoving = false;
            Velocity = Velocity * .98f;
        }
        #endregion

        #region Weapon Methods
        public void setWeapon(string weaponType, int ammoCapacity)
        {
            // List of Primary Weapons
            if (weaponType == "laser")
            {
                primaryType = weaponType;
                primaryAmmo = ammoCapacity;
                currentPrimaryAmmo = ammoCapacity;
            }

            if (weaponType == "rail")
            {
                primaryType = weaponType;
                primaryAmmo = ammoCapacity;
                currentPrimaryAmmo = ammoCapacity;
            }

            // List of Secondary Weapons
            if (weaponType == "gravityWell")
            {
                secondaryType = weaponType;
                secondaryAmmo = ammoCapacity;
                currentSecondaryAmmo = ammoCapacity;
            }

            if (weaponType == "helixMissile")
            {
                secondaryType = weaponType;
                secondaryAmmo = ammoCapacity;
                currentSecondaryAmmo = ammoCapacity;
            }

            if (weaponType == "homingMissile")
            {
                secondaryType = weaponType;
                secondaryAmmo = ammoCapacity;
                currentSecondaryAmmo = ammoCapacity; 
            }

           
        }

        // Chooses which primary weapon to shoot
        public virtual void shootPrimary(ContentManager content, GameTime gameTime)
        {
            if (currentPrimaryAmmo != 0)
            {
                if (primaryType == "laser")
                {
                    shootLaser(content, gameTime);
                }
                if (primaryType == "rail")
                {
                    shootRail(content, gameTime);
                }
            }
        }

        // Chooses which secondary weapon to shoot
        public virtual void shootSecondary(ContentManager content)
        {
            if (currentSecondaryAmmo != 0)
            {
                if (secondaryType == "gravityWell")
                {
                    shootGravityWell(content);
                }
                if (secondaryType == "helixMissile")
                {
                    shootHelixMissile(content);
                }
                if (secondaryType == "homingMissile")
                {
                    shootHomingMissile(content);
                }
            }
        }
        #endregion

        #region Specific Weapon Methods
        public void shootGravityWell(ContentManager content)
        {
            if (secondary.Count + 1 <= secondaryAmmo)
            {
                GravityWell shot = new GravityWell(content, new Vector2(position.X, position.Y - spriteOrigin.Y), 100f);
                secondary.Add(shot);
                hasShot = true;
            }
        }

        public void shootHelixMissile(ContentManager content)
        {
            if (primary.Count + 1 <= primaryAmmo)
            {
                //Weapon shot = new Weapon(weaponTexture, new Vector2(position.X, position.Y - spriteOrigin.Y), 600f); 
                Weapon left = new HelixMissile(content, new Vector2(position.X, position.Y + 50f - spriteOrigin.Y), 10f, -1);
                primary.Add(left);
                Weapon right = new HelixMissile(content, new Vector2(position.X, position.Y + 50f - spriteOrigin.Y), 10f, 1);
                primary.Add(right);
            }
        }

        public void shootHomingMissile(ContentManager content)
        {
            HomingMissile missile = new HomingMissile(content, new Vector2(position.X, position.Y - spriteOrigin.Y), 500f);
            secondary.Add(missile);
        }

        public void shootLaser(ContentManager content, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if (hasShotPrim && timer <= 0.5f)
            {
                Velocity = new Vector2(0.0f, 0.0f);
                timer = 0.0f;
                BasicLaser laser = new BasicLaser(content, new Vector2(position.X, position.Y - spriteOrigin.Y), 500f);
                primary.Add(laser);
                currentPrimaryAmmo--;
            }
            if (timer > 0.8f)
            {
                timer = 0.0f;
            }
        }

        public void shootRail(ContentManager content, GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if (hasShotPrim && timer <= 0.1f)
            {
                Rail rail = new Rail(content, new Vector2(position.X, position.Y - spriteOrigin.Y), 3000f);
                primary.Add(rail);
                Velocity = new Vector2(0.0f, 0.0f);
            }
            if (timer > 2.0f)
            {
                timer = 0.0f;
            }
        }
        #endregion

        #region Reinforcement Methods
        #endregion
    }
}
