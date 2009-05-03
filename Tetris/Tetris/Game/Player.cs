using System;
using System.Collections.Generic;
using System.Xml;
using XnaTetris.Algorithms;
using XnaTetris.Blocks;
using XnaTetris.Interface;

namespace XnaTetris.Game
{
  public class Player
  {
    private const string PlayerFileName = @"Content/player.xml";
    public Dictionary<BlockFactory.BlockType, float> blockChances;

    // chances for super block
    public float Mul2Chance { get; private set; }
    public float Mul3Chance { get; private set; }
    public float Mul5Chance { get; private set; }
    public float NeutralChance { get; private set; }

    public Scores PlayerScore { get; private set; }
    public int PlayerLevel { get; private set; }

    public Player()
    {
      PlayerScore = new Scores();
      InitChances();
      Load();
    }

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

    private void Load()
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(PlayerFileName);
      XmlNode levelNode = doc.SelectSingleNode(String.Format("/player"));
      foreach (XmlNode childNode in levelNode.ChildNodes)
      {
        switch (childNode.Name)
        {
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

          case "BlueScore":
            {
              PlayerScore.BlueScore = Convert.ToInt32(childNode.InnerText);
              break;
            }
          case "RedScore":
            {
              PlayerScore.RedScore = Convert.ToInt32(childNode.InnerText);
              break;
            }
          case "GreenScore":
            {
              PlayerScore.GreenScore = Convert.ToInt32(childNode.InnerText);
              break;
            }
          case "YellowScore":
            {
              PlayerScore.YellowScore = Convert.ToInt32(childNode.InnerText);
              break;
            }
          case "WhiteScore":
            {
              PlayerScore.WhiteScore = Convert.ToInt32(childNode.InnerText);
              break;
            }
          case "GrayScore":
            {
              PlayerScore.GrayScore = Convert.ToInt32(childNode.InnerText);
              break;
            }
          case "AllScore":
            {
              PlayerScore.OverallScore = Convert.ToInt32(childNode.InnerText);
              break;
            }

          case "Level":
            {
              PlayerLevel = Convert.ToInt32(childNode.InnerText);
              break;
            }

          default:
            break;
        }
      }
      // doc = null;
    }

    private void Save()
    {
      
    }
  }
}
