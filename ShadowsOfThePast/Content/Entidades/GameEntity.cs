using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowsOfThePast.Content.Entidades
{
    // Interface for the game entities that will be drawn and updated
    public interface GameEntity
    {
        // Properties for the draw and update order
        int drawOrder { get; set; }
        int updateOrder { get; set; }

        // Draw and Update methods
        void Draw(SpriteBatch sprite, GameTime gameTime);

        void Update(GameTime gameTime);

    }
}
