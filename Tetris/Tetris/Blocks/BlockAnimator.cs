using System;
using Microsoft.Xna.Framework;

namespace XnaTetris.Blocks
{
  /// <summary>
  /// Обеспечивает перемещение и скалинг прямоугольных блоков
  /// </summary>
  class BlockAnimator
  {
    public const int DEFAULT_MOVE_TIME = 500;

    #region Переменные

    private readonly int moveTime;
    private readonly Rectangle srcRect;
    private readonly Rectangle destRect;
    private readonly TimeSpan startTime;

    #endregion

    #region Конструкторы

    public BlockAnimator(Rectangle srcRect, Rectangle destRect, GameTime gameTime)
      : this(srcRect, destRect, gameTime, DEFAULT_MOVE_TIME)
    {
    }

    public BlockAnimator(Rectangle srcRect, Rectangle destRect, GameTime gameTime, int setMoveTime)
    {
      this.srcRect = srcRect;
      this.destRect = destRect;
      startTime = gameTime.TotalRealTime;
      moveTime = setMoveTime;
    }

    #endregion

    #region Публичные методы

    public Rectangle CurrentRectangle(GameTime gameTime)
    {
      long currentMovingTime = GetCurrentMovingTime(gameTime);

      if (currentMovingTime >= moveTime)
      {
        return destRect;
      }

      float percentMove = (float)currentMovingTime / moveTime;

      return new Rectangle((int)((destRect.X - srcRect.X) * percentMove) + srcRect.X,
        (int)((destRect.Y - srcRect.Y) * percentMove) + srcRect.Y,
        (int)((destRect.Width - srcRect.Width) * percentMove) + srcRect.Width,
        (int)((destRect.Height - srcRect.Height) * percentMove) + srcRect.Height);
    }

    public bool IsMoveEnded(GameTime gameTime)
    {
      return GetCurrentMovingTime(gameTime) >= moveTime;
    }

    #endregion

    private long GetCurrentMovingTime(GameTime gameTime)
    {
      return (gameTime.TotalRealTime - startTime).Ticks/TimeSpan.TicksPerMillisecond;
    }
  }
}
