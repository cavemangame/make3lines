using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PingPongLive.Helpers;

namespace PingPongLive.GameInterface
{
  class ActionScene : GameScene
  {
    #region Variables 

    private ImageComponent background;
    private Ball ball;
    private TimeSpan elapsedTime = TimeSpan.Zero;
    private readonly NetworkHelper networkHelper;
    private readonly SpriteFont textFont;

    public bool IsGameOver { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public bool Paused { get; set; }
    #endregion

    #region Constructor

    public ActionScene(Game game, Texture2D bgTexture, Texture2D gameTex, SpriteFont font) : base(game)
    {
      textFont = font;

      background = new ImageComponent(game, bgTexture, DrawMode.Stretch, null);
      Components.Add(background);

      Player1 = new Player(game, gameTex, new Rectangle(23, 0, 22, 92), PlayerIndex.One);
      Player1.Reset();
      Components.Add(Player1);

      Player2 = new Player(game, gameTex, new Rectangle(0, 0, 22, 92), PlayerIndex.Two);
      Player2.Reset();
      Components.Add(Player2);

      ball = new Ball(game, gameTex, new Vector2(400, 300), new Rectangle(1, 94, 33, 33));
      Components.Add(ball);

      networkHelper = (NetworkHelper)Game.Services.GetService(typeof(NetworkHelper));

      IsGameOver = false;
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      if (Paused)
        TextHelper.DrawShadowedText(textFont, "PAUSED", 400, 300, Color.Red, 1.5f);
      base.Draw(gameTime);
    }
    #endregion

    #region Update

    public override void Update(GameTime gameTime)
    {
      if (!Paused)
      {
        elapsedTime += gameTime.ElapsedGameTime;
        if (elapsedTime > TimeSpan.FromMilliseconds(10))
        {
          ball.MoveBall();
          ball.HandleBoundField();
          ball.HandleBoundPaddle(Player1);
          ball.HandleBoundPaddle(Player2);
          ball.UpdateNetworkData();
          if (ball.HandleLoose())
            IsGameOver = true;

          elapsedTime = TimeSpan.Zero;
        }
      }
      base.Update(gameTime);
    }

    #endregion

    #region Handle Network Data

    public void HandleClientData()
    {
      while (networkHelper.ClientPacketReader.PeekChar() != -1)
      {
        char header = networkHelper.ClientPacketReader.ReadChar();

        switch (header)
        {
          case 'P':
            Paused = networkHelper.ClientPacketReader.ReadBoolean();
            break;
          case 'S':
            Player2.Position = networkHelper.ClientPacketReader.ReadVector2();
            break;
        }
      }
    }

    public void HandleServerData()
    {
      while (networkHelper.ServerPacketReader.PeekChar() != -1)
      {
        char header = networkHelper.ServerPacketReader.ReadChar();

        switch (header)
        {
          case 'P':
            Paused = networkHelper.ServerPacketReader.ReadBoolean();
            break;
          case 'S':
            Player1.Position = networkHelper.ServerPacketReader.ReadVector2();
            break;
          case 'B':
            ball.Position = networkHelper.ServerPacketReader.ReadVector2();
            ball.Speed = networkHelper.ServerPacketReader.ReadInt32();
            ball.SpeedVector = networkHelper.ServerPacketReader.ReadVector2();
            break;

        }
      }
    }


    #endregion
  }
}
