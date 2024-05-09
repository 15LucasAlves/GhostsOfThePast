using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShadowsOfThePast.Interface
{
	public interface IGameState
	{
        void Initialize();
        void LoadContent(ContentManager content, SpriteBatch spriteBatch);
        void Update(GameTime gameTime, GraphicsDevice graphicsDevice);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}

