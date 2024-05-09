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


        // HP and MP meter so we know how much hits can the player take and how much spells can he cast
        public int healthPoints;
        public int manaPoints;
        // We need to know the player's score to open the portal eventually
        public int score;
        // We need to know if the player is alive or not so we can end the game
        public bool isAlive;

        // Animation variables
        public int animationCounter;
        public int activeFrame;
        Texture2D animationSprite;
        public Texture2D[] idle;
        public Texture2D[] walkR;
        public Texture2D[] jumpR;


        // Player constructor
        public Player(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
        {
            healthPoints = 3;
            manaPoints = 10;
            isAlive = true;
        }


        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;

            // Load the player's sprites
            idle = new Texture2D[2];
            walkR = new Texture2D[4];
            jumpR = new Texture2D[4];

            idle[0] = _content.Load<Texture2D>("Idle0");
            idle[1] = _content.Load<Texture2D>("Idle1");

            walkR[0] = _content.Load<Texture2D>("WalksideR0");
            walkR[1] = _content.Load<Texture2D>("WalksideR1");
            walkR[2] = _content.Load<Texture2D>("WalksideR2");
            walkR[3] = _content.Load<Texture2D>("WalksideR3");

            jumpR[0] = _content.Load<Texture2D>("jumpR0");
            jumpR[1] = _content.Load<Texture2D>("jumpR1");
            jumpR[2] = _content.Load<Texture2D>("jumpR2");
            jumpR[3] = _content.Load<Texture2D>("jumpR3");
        }   


        // To update the player's
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            KeyboardState keystate = Keyboard.GetState();
            animationCounter++;

            // Limit the animation speed
            if (animationCounter == 30)
            {
                // Idle animation
                if (keystate.GetPressedKeys().Length == 0)
                {
                    // Reset the animation (only has 2 frames so reset every 2 frames)
                    if (activeFrame >= 2)
                    {
                        activeFrame = 0;
                    }

                    animationSprite = idle[activeFrame];

                    activeFrame++;
                }

                // Walking Left Animation
                if (keystate.IsKeyDown(Keys.D))
                {
                    // Reset the animation (only has 4 frames so reset every 4 frames)
                    if (activeFrame >= 4)
                    {
                        activeFrame = 0;
                    }

                    animationSprite = walkR[activeFrame];

                    activeFrame++;
                }
            

                // Walking Right Animation
                if (keystate.IsKeyDown(Keys.A))
                {

                }

                // Jumping Right Animation
                if (keystate.IsKeyDown(Keys.W) || keystate.IsKeyDown(Keys.D))
                {
                    // Reset the animation (only has 4 frames so reset every 4 frames)
                    if (activeFrame >= 4)
                    {
                        activeFrame = 0;
                    }

                    animationSprite = jumpR[activeFrame];
                    
                    activeFrame++;
                }

                // Jumping Left Animation
                if (keystate.IsKeyDown(Keys.W) || keystate.IsKeyDown(Keys.A))
                {

                }

                animationCounter = 0;
            }
        }
    


        // To draw the player
        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color, GameTime gameTime)
        {
            int width = 64;
            int height = 64;           

            Rectangle sourceRectangle = new Rectangle(width, height, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(animationSprite, destinationRectangle, sourceRectangle, color);
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


        // Player swap spell method
        public void SwapSpell()
        {
            // Check if the player should swap spell
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                // Swap the spell


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