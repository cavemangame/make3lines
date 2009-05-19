using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using XnaTetris.Algorithms;
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
			GameStateBetweenLevel
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
			return String.Format("Время:  {0}:{1:D2}", min, sec);
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

    private static string playerName = String.Empty;
	  private static string confPath = @"Content/config.xml";
    public static string PlayerName
    {
      get
      {
        if (playerName.Length > 0)
          return playerName;

        XmlDocument doc = new XmlDocument();
        if (File.Exists(confPath))
        {
          doc.Load(confPath);
          XmlNode node = doc.SelectSingleNode(String.Format("/config/PlayerName"));
          if (node != null)
          {
            return node.InnerText;
          }
        }
        return String.Empty;
      }
      set
      {
        if (value.Length == 0)
          return;

        playerName = value;
        XmlDocument doc = new XmlDocument();
        if (File.Exists(confPath))
        {
          doc.Load(confPath);
          XmlNode node = doc.SelectSingleNode(String.Format("/config/PlayerName"));
          if (node != null)
          {
            node.InnerText = value;
          }
          doc.Save(confPath);
        }
      }
    }

    public static string GetGodNameByScore(Scores scores)
    {
      string godName = String.Empty;
      long maxScore = 0;
      if (scores.BlueScore > maxScore)
      {
        maxScore = scores.BlueScore;
        godName = "Митра";
      }
      if (scores.RedScore > maxScore)
      {
        maxScore = scores.RedScore;
        godName = "Ворган";
      }
      if (scores.GreenScore > maxScore)
      {
        maxScore = scores.GreenScore;
        godName = "Аниэль";
      }
      if (scores.YellowScore > maxScore)
      {
        maxScore = scores.YellowScore;
        godName = "Дамарх";
      }
      if (scores.WhiteScore > maxScore)
      {
        maxScore = scores.WhiteScore;
        godName = "Арх";
      }
      if (scores.GrayScore > maxScore)
      {
        maxScore = scores.GrayScore;
        godName = "Ом";
      }

      return godName;
    }

    public static string GetGodColorByScore(Scores scores)
    {
      string colorName = String.Empty;
      long maxScore = 0;
      if (scores.BlueScore > maxScore)
      {
        maxScore = scores.BlueScore;
        colorName = "Blue";
      }
      if (scores.RedScore > maxScore)
      {
        maxScore = scores.RedScore;
        colorName = "Red";
      }
      if (scores.GreenScore > maxScore)
      {
        maxScore = scores.GreenScore;
        colorName = "Green";
      }
      if (scores.YellowScore > maxScore)
      {
        maxScore = scores.YellowScore;
        colorName = "Yellow";
      }
      if (scores.WhiteScore > maxScore)
      {
        maxScore = scores.WhiteScore;
        colorName = "White";
      }
      if (scores.GrayScore > maxScore)
      {
        maxScore = scores.GrayScore;
        colorName = "Gray";
      }

      return colorName;
    }

    public static void SaveHiScoreIfNeeded(Algorithms.Scores scores)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load("Content/hiscores.xml");
      XmlNode node = doc.SelectSingleNode(String.Format("/hiscores"));
      foreach (XmlNode n in node.ChildNodes)
      {
        // считаем, что всегда рекорды упорядочены по убыванию
        if (scores.OverallScore >= Convert.ToInt32(n.Attributes["score"].Value))
        {
          n.Attributes["score"].Value = scores.OverallScore.ToString();
          n.Attributes["text"].Value = String.Format("{0}, {1}", playerName,
                                                        Title.GetTitle(scores.OverallScore));
          n.Attributes["color"].Value = GetGodColorByScore(scores);
          break;
        }
      }
      doc.Save("Content/hiscores.xml");
    }

    public static void TestHiScoresSave()
    {
      playerName = "NECROMANT";
      Scores sc = new Scores();
      sc.BlueScore = 8;
      sc.OverallScore = 110000;
      SaveHiScoreIfNeeded(sc);
    }
  }
}
