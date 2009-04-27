using System;
using System.Xml;
using XnaTetris.Game;
using XnaTetris.Helpers;
using XnaTetris.Interface;

namespace XnaTetris.Algorithms
{
  public class Level
  {
    private const string LevelsFileName = "Content/Levels.xml";

    public long Time { get; set;}
    public int Number { get; set; }
    public int LevelScore { get; set;}
    public StartLevelWindow  StartWindow { get; set;}
    public SpriteHelper BackgroundSprite { get; set; }
    public LinesGame LinesGame { get; private set;}

    public Level(int number, LinesGame game)
    {
      LinesGame = game;
      Number = number;
      LoadLevel();
    }

    public void LoadLevel()
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(LevelsFileName);
      XmlNode levelNode = doc.SelectSingleNode(String.Format("/levels/level[@number=\"{0}\"]", Number));
      foreach (XmlNode childNode in levelNode.ChildNodes)
      {
        switch (childNode.Name)
        {
          case "time" :
            {
              Time = Convert.ToInt32(childNode.InnerText);
              break;
            }
          case "score":
            {
              LevelScore = Convert.ToInt32(childNode.InnerText);
              break;
            }
          case "background":
            {
              BackgroundSprite = ContentSpace.GetSprite(childNode.InnerText);
              break;
            } 
          case "dialog":
            {
              StartWindow = new StartLevelWindow(LinesGame, childNode);
              break;
            }
          default :
            break;
        }
      }
     // doc = null;
    }

    public string LevelString
    {
      get { return String.Format("Level:  {0}", Number); }
    }
  }
}
