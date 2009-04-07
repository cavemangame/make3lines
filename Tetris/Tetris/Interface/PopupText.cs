using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Interface
{
  public class PopupText : DrawableGameComponent
  {
    #region Variables

    // начальная и конечная позиции
    private readonly Vector2 beginPos;
    private readonly Vector2 endPos;

    // настройки шрифта
    private readonly SpriteFont font;
    private readonly float beginScale;
    private readonly float endScale;
    private readonly Color color;

    // время появления
    private readonly long popupTime;

    // начальная и конечная прозрачности
    private readonly int beginOpacity;
    private readonly int endOpacity;

    private readonly string text;

    private SpriteBatch spriteBatch;


    private Vector2 currentPos;
    private float currentScale;
    private int currentOpacity;
    private readonly TimeSpan startTime;

    #endregion

    #region Constructor

    public PopupText(Microsoft.Xna.Framework.Game setGame, GameTime setStartTime, string setText, long setPopupTime, Vector2 setBeginPos, Vector2 setEndPos,
      SpriteFont setFont, float setBeginScale, float setEndScale, Color setColor,
      int setBeginOpacity, int setEndOpacity)
      :base(setGame)
    {
      startTime = setStartTime.TotalRealTime;
      text = setText;
      popupTime = setPopupTime;
      currentPos = beginPos = setBeginPos;
      endPos = setEndPos;
      font = setFont;
      currentScale = beginScale = setBeginScale;
      endScale = setEndScale;
      color = setColor;
      currentOpacity = beginOpacity = setBeginOpacity;
      endOpacity = setEndOpacity;
      spriteBatch = new SpriteBatch(Game.GraphicsDevice);
    }

    public PopupText(Microsoft.Xna.Framework.Game setGame, GameTime setStartTime, string setText, long setPopupTime, Vector2 setBeginPos,
      SpriteFont setFont, Color setColor)
      : this(setGame, setStartTime, setText, setPopupTime, setBeginPos, setBeginPos, setFont, 1f, 1f, setColor, 255, 0)
    {
      
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      if (!IsMoveEnded(gameTime))
      {
        spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

        spriteBatch.DrawString(font, text, currentPos, GetCurrentColor(), 0f,
                               new Vector2(0, 0), currentScale,
                               SpriteEffects.None, 0);

        spriteBatch.End();
      }
      base.Draw(gameTime);
    }

    private Color GetCurrentColor()
    {
      Color result = new Color(color.R, color.G, color.B) {A = ((byte) currentOpacity)};
      return result;
    }

    #endregion

    #region Update

    public override void Update(GameTime gameTime)
    {
      if (IsMoveEnded(gameTime))
        return;

      float percentMove = (float)GetCurrentMovingTime(gameTime) / popupTime;

      currentPos = new Vector2((int) ((endPos.X - beginPos.X)*percentMove) + beginPos.X,
                               (int) ((endPos.Y - beginPos.Y)*percentMove) + beginPos.Y);

      currentScale = (endScale - beginScale)*percentMove + beginScale;
      currentOpacity = (int)((endOpacity - beginOpacity) *percentMove) + beginOpacity;

      base.Update(gameTime);
    }

    public bool IsMoveEnded(GameTime gameTime)
    {
      return GetCurrentMovingTime(gameTime) >= popupTime;
    }

    private long GetCurrentMovingTime(GameTime gameTime)
    {
      return (gameTime.TotalRealTime - startTime).Ticks / TimeSpan.TicksPerMillisecond;
    }

    #endregion
  }
}
