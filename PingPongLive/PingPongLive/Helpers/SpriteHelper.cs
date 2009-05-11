using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongLive.Helpers
{
  class SpriteHelper
  {
    public Texture2D Texture { get; private set; }
    public Rectangle Rect { get; private set; }

    public SpriteHelper(Texture2D tex, Rectangle rect)
    {
      Rect = rect;
      Texture = tex;
    }
  }
}
