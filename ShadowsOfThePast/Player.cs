using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ShadowsOfThePast
{
    public class Player
    {
        // Depndencies from monogame
        private Game1 _game;
        private GraphicsDeviceManager _graphics;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private levels _levels;

        public Rectangle playerRectangle;
        public Vector2 location;
        public Vector2 velocity;

        // Jumping variables
        int maxJumps = 1; //maximum number of jumps before touching the ground
        int jumpCount = 0; //current number of jumps
        bool playerIsJumping; //if the player is jumping
        int jumpHeight; //how high the player can jump
        Vector2 playerPosBJumping; //player position before jumping

        // HP and MP meter so we know how much hits can the player take and how much spells can he cast
        public int healthPoints;
        public int manaPoints;
        // We need to know the player's score to open the portal eventually
        public int score;
        // We need to know if the player is alive or not so we can end the game
        public bool isAlive;

        // Animation variables
        public string sAnimation;
        public int animationCounter;
        public int activeFrame;
        Texture2D animationSprite;
        public Texture2D[] idle;
        public Texture2D[] walkR;
        public Texture2D[] walkL;
        public Texture2D[] jumpR;
        public Texture2D[] jumpL;


        // Player constructor
        public Player(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content, levels levels)
        {
            healthPoints = 1;
            manaPoints = 3;
            isAlive = true;
            location.X = 100;
            location.Y = 235;
            playerRectangle = new Rectangle((int)location.X, (int)location.Y, 64, 64);
            velocity = new();
            _levels = levels;
        }


        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;

            // Load the player's sprites
            idle = new Texture2D[2];
            walkR = new Texture2D[4];
            walkL = new Texture2D[4];
            jumpR = new Texture2D[4];
            jumpL = new Texture2D[4];

            idle[0] = _content.Load<Texture2D>("Idle0");
            idle[1] = _content.Load<Texture2D>("Idle1");

            if (idle[0] == null)
            {
                System.Diagnostics.Debug.WriteLine("Failed to load textureDic.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Successfully loaded textureDic.");
            }

            walkR[0] = _content.Load<Texture2D>("WalksideR0");
            walkR[1] = _content.Load<Texture2D>("WalksideR1");
            walkR[2] = _content.Load<Texture2D>("WalksideR2");
            walkR[3] = _content.Load<Texture2D>("WalksideR3");

            walkL[0] = _content.Load<Texture2D>("WalksideL0");
            walkL[1] = _content.Load<Texture2D>("WalksideL1");
            walkL[2] = _content.Load<Texture2D>("WalksideL2");
            walkL[3] = _content.Load<Texture2D>("WalksideL3");

            jumpR[0] = _content.Load<Texture2D>("jumpR0");
            jumpR[1] = _content.Load<Texture2D>("jumpR1");
            jumpR[2] = _content.Load<Texture2D>("jumpR2");
            jumpR[3] = _content.Load<Texture2D>("jumpR3");

            jumpL[0] = _content.Load<Texture2D>("jumpL0");
            jumpL[1] = _content.Load<Texture2D>("jumpL1");
            jumpL[2] = _content.Load<Texture2D>("jumpL2");
            jumpL[3] = _content.Load<Texture2D>("jumpL3");

            animationSprite = idle[0];
        }


        // To update the player's
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            KeyboardState KeyState = Keyboard.GetState();

            // Reset the player's velocity
            velocity.X = 0.0f;
            // constant gravity to pull the player down
            if (playerIsJumping == false)
            {
                velocity.Y = 6.0f;
            }
            // Reset the jump count
            if (_levels.ischaronGround == true)
            {
                jumpCount = 0;
            }
            // Get the player's position before jumping and permit the player to move
            if (playerIsJumping == false)
            {
                playerPosBJumping.X = playerRectangle.Center.X;
                playerPosBJumping.Y = playerRectangle.Center.Y;

                // Player movement
                if (KeyState.GetPressedKeys().Length == 0)
                {
                    sAnimation = "i";
                }

                if (KeyState.IsKeyDown(Keys.D))
                {
                    velocity.X = 2;
                    sAnimation = "wR";
                }

                if (KeyState.IsKeyDown(Keys.A))
                {
                    velocity.X = -2;
                    sAnimation = "wL";
                }
            }

            if (playerIsJumping == true)
            {
                // Player movement while jumping
                if (KeyState.IsKeyDown(Keys.D))
                {
                    velocity.X = 4;
                }

                if (KeyState.IsKeyDown(Keys.A))
                {
                    velocity.X = -4;
                }
            }

            if (KeyState.IsKeyDown(Keys.W) && jumpCount < maxJumps)
            {
                playerIsJumping = true;
                velocity.Y = -6.0f;
                jumpCount++;

                if (velocity.X > 0)
                {
                    sAnimation = "jR";
                }
                else
                {
                    sAnimation = "jL";
                }
            }

            if(KeyState.IsKeyDown(Keys.Space))
            {
                // Create attack method
            }

            // Player animation
            // Limit the animation speed
            if (animationCounter == 12)
            {
                if (sAnimation == "wR" && !playerIsJumping)
                {
                    if (activeFrame > 3)
                    {
                        activeFrame = 0;
                    }
                    animationSprite = walkR[activeFrame];
                    activeFrame++;
                }
                else if (sAnimation == "wL" && !playerIsJumping)
                {
                    if (activeFrame > 3)
                    {
                        activeFrame = 0;
                    }
                    animationSprite = walkL[activeFrame];
                    activeFrame++;
                }
                else if (sAnimation == "jR")
                {
                    if (activeFrame > 3)
                    {
                        activeFrame = 0;
                    }
                    animationSprite = jumpR[activeFrame];
                    activeFrame++;
                }
                else if (sAnimation == "jL")
                {
                    if (activeFrame > 3)
                    {
                        activeFrame = 0;
                    }
                    animationSprite = jumpL[activeFrame];
                    activeFrame++;
                }
                else if (sAnimation == "i" && !playerIsJumping)
                {
                    if(activeFrame > 1)
                    {
                        activeFrame = 0;
                    }
                    animationSprite = idle[activeFrame];
                    activeFrame++;
                }
                animationCounter = 0;
            }
            animationCounter++;

            // See if the player completed its jump
            if (playerIsJumping)
            {
                if (playerRectangle.Center.Y <= playerPosBJumping.Y - (playerRectangle.Height * 2) || // adicionar para cair caso esteja a colidir no Y)
                {
                    playerIsJumping = false;
                }
            }
        }


        // To draw the player
        public void Draw(SpriteBatch spriteBatch, Color color, GameTime gameTime)
        {
            Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.Red });
            spriteBatch.Draw(animationSprite, playerRectangle, color);

        }


        // Player fire spell method
        public void Attack()
        {
            // Check if the player should fire spell
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                // Fire the spell


            }
        }


        // Player take damage method
        public void TakeDamage(Player player)
        {
            // Deal damage to the player
            player.healthPoints -= 1;

            // Check if the player should die
            if (player.healthPoints <= 0)
            {
                // Player is dead
                player.isAlive = false;
            }
        }

    }
}