using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ShadowsOfThePast.Interface;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ShadowsOfThePast
{
    public class deathscreen : IGameState
    {
        private Texture2D endscreen;
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;
        private Vector2 location;
        public SoundEffect song;
        public bool playSound;


        public deathscreen(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;
        }
        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;
            endscreen = _content.Load<Texture2D>("youdied_");

            song = _content.Load<SoundEffect>("audio/horror-lose-2028");
            SoundEffectInstance soundEffectInstance = song.CreateInstance();

            location.X = (_graphicsDevice.Viewport.Width - endscreen.Width) / 2;
            location.Y = (_graphicsDevice.Viewport.Height - endscreen.Height) / 2;

        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphicsDeviceManager)
        {
            song.Play();

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(endscreen, location, Color.White);

            spriteBatch.End();
        }
    }
}
