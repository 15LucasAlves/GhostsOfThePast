using Microsoft.Xna.Framework;
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
        public Player(Texture2D texture, int rows, int columns)
        {
            healthPoints = 3;
            manaPoints = 10;
            isAlive = true;
        }


        // Load the player's animations
        public void Load() 
        {
            // Loads the players animations
            idle = new Texture2D[2];
            idle[0] = content.Load<Texture2D>("Idle0");
            idle[1] = Content.Load<Texture2D>("Idle1");

            walkR = new Texture2D[4];
            walkR[0] = Content.Load<Texture2D>("WalksideR0");
            walkR[1] = Content.Load<Texture2D>("WalksideR1");
            walkR[2] = Content.Load<Texture2D>("WalksideR2");
            walkR[3] = Content.Load<Texture2D>("WalksideR3");

            jumpR = new Texture2D[4];
            jumpR[0] = Content.Load<Texture2D>("jumpR0");
            jumpR[1] = Content.Load<Texture2D>("jumpR1");
            jumpR[2] = Content.Load<Texture2D>("jumpR2");
            jumpR[3] = Content.Load<Texture2D>("jumpR3");
        }


        // To update the player's
        public void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();
            animationCounter++;

            // Idle animation
            if (keystate.GetPressedKeys().Length == 0)
            {
                animationSprite = idle[activeFrame];

                activeFrame++;

                if (activeFrame >= 2)
                {
                    activeFrame = 0;
                }
            }

            // Walking Left Animation
            if (keystate.IsKeyDown(Keys.D))
            {

            }

            // Walking Right Animation
            if (keystate.IsKeyDown(Keys.A))
            {

            }

            // Jumping Animation
            if (keystate.IsKeyDown(Keys.W))
            {

            }
        }


        // To draw the player
        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color)
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