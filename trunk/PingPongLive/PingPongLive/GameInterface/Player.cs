﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PingPongLive.GameInterface
{
  class Player : DrawableGameComponent
  {
    #region Variables

    private Texture2D tex;
    protected PlayerIndex playerIndex;
    public Rectangle SpriteRect { get; private set;}

    protected int score;
    protected int speed = 4;
    protected Rectangle screenBounds;

    #endregion

    #region Properties

    public Vector2 Position;

    public int Score
    {
      get { return score; }
      set {
        score = value < 0 ? 0 : value;
      }
    }


    #endregion

    // Область экрана

    public Player(Game game, Texture2D texture, Rectangle rect, PlayerIndex playerID) : base(game)
    {
      Position = new Vector2();
      playerIndex = playerID;
      tex = texture;
      SpriteRect = rect;
      screenBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width,
                  Game.Window.ClientBounds.Height);
    }

    public void Reset()
    {
      if (playerIndex == PlayerIndex.One)
      {
        Position.X = 20;
      }
      else
      {
        Position.X = screenBounds.Right - 40;
      }
      Position.Y = screenBounds.Height - SpriteRect.Height;
      score = 0;
    }

    public override void Update(GameTime gameTime)
    {

      HandlePlayerKeyBoard();

      // Сохраняем игрока в пределах экрана
      KeepInBound();

      base.Update(gameTime);
    }

    private void HandlePlayerKeyBoard()
    {
      KeyboardState keyboard = Keyboard.GetState();

      if (keyboard.IsKeyDown(Keys.Up))
        Position.Y -= speed;
      else if (keyboard.IsKeyDown(Keys.Down))
        Position.Y += speed;
    }

    private void KeepInBound()
    {
      if (Position.Y < screenBounds.Top)
      {
        Position.Y = screenBounds.Top;
      }
      if (Position.Y > screenBounds.Height - SpriteRect.Height)
      {
        Position.Y = screenBounds.Height - SpriteRect.Height;
      }
    }

    public Rectangle GetBounds()
    {
      return new Rectangle((int)Position.X, (int)Position.Y,
                           SpriteRect.Width, SpriteRect.Height);
    }

    public override void Draw(GameTime gameTime)
    {
      SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

      sBatch.Draw(tex, Position, SpriteRect, Color.White);

      base.Draw(gameTime);
    }
  }
}