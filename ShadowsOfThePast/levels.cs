using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using ShadowsOfThePast.Interface;
using Microsoft.Xna.Framework.Content;
using System.Reflection.Metadata;


namespace ShadowsOfThePast
{
	public class levels : IGameState
	{
        private Game1 _game;
        private GraphicsDeviceManager _graphics;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        Player player;

        public Dictionary<Vector2, int> background;
        public Dictionary<Vector2, int> platforms;
        public Dictionary<Vector2, int> props;
        public Dictionary<Vector2, int> house;
        public Dictionary<Vector2, int> collisions;
        public Texture2D textureDic;
        public Texture2D collidortext;
        public Texture2D playertext;
        private Vector2 camera;
        private SpriteFont font;
        const float gravity = 9.8f;
        private int counter = 0;
        int score = 0;
        int display_tilesize = 32;

        public levels(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
		{
            _game = game;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _content = content;
            player = new Player(_game, _graphicsDevice, _spriteBatch, _content);

            background = LoadMap("../../Data/level1_background.csv");
            platforms = LoadMap("../../Data/level1_platforms.csv");
            props = LoadMap("../../Data/level1_props.csv");
            house = LoadMap("../../Data/level1_house.csv");
            collisions = LoadMap("../../Data/level1_collisions.csv");
            camera = Vector2.Zero;
        }

        private Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new();
            StreamReader reader = new(filepath);

            int y = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(',');
                for (int x = 0; x < items.Length; x++)
                {
                    if (int.TryParse(items[x], out int value))
                    {
                        if (value > -1)
                        {
                            result[new Vector2(x, y)] = value;
                        }
                    }
                }
                y++;
            }
            return result;
        }

        public bool Victory() //level switcher based on score
        {
            if (score >= 500)
            {
                counter++;
                score = 0;
            }
            return true;
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _content = content;
            player.LoadContent(_content,_spriteBatch);

            //load the tileset png used to make the tileset on tiled
            textureDic = _content.Load<Texture2D>("mmm");
            if (textureDic == null)
            {
                System.Diagnostics.Debug.WriteLine("Failed to load textureDic.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Successfully loaded textureDic.");
            }

            collidortext = _content.Load<Texture2D>("collision");
            font = _content.Load<SpriteFont>("File");
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            //simple camera to look over the level, later we should make it track the player instead
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camera.X = Math.Min(camera.X - 5, _graphicsDevice.Viewport.Width);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camera.X = Math.Min(camera.X + 5, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camera.Y -= 5;
            }
            

            /*foreach(var item in collisions)
            {
                if(player.location.X(int)item.Key.X)
                {
                    player.location += new Vector2(0, gravity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
            /*
            int tileWidth = 32;
            int tileHeight = 32;

            //gets player position
            int tileX = (int)(player.Position.X / tileWidth);
            int tileY = (int)(player.Position.Y / tileHeight);

            Vector2 playerTilePos = new Vector2(tileX, tileY);


            if (collisions.ContainsKey(playerTilePos))
            {
                // If there's a tile, the player can walk here
                // Update the player's position as normal
            }
            else
            {
                player.Velocity.Y += gravity * gameTime.ElapsedGameTime.TotalSeconds;
            }
            */

            player.Update(gameTime, graphicsDevice);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _spriteBatch.Begin();

            int num_tiles_per_row_png = 14; //number of tiles in a row in the texturedic png
            int tilesize = 32;

            if (counter == 0)
            {
                foreach (var item in background)
                {
                    Rectangle drect = new(
                        (int)item.Key.X * display_tilesize + (int)camera.X, (int)item.Key.Y * display_tilesize + (int)camera.Y, display_tilesize, display_tilesize
                     );

                    int x = item.Value % num_tiles_per_row_png;
                    int y = item.Value / num_tiles_per_row_png;

                    Rectangle src = new(
                        x * tilesize, y * tilesize, tilesize, tilesize
                        );
                    _spriteBatch.Draw(textureDic, drect, src, Color.White);
                }

                foreach (var item in platforms)
                {
                    Rectangle drect = new(
                        (int)item.Key.X * display_tilesize + (int)camera.X, (int)item.Key.Y * display_tilesize + (int)camera.Y, display_tilesize, display_tilesize
                     );

                    int x = item.Value % num_tiles_per_row_png;
                    int y = item.Value / num_tiles_per_row_png;

                    Rectangle src = new(
                        x * tilesize, y * tilesize, tilesize, tilesize
                        );
                    _spriteBatch.Draw(textureDic, drect, src, Color.White);
                }

                foreach (var item in props)
                {
                    Rectangle drect = new(
                        (int)item.Key.X * display_tilesize + (int)camera.X, (int)item.Key.Y * display_tilesize + (int)camera.Y, display_tilesize, display_tilesize
                     );

                    int x = item.Value % num_tiles_per_row_png;
                    int y = item.Value / num_tiles_per_row_png;

                    Rectangle src = new(
                        x * tilesize, y * tilesize, tilesize, tilesize
                        );
                    _spriteBatch.Draw(textureDic, drect, src, Color.White);
                }

                foreach (var item in house)
                {
                    Rectangle drect = new(
                        (int)item.Key.X * display_tilesize + (int)camera.X, (int)item.Key.Y * display_tilesize + (int)camera.Y, display_tilesize, display_tilesize
                     );
                    //value is the tile index in the tileset
                    int x = item.Value % num_tiles_per_row_png;
                    int y = item.Value / num_tiles_per_row_png;

                    Rectangle src = new(
                        x * tilesize, y * tilesize, tilesize, tilesize
                        );
                    _spriteBatch.Draw(textureDic, drect, src, Color.White);
                }
            }
            else
            {
                //add another level
            }

            player.Draw(spriteBatch, Color.White, gameTime);
            _spriteBatch.DrawString(font, "Score:", new Vector2(5, 0), Color.Black);

            _spriteBatch.End();
        }
    }
}

