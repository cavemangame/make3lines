using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongLive.GameInterface
{
  class Ball : DrawableGameComponent
  {
    public Vector2 Position { get; set; }
    private SpriteBatch spriteBatch;
    private Texture2D tex;
    private Rectangle spriteRect;

    public Ball(Game game, Texture2D tex, Vector2 pos, Rectangle rect)
      : base(game)
    {
      spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
      this.tex = tex;
      Position = pos;
      spriteRect = rect;
    }

    public override void Draw(GameTime gameTime)
    {
      spriteBatch.Draw(tex, Position, spriteRect, Color.White);
      base.Draw(gameTime);
    }
  }
}
