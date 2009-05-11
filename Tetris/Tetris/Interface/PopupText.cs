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

    // вспомогательные переменные
    private Vector2 currentPos;
    private float currentScale;
    private int currentOpacity;
    private readonly long startTime;
    public event EventHandler EndDrawing;


    #endregion

    #region Constructor

    public PopupText(Microsoft.Xna.Framework.Game setGame, long setStartTime, string setText, long setPopupTime, Vector2 setBeginPos, Vector2 setEndPos,
      SpriteFont setFont, float setBeginScale, float setEndScale, Color setColor,
      int setBeginOpacity, int setEndOpacity)
      :base(setGame)
    {
      startTime = setStartTime;
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
    }

    public PopupText(Microsoft.Xna.Framework.Game setGame, long setStartTime, string setText, long setPopupTime, Vector2 setBeginPos,
      SpriteFont setFont, Color setColor)
      : this(setGame, setStartTime, setText, setPopupTime, setBeginPos, setBeginPos, setFont, 1f, 1f, setColor, 255, 0)
    {
      
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      if (!IsPopupEnded(gameTime))
      {
        TextHelper.DrawShadowedText(font, text, (int)currentPos.X, (int)currentPos.Y, GetCurrentColor(), currentScale);
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
      if (IsPopupEnded(gameTime))
      {
        EndDrawing(this, EventArgs.Empty);
        return;
      }

      float percentMove = (float)GetCurrentPopupTime(gameTime) / popupTime;

      currentPos = new Vector2((int) ((endPos.X - beginPos.X)*percentMove) + beginPos.X,
                               (int) ((endPos.Y - beginPos.Y)*percentMove) + beginPos.Y);

      currentScale = (endScale - beginScale)*percentMove + beginScale;
      currentOpacity = (int)((endOpacity - beginOpacity) *percentMove) + beginOpacity;

      base.Update(gameTime);
    }

    public bool IsPopupEnded(GameTime gameTime)
    {
      return GetCurrentPopupTime(gameTime) >= popupTime;
    }

    private long GetCurrentPopupTime(GameTime gameTime)
    {
      return ((long) gameTime.TotalRealTime.TotalMilliseconds - startTime);
    }

    #endregion
  }
}
