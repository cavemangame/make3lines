using System.Collections.Generic;
using XnaTetris.Blocks;

namespace XnaTetris.Game
{
  public class Player
  {
    public Dictionary<BlockFactory.BlockType, float> blockChances;

    // chances for super block
    public float Mul2Chance { get; private set; }
    public float Mul3Chance { get; private set; }
    public float Mul5Chance { get; private set; }
    public float NeutralChance { get; private set; }

    public Player()
    {
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
      
    }

    private void Save()
    {
      
    }
  }
}
