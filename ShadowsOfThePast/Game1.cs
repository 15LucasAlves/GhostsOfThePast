using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;
using System;
using ShadowsOfThePast.Interface;
using Microsoft.Xna.Framework.Content;
using System.Reflection.Metadata;

namespace ShadowsOfThePast;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private StateManager _stateManager;
    private mainMenu _mainMenu;
    private levels _levels;
    private Player _player;
    int counter = 1;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _stateManager = new StateManager();
        _stateManager.ChangeState(new levels(this, GraphicsDevice, _spriteBatch, Content));
        _mainMenu = new mainMenu(this, GraphicsDevice, _spriteBatch, Content);
        _levels = new levels(this, GraphicsDevice, _spriteBatch, Content);
        _player = new Player(this, GraphicsDevice, _spriteBatch, Content); 

        _stateManager.ChangeState(_mainMenu);
        base.Initialize();


    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _stateManager.LoadContent(Content, _spriteBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (_stateManager.CurrentState == _mainMenu && Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            _stateManager.ChangeState(_levels);
            LoadContent();
        }

        _stateManager.Update(gameTime, GraphicsDevice);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);


        _stateManager.Draw(gameTime, _spriteBatch);

        base.Draw(gameTime);
    }
}