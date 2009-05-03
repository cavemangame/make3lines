using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaTetris.Blocks;

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

    public class BlockLocation
    {
      public int X { get; private set; }
      public int Y { get; private set; }

      public BlockLocation(int x, int y)
      {
        X = x;
        Y = y;
      }
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

	  public static Dictionary<BlockFactory.BlockType, float> ComputeAllChances(Dictionary<BlockFactory.BlockType, float> chances, Dictionary<BlockFactory.BlockType, float> blockChances)
	  {
	    var result = new Dictionary<BlockFactory.BlockType, float>();
	    foreach (var pair in chances)
	    {
	      result.Add(pair.Key, pair.Value * (blockChances.ContainsKey(pair.Key) ? blockChances[pair.Key] : 1f));    
	    }
	    return result;
 	  }
	}
}
