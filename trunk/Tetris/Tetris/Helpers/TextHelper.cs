using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTetris.Helpers
{
  class TextHelper
  {
    public class TextToRender
    {
      public SpriteFont Font { get; private set;}
      public string Text { get; private set;}
      public int X { get; private set;} 
      public int Y { get; private set;}
      public Color TextColor { get; private set; }
      public float Scale { get; private set; }

      public TextToRender(SpriteFont font, string text, int x, int y, Color color, float scale)
      {
        Font = font;
        Text = text;
        X = x;
        Y = y;
        TextColor = color;
        Scale = scale;
      }
    }

    #region Variables

    private static SpriteBatch fontSprite;
    private static readonly List<TextToRender> renderTexts = new List<TextToRender>();

    #endregion

    #region Constructor

    public TextHelper(GraphicsDevice device)
		{
			fontSprite = new SpriteBatch(device);
		}

    #endregion

    #region Draw Text

    public static void DrawText(SpriteFont font, string text, int x, int y)
    {
      DrawText(font, text, x, y, Color.White);
    }

    public static void DrawText(SpriteFont font, string text, int x, int y, Color color)
    {
      DrawText(font, text, x, y, color, 1f);
    }

    // хер проссыт как из SpriteFont добыть размер, поэтому метод со scale
    public static void DrawText(SpriteFont font, string text, int x, int y, Color color, float scale)
    {
      renderTexts.Add(new TextToRender(font, text, x, y, color, scale));
    }

    public static void DrawShadowedText(SpriteFont font, string text, int x, int y)
    {
      DrawShadowedText(font, text, x, y, Color.White);
    }

    public static void DrawShadowedText(SpriteFont font, string text, int x, int y, Color color)
    {
      DrawShadowedText(font, text, x, y, color, 1f);
    }

    // хер проссыт как из SpriteFont добыть размер, поэтому метод со scale
    public static void DrawShadowedText(SpriteFont font, string text, int x, int y, Color color, float scale)
    {
      DrawText(font, text, x + 1, y + 1, Color.Black, scale); //shadow
      DrawText(font, text, x, y, color, scale);
    }

    public void WriteAll()
    {
      if (renderTexts.Count == 0)
        return;

      fontSprite.Begin(SpriteBlendMode.AlphaBlend);

      foreach (TextToRender rt in renderTexts)
      {
        fontSprite.DrawString(rt.Font, rt.Text, new Vector2(rt.X, rt.Y), rt.TextColor, 0f,
          new Vector2(0, 0), rt.Scale, SpriteEffects.None, 0);
      }

      fontSprite.End();

      renderTexts.Clear();
    }
    #endregion

 
  }
}
