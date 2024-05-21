using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowsOfThePast
{
    public class Magic
    {
        // Magic basic variables
        public int direction;
        public Rectangle magicRectangle;
        public int pXInit;
        public int pXMaxDistance = 300;
        public bool faded;

        // Magic animation variables
        public int animationCounter;
        public int activeFrame;
        Texture2D animationSprite;
        public Texture2D[] magic;

        public Magic(int x, int y, int dir)
        {
            // Initialize the magic's variables
            pXInit = x;
            faded = false;
            direction = dir;
            magicRectangle = new Rectangle(x, y, 13, 13);
        }

        public void loadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            // Load the magic's animation sprites
            animationSprite = content.Load<Texture2D>("magic");
        }

        public void update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            if (direction == 1)
            {
                magicRectangle.X += 3;
            }
            else
            {
                magicRectangle.X -= 3;
            }

            if (magicRectangle.X > pXInit + pXMaxDistance || magicRectangle.X < pXInit - pXMaxDistance)
            {
                faded = true;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            // Draw the magic's animation
            spriteBatch.Draw(animationSprite, magicRectangle, Color.White);
        }
    }
}
