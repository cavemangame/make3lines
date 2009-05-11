using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using PingPongLive.Helpers;

namespace PingPongLive.GameInterface
{
  class Player : DrawableGameComponent
  {
    #region Variables

    private Texture2D tex;
    protected PlayerIndex playerIndex;
    protected int score;
    protected int speed = 4;
    protected Rectangle screenBounds;
    private readonly NetworkHelper networkHelper;

    public Rectangle SpriteRect { get; private set; }
    public NetworkGamer Gamer { get; set; }

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

    #region Constructor

    public Player(Game game, Texture2D texture, Rectangle rect, PlayerIndex playerID) : base(game)
    {
      Position = new Vector2();
      playerIndex = playerID;
      tex = texture;
      SpriteRect = rect;
      screenBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
      networkHelper = (NetworkHelper)Game.Services.GetService(typeof(NetworkHelper));
    }

    #endregion

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
      if (networkHelper.NetworkGameSession != null)
      {
        if (Gamer.IsLocal)
        {
          // Локальный игрок всегда использует основной игровой пульт и клавиши клавиатуры
          HandleInput(PlayerIndex.One);
          KeepInBound();
          UpdateNetworkData();
        }
      }
      else
      {
        HandleInput(playerIndex);
      }

      base.Update(gameTime);
    }

    private void HandleInput(PlayerIndex playerIdx)
    {
      if (playerIdx == playerIndex)
      {
        KeyboardState keyboard = Keyboard.GetState();

        if (keyboard.IsKeyDown(Keys.Up))
          Position.Y -= speed;
        else if (keyboard.IsKeyDown(Keys.Down))
          Position.Y += speed;
      }
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
      return new Rectangle((int)Position.X, (int)Position.Y, SpriteRect.Width, SpriteRect.Height);
    }

    private void UpdateNetworkData()
    {
      if (networkHelper.NetworkGameSession.IsHost)
      {
        networkHelper.ServerPacketWriter.Write('S');
        networkHelper.ServerPacketWriter.Write(Position);
      }
      else
      {
        networkHelper.ClientPacketWriter.Write('S');
        networkHelper.ClientPacketWriter.Write(Position);
      }
    }

    #region Draw
    public override void Draw(GameTime gameTime)
    {
      SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

      sBatch.Draw(tex, Position, SpriteRect, Color.White);

      base.Draw(gameTime);
    }
    #endregion
  }
}
