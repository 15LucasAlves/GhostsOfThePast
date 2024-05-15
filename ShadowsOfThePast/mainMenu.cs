using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ShadowsOfThePast.Interface;
using Microsoft.Xna.Framework.Media;
using static System.Net.Mime.MediaTypeNames;

namespace ShadowsOfThePast
{
    public class mainMenu : IGameState
    {
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;

        Texture2D intro_animation;
        public Texture2D[] intro;
        Texture2D button_texture;
        public SpriteFont font;

        public int activeFrame;
        private Vector2 location;
        private Vector2 location_button;

        public Song song;


        public mainMenu(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
		{
            _graphicsDevice = graphicsDevice;
        }

        public void Initialize()
        {

        }
        public void LoadContent(ContentManager Content, SpriteBatch spriteBatch)
        {
            _content = Content;
            intro = new Texture2D[9];

            intro[0] = _content.Load<Texture2D>("intro/intro1");
            button_texture = _content.Load<Texture2D>("intro/buttontexture");
            font = _content.Load<SpriteFont>("File");
            song = _content.Load<Song>("audio/into_the_woods");

            MediaPlayer.Play(song);

            intro[1] = _content.Load<Texture2D>("intro/intro2");
            intro[2] = _content.Load<Texture2D>("intro/intro3");
            intro[3] = _content.Load<Texture2D>("intro/intro4");
            intro[4] = _content.Load<Texture2D>("intro/intro5");
            intro[5] = _content.Load<Texture2D>("intro/intro6");
            intro[6] = _content.Load<Texture2D>("intro/intro7");
            intro[7] = _content.Load<Texture2D>("intro/intro8");
            intro[8] = _content.Load<Texture2D>("intro/intro9");
            
            intro_animation = intro[0];
            location.X = (_graphicsDevice.Viewport.Width - intro[0].Width) / 2;
            location.Y = (_graphicsDevice.Viewport.Height - intro[0].Height) / 2;
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            /*
             if (activeFrame >= 8)
             {
                 activeFrame = 0;
             }
            
            activeFrame++;
            */
            activeFrame = 0;
            intro_animation = intro[activeFrame];

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            location.X = (_graphicsDevice.Viewport.Width - intro[0].Width) / 2;
            location.Y = (_graphicsDevice.Viewport.Height - intro[0].Height) / 2 - 55;

            spriteBatch.Draw(intro_animation, location, Color.White);

            location_button.X = (_graphicsDevice.Viewport.Width - button_texture.Width) / 2;
            location_button.Y = 300;

            spriteBatch.Draw(button_texture, location_button, Color.White);

            spriteBatch.End();
        }
    }
}

