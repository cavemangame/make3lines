using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

      public TextToRender(SpriteFont font, string text, int x, int y, Color color)
      {
        Font = font;
        Text = text;
        X = x;
        Y = y;
        TextColor = color;
      }

      public TextToRender(SpriteFont font, string text, int x, int y)
        : this(font, text, x, y, Color.White)
      {

      }
    }

    #region Variables

    private static SpriteBatch fontSprite;
    private static List<TextToRender> renderTexts = new List<TextToRender>();

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
      renderTexts.Add(new TextToRender(font, text, x, y));
    }

    public void WriteAll()
    {
      if (renderTexts.Count == 0)
        return;

      // Start rendering
      fontSprite.Begin(SpriteBlendMode.AlphaBlend);

      foreach (TextToRender rt in renderTexts)
      {
        fontSprite.DrawString(rt.Font, rt.Text, new Vector2(rt.X, rt.Y), rt.TextColor);
      }

      // End rendering
      fontSprite.End();

      renderTexts.Clear();
    }
    #endregion

 
  }
}
