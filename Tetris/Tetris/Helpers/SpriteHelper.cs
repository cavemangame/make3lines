using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaTetris.Helpers
{

  public class SpriteHelper
  {
    public Texture2D texture;
    public Rectangle sourceRect;
    public Color color;

    public SpriteHelper(Texture2D setTexture, Rectangle setSourceRect, Color setColor)
    {
      texture = setTexture;
      sourceRect = setSourceRect;
      color = setColor;
    }

    public SpriteHelper(Texture2D setTexture, Rectangle setSourceRect)
      : this(setTexture, setSourceRect, Color.White)
    {
    }

    public SpriteHelper(Texture2D setTexture)
      : this(setTexture, new Rectangle(0, 0, setTexture.Width, setTexture.Height))
    {
    }

    public void Render(SpriteBatch spriteBatch, Rectangle rect, SpriteBlendMode mode, float scale, 
      float rotation)
    {
      spriteBatch.Begin(mode);
      spriteBatch.Draw(texture, rect, sourceRect, color, rotation, Vector2.Zero, SpriteEffects.None, .0f);
      spriteBatch.End();
    }

    
    public void Render(SpriteBatch spriteBatch, Rectangle rect, SpriteBlendMode mode, float scale)
    {
      Render(spriteBatch, rect, mode, scale, .0f);
    }

    public void Render(SpriteBatch spriteBatch, Rectangle rect, SpriteBlendMode mode)
    {
      Render(spriteBatch, rect, mode, 1f);
    }

    public void Render(SpriteBatch spriteBatch, Rectangle rect)
    {
      Render(spriteBatch, rect, SpriteBlendMode.AlphaBlend);
    }

    public void Render(SpriteBatch spriteBatch, Rectangle rect, Rectangle gfxRect)
    {
      spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
      spriteBatch.Draw(texture, rect, gfxRect, color);
      spriteBatch.End();
    }

    public void Render(Rectangle rect)
    {
      SpriteBatch spriteBatch = new SpriteBatch(texture.GraphicsDevice);
      spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
      spriteBatch.Draw(texture, rect, sourceRect, color);
      spriteBatch.End();

      Render(spriteBatch, rect, SpriteBlendMode.AlphaBlend);
    }
  }
}
