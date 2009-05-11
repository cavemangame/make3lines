#region Using directives
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace XnaTetris.Helpers
{
  /// <summary>
  /// Sprite helper class to manage and render sprites.
  /// </summary>
  public class SpriteHelper
  {
    #region SpriteToRender helper class
    private class SpriteToRender
    {
      public readonly Texture2D texture;
      public Rectangle rect;
      public readonly Rectangle? sourceRect;
      public readonly Color color;

      public SpriteToRender(Texture2D setTexture, Rectangle setRect,
        Rectangle? setSourceRect, Color setColor)
      {
        texture = setTexture;
        rect = setRect;
        sourceRect = setSourceRect;
        color = setColor;
      } // SpriteToRender(setTexture, setRect, setColor)
    } // SpriteToRender
    #endregion

    #region Variables
    /// <summary>
    /// Keep a list of all sprites we have to render this frame.
    /// </summary>
    private static readonly List<SpriteToRender> sprites = new List<SpriteToRender>();
    private static SpriteBatch spriteBatch;

    private readonly Texture2D texture;
    /// <summary>
    /// Graphic rectangle used for this sprite inside the texture.
    /// Can be null to use the whole texture.
    /// </summary>
    public Rectangle GfxRect { get; private set; }
    #endregion

    public static SpriteBatch SpriteBatch { get { return spriteBatch; } }

    #region Constructor
    public SpriteHelper(Texture2D setTexture, Rectangle? setGfxRect)
    {
      texture = setTexture;
      GfxRect = setGfxRect == null ? new Rectangle(0, 0, texture.Width, texture.Height) : setGfxRect.Value;
    } // SpriteHelper(setTexture, setGfxRect)
    #endregion

    #region Dispose
    /// <summary>
    /// Dispose the static spriteBatch and sprites helpers in case
    /// the device gets lost.
    /// </summary>
    public static void Dispose()
    {
      sprites.Clear();
      if (spriteBatch != null)
        spriteBatch.Dispose();
      spriteBatch = null;
    } // Dispose()
    #endregion

    #region RenderSprite
    public void Render(Rectangle rect, Color color, Rectangle _gfxRect)
    {
      sprites.Add(new SpriteToRender(texture, rect, _gfxRect, color));
    }

    public void Render(Rectangle rect, Color color)
    {
      Render(rect, color, GfxRect);
    } // Render(texture, rect, sourceRect, color)

    public void Render(Rectangle rect)
    {
      Render(rect, Color.White);
    } // Render(texture, rect, sourceRect)

    public void Render(int x, int y, Color color)
    {
      Render(new Rectangle(x, y, GfxRect.Width, GfxRect.Height), color);
    } // Render(texture, rect, sourceRect)

    public void Render(int x, int y)
    {
      Render(new Rectangle(x, y, GfxRect.Width, GfxRect.Height));
    } // Render(texture, rect, sourceRect)
    #endregion

    #region DrawSprites
    public static void DrawSprites()
    {
      // No need to render if we got no sprites this frame
      if (sprites.Count == 0)
        return;

      // Create sprite batch if we have not done it yet.
      // Use device from texture to create the sprite batch.
      if (spriteBatch == null)
        spriteBatch = new SpriteBatch(sprites[0].texture.GraphicsDevice);

      Texture2D lastSpriteTexture = null;
      bool spriteBatchStarted = false;
      // Render all sprites
      foreach (SpriteToRender sprite in sprites)
      {
        // Start rendering sprites
        // Note: Now moved inside loop to fix most render sorting errors!
        if (lastSpriteTexture != sprite.texture)
        {
          if (spriteBatchStarted)
            spriteBatch.End();
          spriteBatchStarted = true;
          spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
            SpriteSortMode.BackToFront, SaveStateMode.None);
        } // if

        spriteBatch.Draw(sprite.texture,
          // Rescale to fit resolution
          new Rectangle(
          sprite.rect.X,
          sprite.rect.Y,
          sprite.rect.Width,
          sprite.rect.Height),
          sprite.sourceRect, sprite.color);
      } // foreach

      // We are done, draw everything on screen with help of the end method.
      if (spriteBatchStarted)
        spriteBatch.End();

      // Kill list of remembered sprites
      sprites.Clear();
    } // DrawSprites()
    #endregion
  } // class SpriteHelper
} // namespace XnaTetris
