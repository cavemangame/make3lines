using System;
using System.Collections.Generic;
using System.Xml;
using XnaTetris.Blocks;
using XnaTetris.Game;
using XnaTetris.Helpers;
using XnaTetris.Interface;

namespace XnaTetris.Algorithms
{
  public class Level
  {
    #region Constants

    private const string LevelsFileName = "Content/Levels.xml";

    #endregion

    #region Variables

    public long Time { get; set;}
    public int Number { get; set; }
    public int LevelScore { get; set;}
    public StartLevelWindow  StartWindow { get; set;}
    public SpriteHelper BackgroundSprite { get; set; }
    public LinesGame LinesGame { get; private set;}

    // chances for super block
    public float Mul2Chance { get; private set; }
    public float Mul3Chance { get; private set; }
    public float Mul5Chance { get; private set; }
    public float NeutralChance { get; private set; }

    // location of invul blocks
    public List<Serv.BlockLocation> invulBlocks = new List<Serv.BlockLocation>();

    public Dictionary<BlockFactory.BlockType, float> blockChances;

    #endregion

    #region Constructor

    public Level(int number, LinesGame game)
    {
      LinesGame = game;
      Number = number;
      InitChances();
      LoadLevel();
    }

    #endregion

    private void InitChances()
    {
      blockChances = new Dictionary<BlockFactory.BlockType, float>
                       {
                         {BlockFactory.BlockType.Blue, 1f},
                         {BlockFactory.BlockType.Red, 1f},
                         {BlockFactory.BlockType.Green, 1f},
                         {BlockFactory.BlockType.Yellow, 1f},
                         {BlockFactory.BlockType.White, 1f},
                         {BlockFactory.BlockType.Gray, 1f}
                       };
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
          case "mul2chance":
            {
              Mul2Chance = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "mul3chance":
            {
              Mul3Chance = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "mul5chance":
            {
              Mul5Chance = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "neutralchance":
            {
              NeutralChance = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "RedChance":
            {
              blockChances[BlockFactory.BlockType.Red] = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "BlueChance":
            {
              blockChances[BlockFactory.BlockType.Blue] = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "GreenChance":
            {
              blockChances[BlockFactory.BlockType.Green] = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "YellowChance":
            {
              blockChances[BlockFactory.BlockType.Yellow] = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "WhiteChance":
            {
              blockChances[BlockFactory.BlockType.White] = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "GrayChance":
            {
              blockChances[BlockFactory.BlockType.Gray] = Convert.ToSingle(childNode.InnerText);
              break;
            }
          case "invulblock":
            {
              AddInvulLocation(childNode);
              break;
            }

          default :
            break;
        }
      }
     // doc = null;
    }

    private void AddInvulLocation(XmlNode node)
    {
      invulBlocks.Add(new Serv.BlockLocation(Convert.ToInt32(node.Attributes["X"].Value),
        Convert.ToInt32(node.Attributes["Y"].Value)));
    }

    public string LevelString
    {
      get { return String.Format("Уровень:  {0}", Number); }
    }

    public BlockFactory.BlockType GetNewRandomBlock(ref int multiplier)
    {
      BlockFactory.BlockType type = RandomBlockHelper.GenerateNewBlockType
        (Serv.ComputeAllChances(LinesGame.Player.blockChances, blockChances));

      multiplier = 1;
      // идем в сторону увеличения
      if (RandomBlockHelper.CheckChance(Mul2Chance + LinesGame.Player.Mul2Chance))
        multiplier = 2;
      if (RandomBlockHelper.CheckChance(Mul3Chance + LinesGame.Player.Mul3Chance))
        multiplier = 3;
      if (RandomBlockHelper.CheckChance(Mul5Chance + LinesGame.Player.Mul5Chance))
        multiplier = 5;

      if (RandomBlockHelper.CheckChance(NeutralChance + LinesGame.Player.NeutralChance))
        return BlockFactory.BlockType.Neutral;

      return type;
    }
  }
}
