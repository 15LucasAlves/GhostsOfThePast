using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;
using System;

namespace ShadowsOfThePast;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Dictionary<Vector2, int> background;
    private Dictionary<Vector2, int> platforms;
    private Dictionary<Vector2, int> props;
    private Dictionary<Vector2, int> house;
    private Dictionary<Vector2, int> collisions;
    private Texture2D textureDic;
    private Texture2D collidortext;
    private Texture2D playertext;
    private Vector2 camera;
    private Player player;
    private SpriteFont font;
    //const float gravity = 9.8f;
    private int counter = 0;
    int score = 0;
    int display_tilesize = 32;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

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

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
        Player player = new Player();

    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        //load the tileset png used to make the tileset on tiled
        textureDic = Content.Load<Texture2D>("mmm");
        collidortext = Content.Load<Texture2D>("collision");
        font = Content.Load<SpriteFont>("File");
        playertext = Content.Load<Texture2D>("playertext");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //simple camera to look over the level, later we should make it track the player instead
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            camera.X = Math.Min(camera.X - 5, GraphicsDevice.Viewport.Width);
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            camera.X = Math.Min(camera.X + 5, 0);
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            camera.Y -= 5;
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
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

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

        _spriteBatch.DrawString(font, "Score:", new Vector2(5, 0), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}