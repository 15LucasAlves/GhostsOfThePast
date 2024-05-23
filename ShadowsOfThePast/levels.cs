using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using ShadowsOfThePast.Interface;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System.Reflection.Metadata;
using System.Diagnostics.SymbolStore;
using System.Linq;


namespace ShadowsOfThePast
{
    public class levels : IGameState
    {
        private Game1 _game;
        private GraphicsDeviceManager _graphics;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private StateManager _stateManager;
        private deathscreen _deathscreen;
        public bool endgame;

        Player player;
        Enemy enemy0;
        Enemy enemy1;
        Enemy enemy2;
        Enemy enemy3;
        Enemy enemy4;
        Enemy enemy5;
        Enemy enemy6;
        Enemy enemy7;
        private List<Enemy> enemies;
        mainMenu MainMenu;

        private OrthographicCamera _camera;
        private Vector2 _cameraTarget;

        public Dictionary<Vector2, int> background;
        public Dictionary<Vector2, int> platforms;
        public Dictionary<Vector2, int> props;
        public Dictionary<Vector2, int> house;
        public Dictionary<Vector2, int> collisions;
        public Dictionary<Vector2, int> sidecollisions;
        public Dictionary<Vector2, int> boxes;
        public Dictionary<Vector2, int> teleport;
        public Dictionary<Vector2, int> finalportal;


        public Texture2D textureDic;
        public Texture2D collidortext;
        public Texture2D playertext;
        public Texture2D box;
        public Texture2D map;
        private SpriteFont font;
        public Song song;

        private int counter = 0;
        int score = 0;
        int display_tilesize = 64;
        public bool ischaronGround;

        //colision variables
        public int TILESIZE = 64;
        private List<Rectangle> horizontal_intersections;
        private List<Rectangle> vertical_intersections;
        private List<Rectangle> horizontal_intersectionsEnemies;


        public levels(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _content = content;
            player = new Player(_game, _graphicsDevice, _spriteBatch, _content, this);
            enemies = new List<Enemy>();
            enemy0 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 1006, 870);
            enemy1 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 1916, 870);
            enemy2 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 1280, 358);
            enemy3 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 1984, 230);
            enemy4 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 2556, 230);
            enemy5 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 3028, 166);
            enemy6 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 3766, 358);
            enemy7 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 3880, 102);
            MainMenu = new mainMenu(_game, _graphicsDevice, _spriteBatch, _content);
            _deathscreen = new deathscreen(_game, _graphicsDevice, _spriteBatch, _content);
            _stateManager = new StateManager();

            var viewportAdapter = new BoxingViewportAdapter(game.Window, graphicsDevice, 800, 480);
            _camera = new OrthographicCamera(viewportAdapter);

            //level1
            background = LoadMap("../../Data/level1_background.csv");
            platforms = LoadMap("../../Data/level1_platforms.csv");
            house = LoadMap("../../Data/level1_house.csv");
            collisions = LoadMap("../../Data/level1_collisions.csv");
            sidecollisions = LoadMap("../../Data/sidebound.csv");
            boxes = LoadMap("../../Data/boxes.csv");
            teleport = LoadMap("../../Data/teleport_portal.csv");
            finalportal = LoadMap("../../Data/final_portal_final portal.csv");
            endgame = false;
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
            endgame = false;
            //reset variables
            score = 0;

            player = new Player(_game, _graphicsDevice, _spriteBatch, _content, this);
            enemies = new List<Enemy>();
            enemy0 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 1006, 870);
            enemy1 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 1916, 870);
            enemy2 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 1280, 358);
            enemy3 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 1984, 230);
            enemy4 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 2556, 230);
            enemy5 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 3028, 166);
            enemy6 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 3766, 358);
            enemy7 = new Enemy(_game, _graphicsDevice, _spriteBatch, _content, this, 3880, 102);
            player.healthPoints = 1;

            boxes = LoadMap("../../Data/boxes.csv");
            _spriteBatch.Begin();
            foreach (var item in boxes)
            {
                Rectangle drect = new(
                    (int)item.Key.X * display_tilesize, (int)item.Key.Y * display_tilesize, display_tilesize, display_tilesize
                 );
                //value is the tile index in the tileset
                int x = item.Value % 7;
                int y = item.Value / 7;

                Rectangle src = new(
                    x * 64, y * 64, 64, 64
                    );
                _spriteBatch.Draw(box, drect, src, Color.White);
            }
            _spriteBatch.End();
        }

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _content = content;

            enemies.Add(enemy0);
            enemies.Add(enemy1);
            enemies.Add(enemy2);
            enemies.Add(enemy3);
            enemies.Add(enemy4);
            enemies.Add(enemy5);
            enemies.Add(enemy6);
            enemies.Add(enemy7);

            player.LoadContent(_content, _spriteBatch);

            foreach (var enemy in enemies)
            {
                enemy.loadContent(_content, _spriteBatch);
            }

            //load the tileset png used to make the tileset on tiled
            textureDic = _content.Load<Texture2D>("map1");

            map = _content.Load<Texture2D>("map2");

            collidortext = _content.Load<Texture2D>("collision");

            box = _content.Load<Texture2D>("box");

            font = _content.Load<SpriteFont>("File");

            song = _content.Load<Song>("audio/lost_and_notfound");

            MediaPlayer.Play(song);
        }


        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, GraphicsDeviceManager _graphics)
        {
            player.Update(gameTime, graphicsDevice);

            if (player.isAlive == false)
            {
                endgame = true;
            }

            foreach (var enemy in enemies.ToList())
            {
                enemy.Update(gameTime, graphicsDevice);

                if(enemy.isAlive == false)
                {
                    enemies.Remove(enemy);
                }
            }

            player.playerRectangle.X += (int)player.velocity.X;
            //System.Diagnostics.Debug.WriteLine(player.playerRectangle.X);
            horizontal_intersections = getIntersectingTilesHorizontal(player.playerRectangle);

            foreach (var rect in horizontal_intersections)
            {
                //removes box every time the player collides with it and increases score
                if (boxes.TryGetValue(new Vector2(rect.X, rect.Y), out int val1))
                {
                    Vector2 key = new Vector2(rect.X, rect.Y);
                    score += 100;
                    boxes.Remove(key);
                }

                if (teleport.TryGetValue(new Vector2(rect.X, rect.Y), out int val2))
                {
                    player.playerRectangle.X = 100;
                    player.playerRectangle.Y = 235;
                }

                //changes levels if player hits portal after having a certain score
                if (finalportal.TryGetValue(new Vector2(rect.X, rect.Y), out int val3))
                {
                    if (score >= 70)
                    {
                        counter++;
                    }
                }

                // handle collisions if the tile position exists in the tile map layer.
                if (collisions.TryGetValue(new Vector2(rect.X, rect.Y), out int _val) || sidecollisions.TryGetValue(new Vector2(rect.X, rect.Y), out int val))
                {

                    //System.Diagnostics.Debug.WriteLine("intersecting horizontally" + rect);
                    // create temp rect to handle collisions 
                    Rectangle collision = new(
                        rect.X * display_tilesize,
                        rect.Y * display_tilesize,
                        display_tilesize,
                        display_tilesize
                    );


                    // handle collisions based on the direction the player is moving
                    if (player.velocity.X > 0.0f)
                    {
                        player.playerRectangle.X = collision.Left - player.playerRectangle.Width;
                    }
                    else if (player.velocity.X < 0.0f)
                    {
                        player.playerRectangle.X = collision.Right;
                    }
                }
            }

            foreach(var enemy in enemies)
            {
                horizontal_intersectionsEnemies = getIntersectingTilesEnemies(enemy.enemyRectangle);

                if(enemy.enemyRectangle.Intersects(player.playerRectangle))
                {
                    player.healthPoints--;
                }

                foreach(var magic in player.magics)
                {
                    if(enemy.enemyRectangle.Intersects(magic.magicRectangle))
                    {
                        enemy.healthPoints--;
                        magic.faded = true;
                    }
                }

                // Enemy collision horozontally
                foreach (var rectangle in horizontal_intersectionsEnemies)
                {
                    // handle collisions if the tile position exists in the tile map layer.
                    if (collisions.TryGetValue(new Vector2(rectangle.X, rectangle.Y), out int _val) || sidecollisions.TryGetValue(new Vector2(rectangle.X, rectangle.Y), out int val4))
                    {
                        // create temp rect to handle collisions 
                        Rectangle collision = new(
                            rectangle.X * display_tilesize,
                            rectangle.Y * display_tilesize,
                            display_tilesize,
                            display_tilesize
                        );

                        // handle collisions based on the direction the enemy is moving
                        if (enemy.velocity.X > 0.0f)
                        {
                            enemy.velocity.X = 0;
                            enemy.enemyRectangle.X = collision.Left - enemy.enemyRectangle.Width;
                        }
                        else if (enemy.velocity.X < 0.0f)
                        {
                            enemy.velocity.X = 0;
                            enemy.enemyRectangle.X = collision.Right;
                        }
                    }
                }
            }

            // same as horizontal collisions
            player.playerRectangle.Y += (int)player.velocity.Y;
            //System.Diagnostics.Debug.WriteLine(player.playerRectangle.Y);
            vertical_intersections = getIntersectingTilesVertical(player.playerRectangle);

            foreach (var rect in vertical_intersections)
            {
                if (boxes.TryGetValue(new Vector2(rect.X, rect.Y), out int val1))
                {
                    Vector2 key = new Vector2(rect.X, rect.Y);
                    score += 100;
                    boxes.Remove(key);
                }

                if (collisions.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {
                    Rectangle collision = new Rectangle(
                        rect.X * TILESIZE,
                        rect.Y * TILESIZE,
                        TILESIZE,
                        TILESIZE
                    );
                    // System.Diagnostics.Debug.WriteLine("intersecting vertically" + rect);
                    ischaronGround = true;

                    if (player.velocity.Y > 0.0f)
                    {
                        player.playerRectangle.Y = collision.Top - player.playerRectangle.Height;
                    }
                    else if (player.velocity.Y < 0.0f)
                    {
                        player.playerRectangle.Y = collision.Bottom;
                    }

                }
                else
                {
                    ischaronGround = false;
                }

            }

            Rectangle target = player.playerRectangle;

            // camera logic
            _camera.Zoom = 1.0f;

            if (player.playerRectangle.Center.X < _graphics.PreferredBackBufferWidth / 2)
            {
                if (player.playerRectangle.Y < _graphics.PreferredBackBufferHeight / 2)
                {
                    _cameraTarget = new Vector2(40, 0);
                }
                else if (player.playerRectangle.Y > 1088 - _graphics.PreferredBackBufferHeight / 2)
                {
                    _cameraTarget = new Vector2(40, 1088 - _graphics.PreferredBackBufferHeight);
                }
                else
                {
                    _cameraTarget = new Vector2(40, player.playerRectangle.Center.Y - _graphics.PreferredBackBufferHeight / 2);
                }
            }
            else if (player.playerRectangle.Center.X > 5120 - _graphics.PreferredBackBufferWidth / 2)
            {
                if (player.playerRectangle.Y < _graphics.PreferredBackBufferHeight / 2)
                {
                    _cameraTarget = new Vector2(5120 - _graphics.PreferredBackBufferWidth, 0);
                }
                else if (player.playerRectangle.Y > 1088 - _graphics.PreferredBackBufferHeight)
                {
                    _cameraTarget = new Vector2(5120 - _graphics.PreferredBackBufferWidth, 1088 - _graphics.PreferredBackBufferHeight / 2);
                }
                else
                {
                    _cameraTarget = new Vector2(5120 - _graphics.PreferredBackBufferWidth, player.playerRectangle.Center.Y - _graphics.PreferredBackBufferHeight / 2);
                }
            }
            else
            {
                if (player.playerRectangle.Y < (_graphics.PreferredBackBufferHeight / 2))
                {
                    _cameraTarget = new Vector2(player.playerRectangle.Center.X - _graphics.PreferredBackBufferWidth / 2, 0);
                }
                else if (player.playerRectangle.Y > 1088 - _graphics.PreferredBackBufferHeight)
                {
                    _cameraTarget = new Vector2(player.playerRectangle.Center.X - _graphics.PreferredBackBufferWidth / 2, 1088 - _graphics.PreferredBackBufferHeight);
                }
                else
                {
                    _cameraTarget = player.playerRectangle.Center.ToVector2();
                    _cameraTarget -= new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
                }
            }
            float smoothingFactor = 0.05f; // adjust this value to get the desired smoothing effect
            _camera.Position += (_cameraTarget - _camera.Position) * smoothingFactor; // atualiza a posição da camera

        }


        public List<Rectangle> getIntersectingTilesHorizontal(Rectangle target)
        {

            List<Rectangle> horizontal_intersections = new();

            int widthInTiles = (target.Width - (target.Width % TILESIZE)) / TILESIZE;
            int heightInTiles = (target.Height - (target.Height % TILESIZE)) / TILESIZE;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    horizontal_intersections.Add(new Rectangle(

                        (target.X + x * TILESIZE) / display_tilesize,
                        (target.Y + y * (TILESIZE - 1)) / display_tilesize,
                        display_tilesize,
                        display_tilesize

                    ));
                    // System.Diagnostics.Debug.WriteLine($"Checking tile at ({x}, {y}) with rectangle: {target}");
                }
            }

            return horizontal_intersections;
        }

        public List<Rectangle> getIntersectingTilesEnemies(Rectangle enemyRectangle)
        {  
            List<Rectangle> horizontal_intersectionsEnemies = new();

            int widthInTiles = (enemyRectangle.Width - (enemyRectangle.Width % TILESIZE)) / TILESIZE;
            int heightInTiles = (enemyRectangle.Height - (enemyRectangle.Height % TILESIZE)) / TILESIZE;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    horizontal_intersectionsEnemies.Add(new Rectangle(

                        (enemyRectangle.X + x * TILESIZE) / display_tilesize,
                        (enemyRectangle.Y + y * (TILESIZE - 1)) / display_tilesize,
                        display_tilesize,
                        display_tilesize

                    ));
                    // System.Diagnostics.Debug.WriteLine($"Checking tile at ({x}, {y}) with rectangle: {target}");
                }
            }
           
            return horizontal_intersectionsEnemies;
        }

        public List<Rectangle> getIntersectingTilesVertical(Rectangle target)
        {

            List<Rectangle> vertical_intersections = new();

            int widthInTiles = (target.Width - (target.Width % TILESIZE)) / TILESIZE;
            int heightInTiles = (target.Height - (target.Height % TILESIZE)) / TILESIZE;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {

                    vertical_intersections.Add(new Rectangle(

                        (target.X + x * (TILESIZE - 1)) / display_tilesize,
                        (target.Y + y * TILESIZE) / display_tilesize,
                        display_tilesize,
                        display_tilesize

                    ));

                }
            }

            return vertical_intersections;
        }
   

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            int num_tiles_per_row_png = 7; //number of tiles in a row in the texturedic png
            int tilesize = 64;

            if (counter == 0)
            {   
                foreach (var item in background)
                {
                    Rectangle drect = new(
                        (int)item.Key.X * display_tilesize, (int)item.Key.Y * display_tilesize, display_tilesize, display_tilesize
                     );

                    int x = item.Value % num_tiles_per_row_png;
                    int y = item.Value / num_tiles_per_row_png;

                    Rectangle src = new(
                        x * tilesize, y * tilesize, tilesize, tilesize
                        );
                    _spriteBatch.Draw(textureDic, drect, src, Color.White);
                }

                foreach (var item in teleport)
                {
                    Rectangle drect = new(
                        (int)item.Key.X * display_tilesize, (int)item.Key.Y * display_tilesize, display_tilesize, display_tilesize
                     );
                    //value is the tile index in the tileset
                    int x = item.Value % num_tiles_per_row_png;
                    int y = item.Value / num_tiles_per_row_png;

                    Rectangle src = new(
                        x * tilesize, y * tilesize, tilesize, tilesize
                        );
                    _spriteBatch.Draw(textureDic, drect, src, Color.White);
                }

                foreach (var item in finalportal)
                {
                    Rectangle drect = new(
                        (int)item.Key.X * display_tilesize, (int)item.Key.Y * display_tilesize, display_tilesize, display_tilesize
                     );
                    //value is the tile index in the tileset
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
                        (int)item.Key.X * display_tilesize, (int)item.Key.Y * display_tilesize, display_tilesize, display_tilesize
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
                        (int)item.Key.X * display_tilesize, (int)item.Key.Y * display_tilesize, display_tilesize, display_tilesize
                     );
                    //value is the tile index in the tileset
                    int x = item.Value % num_tiles_per_row_png;
                    int y = item.Value / num_tiles_per_row_png;

                    Rectangle src = new(
                        x * tilesize, y * tilesize, tilesize, tilesize
                        );
                    _spriteBatch.Draw(textureDic, drect, src, Color.White);
                }
                foreach (var item in boxes)
                {
                    Rectangle drect = new(
                        (int)item.Key.X * display_tilesize, (int)item.Key.Y * display_tilesize, display_tilesize, display_tilesize
                     );
                    //value is the tile index in the tileset
                    int x = item.Value % num_tiles_per_row_png;
                    int y = item.Value / num_tiles_per_row_png;

                    Rectangle src = new(
                        x * tilesize, y * tilesize, tilesize, tilesize
                        );
                    _spriteBatch.Draw(box, drect, src, Color.White);
                }
            }
            else
            {
                //add another level
            }

            player.Draw(spriteBatch, Color.White, gameTime);

            foreach (var enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            _spriteBatch.DrawString(font, $"Score: {score}", _camera.ScreenToWorld(new Vector2(10, 10)), Color.Yellow);
            _spriteBatch.DrawString(font, $"HP: {player.healthPoints}", _camera.ScreenToWorld(new Vector2(10, 25)), Color.Red);
            _spriteBatch.DrawString(font, $"MP: {player.manaPoints}", _camera.ScreenToWorld(new Vector2(10, 40)), Color.Blue);

            _spriteBatch.End();
        }

        public void Reset()
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.isAlive = false;
            }

            boxes.Clear();

            Initialize();
        }
    }
}

