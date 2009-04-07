using System;
using Microsoft.Xna.Framework;

namespace XnaTetris.Blocks
{
  class BlockAnimator
  {
    public const int DEFAULT_SPEED = 200;
    private const int SPEED_ADDITION = 7;

    #region Переменные

    private readonly Rectangle srcRect;
    private readonly Rectangle destRect;
    private readonly int totalDistance;
    private readonly TimeSpan startTime;
    private int speed;
    private readonly bool isAcceleration;

    #endregion

    #region Конструкторы
    public BlockAnimator(Rectangle srcRect, Rectangle destRect, GameTime gameTime)
      : this(srcRect, destRect, gameTime, DEFAULT_SPEED) {}

    public BlockAnimator(Rectangle srcRect, Rectangle destRect, GameTime gameTime, int speed)
      : this(srcRect, destRect, gameTime, speed, false) { }

    public BlockAnimator(Rectangle srcRect, Rectangle destRect, GameTime gameTime, bool isAcceleration)
      : this(srcRect, destRect, gameTime, DEFAULT_SPEED, isAcceleration) { }

    public BlockAnimator(Rectangle srcRect, Rectangle destRect, GameTime gameTime, int speed, bool isAcceleration)
    {
      this.srcRect = srcRect;
      this.destRect = destRect;
      totalDistance = Math.Abs((destRect.X - srcRect.X) + (destRect.Y - srcRect.Y));
      startTime = gameTime.TotalRealTime;
      this.speed = speed;
      this.isAcceleration = isAcceleration;
    }
    #endregion

    #region Публичные методы

    public Rectangle CurrentRectangle(GameTime gameTime)
    {
      if (isAcceleration)
      {
        speed += SPEED_ADDITION;
      }

      long totalMovingTime = GetTotalMovingTime();
      long currentMovingTime = GetCurrentMovingTime(gameTime);

      if (currentMovingTime >= totalMovingTime)
      {
        return destRect;
      }

      float percentMove = (float)currentMovingTime / totalMovingTime;

      return new Rectangle((int)((destRect.X - srcRect.X) * percentMove) + srcRect.X,
                           (int)((destRect.Y - srcRect.Y) * percentMove) + srcRect.Y,
                           srcRect.Width, srcRect.Height);
    }

    public bool IsMoveEnded(GameTime gameTime)
    {
      return GetCurrentMovingTime(gameTime) >= GetTotalMovingTime();
    }

    #endregion

    private long GetCurrentMovingTime(GameTime gameTime)
    {
      return (gameTime.TotalRealTime - startTime).Ticks/TimeSpan.TicksPerMillisecond;
    }

    private long GetTotalMovingTime()
    {
      return (long)((float)totalDistance / speed * 1000);
    }
  }
}
