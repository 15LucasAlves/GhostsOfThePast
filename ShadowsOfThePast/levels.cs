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

        // Player and Camera variables
        Player player;
        private Vector2 camera;

        public Dictionary<Vector2, int> background;
        public Dictionary<Vector2, int> platforms;
        public Dictionary<Vector2, int> props;
        public Dictionary<Vector2, int> house;
        public Dictionary<Vector2, int> collisions;
        public Texture2D textureDic;
        public Texture2D collidortext;
        public Texture2D playertext;
        private SpriteFont font;
        private int counter = 0;
        int score = 0;
        int display_tilesize = 64;

        // Gravity and colision variables
        const float gravity = 9.8f;
        private int TILESIZE = 64;
        private List<Rectangle> intersections;


        public levels(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _content = content;
            player = new Player(_game, _graphicsDevice, _spriteBatch, _content);

            background = LoadMap("../../Data/level1_background.csv");
            platforms = LoadMap("../../Data/level1_platforms.csv");
            //props = LoadMap("../../Data/level1_props.csv");
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
            player.LoadContent(_content, _spriteBatch);

            //load the tileset png used to make the tileset on tiled
            textureDic = _content.Load<Texture2D>("map");
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
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camera.Y += 5;
            }

            player.Update(gameTime, graphicsDevice);
            player.playerRectangle.X += (int)player.velocity.X;
            intersections = getIntersectingTilesHorizontal(player.playerRectangle);

            foreach (var rect in intersections)
            {

                // handle collisions if the tile position exists in the tile map layer.
                if (collisions.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {

                    // create temp rect to handle collisions (not necessary, you can optimize!)
                    Rectangle collision = new(
                        rect.X * display_tilesize,
                        rect.Y * display_tilesize,
                        display_tilesize,
                        display_tilesize
                    );

                    // handle collisions based on the direction the player is moving
                    if (player.velocity.X > 0.0f)
                    {
                        player.playerRectangle.X = collision.Left - player.playerRectangle.Width / 1;
                    }
                    else if (player.velocity.X < 0.0f)
                    {
                        player.playerRectangle.X = collision.Right + player.playerRectangle.Width / 1;
                    }

                }

            }

            // same as horizontal collisions

            player.playerRectangle.Y += (int)player.velocity.Y;
            intersections = getIntersectingTilesVertical(player.playerRectangle);

            foreach (var rect in intersections)
            {

                if (collisions.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {

                    Rectangle collision = new Rectangle(
                        rect.X * TILESIZE,
                        rect.Y * TILESIZE,
                        TILESIZE,
                        TILESIZE
                    );

                    if (player.velocity.Y > 0.0f)
                    {
                        player.playerRectangle.Y = collision.Top - player.playerRectangle.Height / 1;
                    }
                    else if (player.velocity.Y < 0.0f)
                    {
                        player.playerRectangle.Y = collision.Bottom + player.playerRectangle.Height / 1;
                    }

                }
            }

        }

        public List<Rectangle> getIntersectingTilesHorizontal(Rectangle playerRectangle)
        {

            List<Rectangle> intersections = new();

            int widthInTiles = (playerRectangle.Width - (playerRectangle.Width % TILESIZE)) / TILESIZE;
            int heightInTiles = (playerRectangle.Height - (playerRectangle.Height % TILESIZE)) / TILESIZE;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {

                    intersections.Add(new Rectangle(

                        (playerRectangle.X + x * TILESIZE) / display_tilesize,
                        (playerRectangle.Y + y * (TILESIZE - 1)) / display_tilesize,
                        display_tilesize,
                        display_tilesize

                    ));

                }
            }

            return intersections;
        }

        public List<Rectangle> getIntersectingTilesVertical(Rectangle playerRectangle)
        {

            List<Rectangle> intersections = new();

            int widthInTiles = (playerRectangle.Width - (playerRectangle.Width % TILESIZE)) / TILESIZE;
            int heightInTiles = (playerRectangle.Height - (playerRectangle.Height % TILESIZE)) / TILESIZE;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {

                    intersections.Add(new Rectangle(

                        (playerRectangle.X + x * (TILESIZE - 1)) / display_tilesize,
                        (playerRectangle.Y + y * TILESIZE) / display_tilesize,
                        display_tilesize,
                        display_tilesize

                    ));

                }
            }

            return intersections;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _spriteBatch.Begin();

            int num_tiles_per_row_png = 7; //number of tiles in a row in the texturedic png
            int tilesize = 64;

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

