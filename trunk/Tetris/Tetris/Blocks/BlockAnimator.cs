using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaTetris.Blocks
{
	/// <summary>
	/// Обеспечивает перемещение и скалинг прямоугольных блоков
	/// </summary>
	class BlockAnimator
	{
		#region Переменные

		private readonly int moveTime = 500;
		private Rectangle oldRect;
		private readonly Rectangle newRect;
		private readonly TimeSpan startTime;

		#endregion

		#region Конструкторы

		public BlockAnimator(Rectangle setOldRect, Rectangle setNewRect, GameTime gameTime)
		{
			oldRect = setOldRect;
			newRect = setNewRect;
			startTime = gameTime.TotalRealTime;
		}

		public BlockAnimator(Rectangle setOldRect, Rectangle setNewRect, GameTime gameTime, int setMoveTime)
			: this(setOldRect, setNewRect, gameTime)
		{
			moveTime = setMoveTime;
		}

		#endregion

		#region Публичные методы

		public Rectangle CurrentRectangle(GameTime gameTime)
		{
			int pastPartTime = (gameTime.TotalRealTime - startTime).Milliseconds;
			if (pastPartTime >= moveTime)
				return newRect;

			float percentMove = (float)pastPartTime / moveTime;

			return new Rectangle((int)((newRect.X - oldRect.X) * percentMove) + oldRect.X, 
				(int)((newRect.Y - oldRect.Y) * percentMove) + oldRect.Y,
				(int)((newRect.Width - oldRect.Width) * percentMove) + oldRect.Width,
				(int)((newRect.Height - oldRect.Height) * percentMove) + oldRect.Height);
		}

		public bool IsMoveEnded(GameTime gameTime)
		{				
			return ((gameTime.TotalRealTime - startTime).Milliseconds >= moveTime);
		}

		#endregion
	}
}
