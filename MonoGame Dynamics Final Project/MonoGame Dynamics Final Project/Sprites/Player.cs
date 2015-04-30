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
        public bool isTurning;
        public int turnOrientation;

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
        protected string secondaryType;
        public string SecondaryType
        {
            get { return secondaryType; }
            set { secondaryType = value; }
        }
        protected string primaryType;
        public string PrimaryType
        {
            get { return primaryType; }
            set { primaryType = value; }
        } 

        public bool SetOrigin { get; set; }
        public float Scale { get; set; }

        public float rotation;

        protected SpriteEffects Spriteeffect { get; set; }
        
        //public float mass;
        public bool Alive { get; set; }

        // animation
        public int frameNum;
        public float frameTime;

        // pixel collision
        public bool collisionDetected;
        public Rectangle source;
        public Color[] textureData;

        //Texture object and a collision rectangle
        public Texture2D TextureImage { get; set; }

        public float AtkSpeed;
        public float MoveSpeed = 1000.0f;

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

        public int flashCounter = 0;
        public Color hurtFlash = Color.White;
        public bool painSwitch = false;
        protected RailTurret railLeft, railRight;
        public RailTurret RailLeft
        {
            get { return railLeft; }
            set { railLeft = value; }
        }

        public RailTurret RailRight
        {
            get { return railRight; }
            set { railRight = value; }
        }
        protected Texture2D railTurretImage;
        //rail
        Rectangle extractRegion;
        #endregion

        public Player(int FrameWidth, int FrameHeight, Texture2D textureImage, Vector2 position, Vector2 velocity, bool setOrig, float scale, float health)
        {
            frameWidth = FrameWidth;
            frameHeight = FrameHeight;
            source = new Rectangle(0, 0, frameWidth, frameHeight);
            frameNum = 4;
            frameTime = 0.2f;
            TextureImage = textureImage;
            
            //textureImage.GetData<Color>(0, source, textureData, 0, source.Width * source.Height);
            AtkSpeed = .7f;
            

            Position = position;
            
            InitialVelocity = velocity;
            Velocity = velocity;
            SetOrigin = setOrig;
            Acceleration = acceleration;
            Health = health;
            Damage = damage;
            Scale = scale;
            //if (SetOrigin)
            //{
                SpriteOrigin = new Vector2(FrameWidth * scale/ 2, FrameHeight * scale / 2);
            //}
            collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), frameWidth / 2);
            
            rotation = 0f;
            Alive = true;
            isMoving = false;
            primary =  new List<Weapon>();
            secondary = new List<Weapon>();
            
            // Set Default Weapons
            setWeapon("rail", 5);
            hasShot = false;
            hasShotPrim = false;
            forcePull = false;
            isTurning = false;
            turnOrientation = 1;
        }

        public Player(int FrameWidth, int FrameHeight, Texture2D textureImage, Texture2D turretImage, Vector2 position, Vector2 velocity, bool setOrig, float scale, float health)
            : this(FrameWidth, FrameHeight, textureImage, position, velocity, setOrig, scale, health)
        {
            railTurretImage = turretImage;
            railLeft = new RailTurret(railTurretImage, position, velocity, 1);
            railRight = new RailTurret(railTurretImage, position, velocity, -1);
        }
        
        // Draws the ship and all projectiles currently in motion
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, Color color)
        {
            float timeLapse = (gameTime.ElapsedGameTime.Milliseconds/1000f);
            if (Alive)
            {
                foreach (Weapon shot in secondary)
                {
                    shot.Draw(spriteBatch);
                }

                if (railLeft != null && railRight != null)
                {
                    foreach (Weapon shot in railLeft.primary)
                    {
                        shot.Draw(spriteBatch);
                    }
                    foreach(Weapon shot in railRight.primary)
                    {
                        shot.Draw(spriteBatch);
                    }

                    railLeft.Draw(spriteBatch, gameTime, Color.White);
                    railRight.Draw(spriteBatch, gameTime, Color.White);
                }

                spriteBatch.Draw(TextureImage,
                    position,
                    source,
                    color,
                    rotation,
                    spriteOrigin,
                    Scale*2,
                    Spriteeffect,
                    0.0f);
            }
        }

        #region Update Methods

    
        public virtual void Update(GameTime gameTime, List<Enemy> enemyWave)
        {
            if (Alive)
            {
                collisionRange = new BoundingSphere(new Vector3(position.X + spriteOrigin.X, position.Y + spriteOrigin.Y, 0), 100f);
                //Time between the frames
                float timeLapse = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                source = animatedSprite(frameNum, frameTime, frameWidth, frameHeight, TextureImage, timeLapse);
            //    TextureImage.GetData<Color>(0, source, textureData, 0, source.Width * source.Height);

                //Move the sprite
                position += Velocity * timeLapse;

                
                if(isTurning)
                {
                    if(turnOrientation == 1)
                    {
                        Vector2 pos = position;
                        pos.X -= railLeft.offset.X * 2;
                        railRight.Update(gameTime, pos);
                    }
                    else if(turnOrientation == -1)
                    {
                        railLeft.Update(gameTime, new Vector2(position.X + railRight.offset.X, position.Y));
                    }
                }

                railRight.Update(gameTime, position);
                railLeft.Update(gameTime, position);

            }
        }

        // Follower Update. Confirmed. 
        public virtual void Update(Player player, GameTime gameTime)
        {
            float timeLapse = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            source = animatedSprite(frameNum, frameTime, frameWidth, frameHeight, TextureImage, timeLapse);
           // TextureImage.GetData<Color>(0, source, textureData, 0, source.Width * source.Height);
            if (position.Y > 0)
            {
                Alive = true;
            }
            //position.Y = player.position.Y;
            //position.X = player.position.X;
            //position += Velocity * timeLapse;
        }

        // Update methods for bounds checks
        public virtual void Update(GameTime gameTime, Rectangle virtualSize, List<Enemy> enemyWave)
        {
            float timeLapse = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            //update weapons
            railRight.UpdateWeapon(railRight.primary, gameTime, enemyWave, virtualSize);
            railLeft.UpdateWeapon(railLeft.primary, gameTime, enemyWave, virtualSize);
            UpdateWeapon(secondary, gameTime, enemyWave, virtualSize);

         //   TextureImage.GetData<Color>(0, source, textureData, 0, source.Width * source.Height);
            if (Alive)
            {
                //Keep the sprite onscreen
                Update(gameTime, enemyWave);
                
                if (Position.X > virtualSize.Width + frameWidth)
                {
                    position.X = frameWidth * -1;
                }
                else if (Position.X < frameWidth*-1)
                {
                    position.X = virtualSize.Width + frameWidth;
                }

                if (Position.Y >= virtualSize.Height - SpriteOrigin.Y * Scale)
                {
                    position.Y = virtualSize.Height - SpriteOrigin.Y * Scale;
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
                 

                //Console.WriteLine("{0}, {1}, {2}", position.Y, velocity.Y, velocity.X);
            }
        }

        // Update helper: updates weapons, ammo counts and removes weapons that are off screen
        public virtual void UpdateWeapon(List<Weapon> weapon, GameTime gameTime, List<Enemy> enemyWave, Rectangle virtualSize)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            for (int i = 0; i < weapon.Count; i++)
            {
                if (secondaryType == "homingMissile")
                {
                    weapon[i].Update(gameTime, enemyWave);
                }
                if(secondaryType == "helixMissile")
                {
                    weapon[i].Update(gameTime);
                }
                else
                {
                    weapon[i].Update(gameTime);
                }

                switch (primaryType)
                {
                    case  "laser":
                        weapon[i].Update(gameTime);
                        break;

                    case "rail":
                        weapon[i].Update(gameTime, rotation);
                        break;

                    default:
                        weapon[i].Update(gameTime);
                        break;
                }

                if (weapon[i].Position.Y + weapon[i].TextureImage.Height < 0 || weapon[i].Position.Y > virtualSize.Height)
                {
                    weapon[i].offScreen = true;
                }
                else if (weapon[i].Position.X + weapon[i].TextureImage.Width > virtualSize.Width || weapon[i].Position.X < 0)
                {
                    weapon[i].offScreen = true;
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
        // turret update
        public virtual void Update(GameTime gameTime, Vector2 playerPos)
        {

        }
        #endregion

        #region Collision Detection Methods
        // Basic Collision Detection
        public bool CollisionSprite(Player sprite)
        {
            return CollisionRectangle.Intersects(sprite.CollisionRectangle);
        }

        // Pixel Collision Detection
        public bool IntersectsPixel(Player enemy)
        {
            extractRegion = source;
            textureData = new Color[source.Width * source.Height];
            TextureImage.GetData<Color>(0, extractRegion, textureData, 0, source.Width * source.Height);

            enemy.extractRegion = enemy.source;
            enemy.textureData = new Color[enemy.source.Width * enemy.source.Height];
            enemy.TextureImage.GetData<Color>(0, enemy.extractRegion, enemy.textureData, 0, enemy.source.Width * enemy.source.Height);

            Rectangle rect1 = source;
            Color[] data1 = textureData; 
            Rectangle rect2 = enemy.source;
            Color[] data2 = enemy.textureData;

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
                /*if (IntersectsPixel(source, textureData, shots[i].CollisionRectangle, shots[i].textureData))
                {
                    return i;
                }*/

                if(shots[i].CollisionRectangle.Intersects(CollisionRectangle))
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
            if (velocity.X <= MoveSpeed)
            {
                velocity.X += InitialVelocity.X;
            }
        }

        public virtual void Left()
        {
            isMoving = true;
            if (velocity.X >= -MoveSpeed)
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
                AtkSpeed = .15f;
            }

            if (weaponType == "rail")
            {
                primaryType = weaponType;
                primaryAmmo = ammoCapacity;
                currentPrimaryAmmo = ammoCapacity;
                AtkSpeed = .7f;
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
            if (currentPrimaryAmmo != 0 && timer >= AtkSpeed)
            {
                if (primaryType == "laser")
                {
                    shootLaser(content, gameTime);
                }
                if (primaryType == "rail")
                {
                    shootRail(content);
                }
                timer = 0;
            }
        }

        // Chooses which secondary weapon to shoot
        public virtual void shootSecondary(ContentManager content)
        {
            if (currentSecondaryAmmo != 0 && timer >= AtkSpeed)
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
                timer = 0;
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
                Weapon left = new HelixMissile(content, new Vector2(position.X, position.Y + 50f - spriteOrigin.Y), 2f, -1);
                secondary.Add(left);
                Weapon right = new HelixMissile(content, new Vector2(position.X, position.Y + 50f - spriteOrigin.Y), 2f, 1);
                secondary.Add(right);
            }
        }

        public void shootHomingMissile(ContentManager content)
        {
            HomingMissile missile = new HomingMissile(content, new Vector2(position.X, position.Y - spriteOrigin.Y), 500f);
            secondary.Add(missile);
        }

        public void shootLaser(ContentManager content, GameTime gameTime)
        {
                BasicLaser laser = new BasicLaser(content, railLeft.position, 2000f);
                railLeft.primary.Add(laser);
                laser = new BasicLaser(content, railRight.position, 2000f);
                railRight.primary.Add(laser);
        }

        public void shootRail(ContentManager content)
        {
            Rail rail = new Rail(content, railLeft.position, 250f, -1);
            railLeft.primary.Add(rail);
            rail = new Rail(content, railRight.position, 250f, 1);
            railRight.primary.Add(rail);
        }
        #endregion

        #region Reinforcement Methods
        #endregion
    }
}
