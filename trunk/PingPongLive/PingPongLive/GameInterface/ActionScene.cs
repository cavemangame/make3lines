using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongLive.GameInterface
{
  class ActionScene : GameScene
  {
    #region Variables 

    private ImageComponent background;
    private Ball ball;
    private TimeSpan elapsedTime = TimeSpan.Zero;

    public bool IsGameOver { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    #endregion

    #region Constructor

    public ActionScene(Game game, Texture2D bgTexture, Texture2D gameTex) : base(game)
    {
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

      IsGameOver = false;
    }

    #endregion

    public override void Update(GameTime gameTime)
    {
      elapsedTime += gameTime.ElapsedGameTime;
      if (elapsedTime > TimeSpan.FromMilliseconds(10))
      {
        ball.MoveBall();
        ball.HandleBoundField();
        ball.HandleBoundPaddle(Player1);
        ball.HandleBoundPaddle(Player2);

        if (ball.HandleLoose())
          IsGameOver = true;

        elapsedTime = TimeSpan.Zero;
      }
      base.Update(gameTime);
    }
  }
}
