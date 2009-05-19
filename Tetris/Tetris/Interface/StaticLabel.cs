using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;

namespace XnaTetris.Interface
{
  class StaticLabel : LinesControl
  {
    #region Variables

    private SpriteHelper bgSprite;
    private SpriteHelper bgHiSprite;

    private bool IsMouseOver;

    private float textScale = 1.0f;
    private Color textColor = Color.White;
    private SpriteFont textFont;

    public event EventHandler MousePressed;

    #endregion

    #region Properties

    public string Text { get; private set; }

    #endregion

    #region Constructors

    public StaticLabel(Microsoft.Xna.Framework.Game game, Rectangle setRect, string text, SpriteFont font)
      : base(game, setRect)
    {
      this.textFont = font;
      Text = text;
      LoadOtherDataIfNeeded();
    }

    public StaticLabel(Microsoft.Xna.Framework.Game game, Rectangle setRect, string text, SpriteFont font, Color color)
      : this(game, setRect, text, font)
    {
      textColor = color;
    }

    public StaticLabel(Microsoft.Xna.Framework.Game game, Rectangle setRect, string text, SpriteFont font, Color color, float scale)
      : this(game, setRect, text, font, color)
    {
      textScale = scale;
    }

    #endregion



    protected void LoadOtherDataIfNeeded()
    {
      if (textFont == null)
        textFont = ContentSpace.GetFont("NormalFont");
      if (bgSprite == null) //создадим спрайт ручками
      {
        Texture2D backgroundTexture = new Texture2D(Game.GraphicsDevice, 1, 1, 1,
                            TextureUsage.Linear, SurfaceFormat.Color);
        backgroundTexture.SetData(new[] { ColorHelper.SmallAlphaBlack });
        bgSprite = new SpriteHelper(backgroundTexture, BoundingRect);
      }
      if (bgHiSprite == null) //создадим спрайт ручками
      {
        Texture2D backgroundHiTexture = new Texture2D(Game.GraphicsDevice, 1, 1, 1,
                            TextureUsage.Linear, SurfaceFormat.Color);
        backgroundHiTexture.SetData(new[] { ColorHelper.SmallAlphaBlack });
        bgHiSprite = new SpriteHelper(backgroundHiTexture, BoundingRect);
      }
    }

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      if (IsMouseOver)
        bgHiSprite.Render(BoundingRect);
      else
        bgSprite.Render(BoundingRect);

      TextHelper.DrawShadowedText(textFont, Text, BoundingRect.X + 2,
                                  BoundingRect.Y + 1, textColor, textScale);

      base.Draw(gameTime);
    }

    #endregion

    #region Events

    protected override void HandleMouseClick()
    {
      //MousePressed(this, EventArgs.Empty);
    }

     protected override void HandleMouseOver(bool isOver)
     {
       //IsMouseOver = isOver;
     }

    #endregion
  }
}
