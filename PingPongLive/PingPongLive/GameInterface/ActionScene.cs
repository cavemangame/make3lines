using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongLive.GameInterface
{
  class ActionScene : GameScene
  {
    private ImageComponent background;
    private Player player1, player2;
    private Ball ball;

    public ActionScene(Game game, Texture2D bgTexture, Texture2D gameTex) : base(game)
    {
      background = new ImageComponent(game, bgTexture, DrawMode.Stretch, null);
      Components.Add(background);

      player1 = new Player(game, gameTex, new Rectangle(23, 0, 22, 92), PlayerIndex.One);
      player1.Reset();
      Components.Add(player1);

      player2 = new Player(game, gameTex, new Rectangle(0, 0, 22, 92), PlayerIndex.Two);
      player2.Reset();
      Components.Add(player2);

      ball = new Ball(game, gameTex, new Vector2(400, 300), new Rectangle(1, 94, 33, 33));
      Components.Add(ball);

    }
  }
}
