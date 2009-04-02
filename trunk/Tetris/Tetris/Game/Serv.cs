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
	}
}
