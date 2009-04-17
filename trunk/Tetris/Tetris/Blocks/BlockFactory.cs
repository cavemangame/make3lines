using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
  public class BlockFactory
  {
    private readonly LinesGame game;

    private static BlockFactory instance;

    public enum BlockType
    {
      Green,
      Red,
      Blue,
      Yellow,
      White,
      Gray
    }

    private BlockFactory(LinesGame setGame)
    {
      game = setGame;
    }

    public static BlockFactory GetBlockFactory(LinesGame setGame)
    {
      if (instance == null)
      {
        instance = new BlockFactory(setGame);
      }
      return instance;
    }

    public Block GetNewBlock(BlockType blockType, Rectangle setRect, int x, int y)
    {
      switch (blockType)
      {
        case BlockType.Green:
          return new BlockGreen(game, setRect, ContentSpace.greenBlock, ContentSpace.greenHiBlock, x, y);
        case BlockType.Red:
          return new BlockRed(game, setRect, ContentSpace.redBlock, ContentSpace.redHiBlock, x, y);
        case BlockType.Blue:
          return new BlockBlue(game, setRect, ContentSpace.blueBlock, ContentSpace.blueHiBlock, x, y);
        case BlockType.Yellow:
          return new BlockYellow(game, setRect, ContentSpace.yellowBlock, ContentSpace.yellowHiBlock, x, y);
        case BlockType.White:
          return new BlockWhite(game, setRect, ContentSpace.blackBlock, ContentSpace.blackHiBlock, x, y);
        case BlockType.Gray:
          return new BlockGray(game, setRect, ContentSpace.someBlock, ContentSpace.someHiBlock, x, y);
        default:
          throw new ArgumentOutOfRangeException("blockType");
      }
    }

    internal static BlockType GetRandomBlockType()
    {
      return (BlockType)RandomHelper.GetRandomInt(EnumHelper.GetSize(typeof(BlockType)));
    }
  }
}
