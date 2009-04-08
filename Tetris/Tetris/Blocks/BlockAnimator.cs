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
    private readonly double startTime;
    private int speed;
    private readonly bool isAcceleration;

    #endregion

    #region Конструкторы
    public BlockAnimator(Rectangle srcRect, Rectangle destRect, double startTime)
      : this(srcRect, destRect, startTime, DEFAULT_SPEED) { }

    public BlockAnimator(Rectangle srcRect, Rectangle destRect, double startTime, int speed)
      : this(srcRect, destRect, startTime, speed, false) { }

    public BlockAnimator(Rectangle srcRect, Rectangle destRect, double startTime, bool isAcceleration)
      : this(srcRect, destRect, startTime, DEFAULT_SPEED, isAcceleration) { }

    public BlockAnimator(Rectangle srcRect, Rectangle destRect, double startTime, int speed, bool isAcceleration)
    {
      this.srcRect = srcRect;
      this.destRect = destRect;
      totalDistance = Math.Abs((destRect.X - srcRect.X) + (destRect.Y - srcRect.Y));
      this.startTime = startTime;
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
      return (long)((double)gameTime.TotalRealTime.Ticks / TimeSpan.TicksPerMillisecond - startTime);
    }

    private long GetTotalMovingTime()
    {
      return (long)((float)totalDistance / speed * 1000);
    }
  }
}
