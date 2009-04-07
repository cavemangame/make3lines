using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaTetris.Game
{
	public class Serv
	{
		public enum MoveDirection
		{
			Left,
			Right,
			Down,
			Up
		}

		public enum GameState
		{
			GameStateMenu,
			GameStatePause,
			GameStateRunning,
			GameStateLevelEnd
		}

		public static Rectangle EmptyRect
		{
			get { return Rectangle.Empty; }
		}

		public static string GetTimeString(long time)
		{
			long min = time/(60*1000);
			long sec = (time/1000) % 60;
			return String.Format("Time:  {0}:{1:D2}", min, sec);
		}

		public static bool PointInRectangle(Point point, Rectangle rect)
		{
			bool result;
			rect.Contains(ref point, out result);
			return result;
		}

		public static Point CorrectPositionWithGameScale(Point point)
		{
			return new Point(point.X*1024/BaseGame.Width, point.Y*768/BaseGame.Height);
		}

    public static Point InvertCorrectPositionWithGameScale(Point point)
    {
      return new Point(point.X * BaseGame.Width / 1024, point.Y * BaseGame.Height / 768);
    }
	}
}
