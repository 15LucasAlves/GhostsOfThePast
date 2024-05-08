using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // Player sprite and position
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        // Animation variables
        private int currentFrame;
        private int totalFrames;

        // Slow down frame animation
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 400;

        // Player constructor
        public Player(Texture2D texture, int rows, int columns)
        {
            healthPoints = 3;
            manaPoints = 10;
            isAlive = true;

            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }


        // To update the player's
        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;

                KeyboardState keystate = Keyboard.GetState();

                // Idle animation
                if (keystate.GetPressedKeys().Length == 0)
                    currentFrame++;
                timeSinceLastFrame = 0;
                if (currentFrame == 2)
                    currentFrame = 0;

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
        }

        // To draw the player
        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color)
        {
            int width = 64;
            int height = 64;
            int row = (int)((float)currentFrame / Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color);
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