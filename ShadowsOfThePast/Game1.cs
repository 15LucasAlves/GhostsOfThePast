using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

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

        //loads all the csv maps from tiled into their respective dictionaries
        background = LoadMap("../../Data/level1_background.csv");
        platforms = LoadMap("../../Data/level1_platforms.csv");
        props = LoadMap("../../Data/level1_props.csv");
        house = LoadMap("../../Data/level1_house.csv");
        collisions = LoadMap("../../Data/level1_collisions.csv");

        //initializes camera position to zero
        camera = Vector2.Zero;
    }

    //loads a csv map from tiled and returns a dictionary with the tile positions and tile ids
    //an csv file is basically a file composed by numbers, the numbers symbolize the tile id, or, the tile used from the mmm png
    //this dictionary will associate a vector2(position) to an int value(tile id)
    private Dictionary<Vector2, int> LoadMap(string filepath)
    {
        //creates the dictionary to store the map 
        Dictionary<Vector2, int> result = new();
        StreamReader reader = new(filepath);

        int y = 0;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            //reads the csv file and uses a comma as the delimiter for the tile id
            string[] items = line.Split(',');
            for (int x = 0; x < items.Length; x++)
            {
                //parses string to int
                if (int.TryParse(items[x], out int value))
                {
                    if (value > -1)
                    {
                        //add the tile position and id to the dictionary(tile position as key and id as value)
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
        //if score reaches 500 or more then the score is once again set to 0
        //and the counter is increased by 1
        //(condition in draw function makes it so it will only draw level 1 while the counter is 0 and so on)
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
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        //load the tileset png used to make the tileset on tiled
        textureDic = Content.Load<Texture2D>("mmm");
        collidortext = Content.Load<Texture2D>("collision");
        //loads a font to use on the "score"
        font = Content.Load<SpriteFont>("File");
        playertext = Content.Load<Texture2D>("playertext");

        Texture2D texture = Content.Load<Texture2D>("playertext");
        player = new Player(texture, 2, 4);
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
        else if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            camera.Y += 5;
        }

        player.Update(gameTime);

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
        int tilesize = 32; //size of tile

        if (counter == 0)
        {
            foreach (var item in background) //for each tile in the background dictionary
            {
                //caculates the destination rectangle aka where the tile will be drawn on the screen, based on the .key which is the x and y position in the dictionary
                Rectangle drect = new(
                    (int)item.Key.X * display_tilesize + (int)camera.X, (int)item.Key.Y * display_tilesize + (int)camera.Y, display_tilesize, display_tilesize
                 );

                int x = item.Value % num_tiles_per_row_png;
                int y = item.Value / num_tiles_per_row_png;

                //calculates the source rectangle position on the tileset atlas (tilesetDic) based on the tile id given by .value
                Rectangle src = new(
                    x * tilesize, y * tilesize, tilesize, tilesize
                    );
                //draws the actual tile on screen until ther are not more items in the choosen dictionary
                _spriteBatch.Draw(textureDic, drect, src, Color.White);
            }

            //every single foreach below follows the same rules as the one above

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

            player.Draw(_spriteBatch, new Vector2(200, 200), Color.White);

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