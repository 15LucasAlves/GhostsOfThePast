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
    public class Camera
    {
        public Vector2 position;
        public Matrix _translation;

        private GraphicsDevice _graphicsDevice;
        private Game1 _game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        public Vector2 positions;

        int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;


        public Camera(Vector2 position)
        {
            this.position = position;
        }

        public void followPlayer(Rectangle targetPlayer, Vector2 screenSize)
        {
            position = new Vector2(-targetPlayer.X + (screenSize.X / 2 - targetPlayer.Width / 2), -targetPlayer.Y + (screenSize.Y / 2 - targetPlayer.Height / 2));
        }

        public void calcTranslation(int plx, int ply)
        {
            var cx = (width / 2) - plx;
            var cy = (height / 2) - ply;
            _translation = Matrix.CreateTranslation(cx, cy, 0f);
        }
    }
}