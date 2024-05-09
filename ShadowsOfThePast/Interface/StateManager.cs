using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShadowsOfThePast.Interface
{
	public class StateManager
	{
        private IGameState _currentState;

        public IGameState CurrentState
        {
            get { return _currentState; }
        }

        public void ChangeState(IGameState newState)
        {
            _currentState = newState;
        }

        public void Initialize()
        {
            _currentState.Initialize();
        }

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _currentState.LoadContent(content, spriteBatch);
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            _currentState.Update(gameTime, graphicsDevice);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _currentState.Draw(gameTime, spriteBatch);
        }
    }
}

