using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowsOfThePast
{
    internal class Enemy
    {
        // Enemy basic variables
        public int healthPoints;
        public bool gotDamaged;
        public bool isAlive;

        // Enemy animation variables
        public int animationCounter;
        public int activeFrame;
        Texture2D animationSprite;
        public Texture2D[] movingR;
        public Texture2D[] movingL;
        bool isFacingLeft;

        // Enemy drawing / colisions variables 
        public Rectangle enemyRectangle;
        public int colliding;

        public Enemy(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content, levels levels)
        {
            // Initialize the enemy's variables
            healthPoints = 2;
            isAlive = true;
            gotDamaged = false;
            colliding = 0;
            enemyRectangle = new Rectangle(150, 200, 64, 64);
        }

        public void loadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            // Load the enemmy's animation sprites
            movingR = new Texture2D[4];
            movingL = new Texture2D[4];

            movingR[0] = content.Load<Texture2D>("emR0");
            movingR[1] = content.Load<Texture2D>("emR1");
            movingR[2] = content.Load<Texture2D>("emR2");
            movingR[3] = content.Load<Texture2D>("emR3");

            movingL[0] = content.Load<Texture2D>("emL0");
            movingL[1] = content.Load<Texture2D>("emL1");
            movingL[2] = content.Load<Texture2D>("emL2");
            movingL[3] = content.Load<Texture2D>("emL3");

            animationSprite = movingL[0];
        }


        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            if (colliding % 2 == 0)
            {
                enemyRectangle.X -= 1;
                isFacingLeft = true;
            }
            else if (colliding % 2 != 0)
            {
                enemyRectangle.X += 1;
            }

            // Limit the animation speed
            if (animationCounter == 9)
            {
                if (isFacingLeft)
                {
                    //reset the animation (only has 4 frames so reset every 4 frames)
                    if (activeFrame >= 4)
                    {
                        activeFrame = 0;
                    }

                    animationSprite = movingL[activeFrame];
                }
                else
                {
                    if (activeFrame >= 4)
                    {
                        activeFrame = 0;
                    }

                    animationSprite = movingR[activeFrame];
                }

                activeFrame++;
                animationCounter = 0;
            }
            animationCounter++;

            if (healthPoints <= 0)
            {
                isAlive = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                spriteBatch.Draw(animationSprite, enemyRectangle, Color.White);    
            }
        }   
    }
}
