using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using XnaTetris.Algorithms;
using XnaTetris.Blocks;

namespace XnaTetris.Game
{
  public class Player
  {
    public string PlayerFileName 
    {
      get { return String.Format(@"Content/{0}.xml", playerName); }
    }

    public string DefaultPlayerFileName
    {
      get { return String.Format(@"Content/player.xml"); }
    }

    private string playerName = String.Empty;
    private XmlDocument doc; //будем тут хранить загруженную инфу, чтоб не создавать руками при сейве

    public Dictionary<BlockFactory.BlockType, float> blockChances;

    // chances for super block
    public float Mul2Chance { get; private set; }
    public float Mul3Chance { get; private set; }
    public float Mul5Chance { get; private set; }
    public float NeutralChance { get; private set; }

    public Scores PlayerScore { get; set; }
    public int PlayerLevel { get; set; }

    public Player(string name)
    {
      playerName = name;
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

    public void Load()
    {
      doc = new XmlDocument();
      if (File.Exists(PlayerFileName))
        doc.Load(PlayerFileName);
      else
        doc.Load(DefaultPlayerFileName);
        
      XmlNode playerNode = doc.SelectSingleNode(String.Format("/player"));
      foreach (XmlNode childNode in playerNode.ChildNodes)
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

    private void SetNodeValue(XmlNode node, object o)
    {
      node.InnerText = o.ToString();
    }

    public void Save()
    {
      XmlNode playerNode = doc.SelectSingleNode(String.Format("/player"));

      foreach (XmlNode childNode in playerNode.ChildNodes)
      {
        switch (childNode.Name)
        {
          case "mul2chance":
            {
              SetNodeValue(childNode, Mul2Chance);
              break;
            }
          case "mul3chance":
            {
              SetNodeValue(childNode, Mul3Chance);
              break;
            }
          case "mul5chance":
            {
              SetNodeValue(childNode, Mul5Chance);
              break;
            }
          case "neutralchance":
            {
              SetNodeValue(childNode, NeutralChance);
              break;
            }
          case "RedChance":
            {
              SetNodeValue(childNode, blockChances[BlockFactory.BlockType.Red]);
              break;
            }
          case "BlueChance":
            {
              SetNodeValue(childNode, blockChances[BlockFactory.BlockType.Blue]);
              break;
            }
          case "GreenChance":
            {
              SetNodeValue(childNode, blockChances[BlockFactory.BlockType.Green]);
              break;
            }
          case "YellowChance":
            {
              SetNodeValue(childNode, blockChances[BlockFactory.BlockType.Yellow]);
              break;
            }
          case "WhiteChance":
            {
              SetNodeValue(childNode, blockChances[BlockFactory.BlockType.White]);
              break;
            }
          case "GrayChance":
            {
              SetNodeValue(childNode, blockChances[BlockFactory.BlockType.Gray]);
              break;
            }

          case "BlueScore":
            {
              SetNodeValue(childNode, PlayerScore.BlueScore);
              break;
            }
          case "RedScore":
            {
              SetNodeValue(childNode, PlayerScore.RedScore);
              break;
            }
          case "GreenScore":
            {
              SetNodeValue(childNode, PlayerScore.GreenScore);
              break;
            }
          case "YellowScore":
            {
              SetNodeValue(childNode, PlayerScore.YellowScore);
              break;
            }
          case "WhiteScore":
            {
              SetNodeValue(childNode, PlayerScore.WhiteScore);
              break;
            }
          case "GrayScore":
            {
              SetNodeValue(childNode, PlayerScore.GrayScore);
              break;
            }
          case "AllScore":
            {
              SetNodeValue(childNode, PlayerScore.OverallScore);
              break;
            }

          case "Level":
            {
              SetNodeValue(childNode, PlayerLevel);
              break;
            }

          default:
            break;
        }
      }
      doc.Save(PlayerFileName);
    }
  }
}
