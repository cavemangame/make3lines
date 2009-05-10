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
      Gray,
      Neutral,
      Invul
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

    public Block GetNewBlock(BlockType blockType, Rectangle setRect, int x, int y, int multiplier)
    {
      switch (blockType)
      {
        case BlockType.Green:
          return new BlockGreen(game, setRect, ContentSpace.GetSprite("GreenBlock"),
            ContentSpace.GetSprite("GreenHiBlock"), x, y, multiplier);
        case BlockType.Red:
          return new BlockRed(game, setRect, ContentSpace.GetSprite("RedBlock"),
            ContentSpace.GetSprite("RedHiBlock"), x, y, multiplier);
        case BlockType.Blue:
          return new BlockBlue(game, setRect, ContentSpace.GetSprite("BlueBlock"),
            ContentSpace.GetSprite("BlueHiBlock"), x, y, multiplier);
        case BlockType.Yellow:
          return new BlockYellow(game, setRect, ContentSpace.GetSprite("YellowBlock"),
            ContentSpace.GetSprite("YellowHiBlock"), x, y, multiplier);
        case BlockType.White:
          return new BlockWhite(game, setRect, ContentSpace.GetSprite("WhiteBlock"),
            ContentSpace.GetSprite("WhiteHiBlock"), x, y, multiplier);
        case BlockType.Gray:
          return new BlockGray(game, setRect, ContentSpace.GetSprite("GrayBlock"),
            ContentSpace.GetSprite("GrayHiBlock"), x, y, multiplier);
        case BlockType.Invul:
          return new BlockInvulnerable(game, setRect, ContentSpace.GetSprite("InvulBlock"),
            ContentSpace.GetSprite("InvulBlock"), x, y, 1);
        case BlockType.Neutral:
          return new BlockNeutral(game, setRect, ContentSpace.GetSprite("NeutralBlock"),
            ContentSpace.GetSprite("NeutralBlock"), x, y, 1);

        default:
          throw new ArgumentOutOfRangeException("blockType");
      }
    }

    internal BlockType GetRandomBlockType(ref int multiplier)
    {
      // без нейтральных
      return game.GameField.CurrentLevel.GetNewRandomBlock(ref multiplier);
    }
  }
}
