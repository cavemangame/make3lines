using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
  class BlockFactory
  {
    private SpriteHelper greenBlock, redBlock, blueBlock, yellowBlock, blackBlock, someBlock;
    private readonly LinesGame game;

    private static BlockFactory instance;

    public enum BlockType
    {
      Green,
      Red,
      Blue,
      Yellow,
      Black,
      Someone
    }

    private BlockFactory(LinesGame setGame)
    {
      game = setGame;
      LoadTexturesAndSprites();
    }

    private void LoadTexturesAndSprites()
    {
      if (game != null)
      {
        game.Content.RootDirectory = "Content";

        // Create block sprites
        greenBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(0, 0, 64, 64));
        redBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64, 0, 64, 64));
        blueBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64*2, 0, 64, 64));
        yellowBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64*3, 0, 64, 64));
        blackBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64*4, 0, 64, 64));
        someBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64*5, 0, 64, 64));
      }
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
          return new BlockGreen(game, setRect, greenBlock, x, y);
        case BlockType.Red:
          return new BlockRed(game, setRect, redBlock, x, y);
        case BlockType.Blue:
          return new BlockBlue(game, setRect, blueBlock, x, y);
        case BlockType.Yellow:
          return new BlockYellow(game, setRect, yellowBlock, x, y);
        case BlockType.Black:
          return new BlockBlack(game, setRect, blackBlock, x, y);
        case BlockType.Someone:
          return new BlockSomeone(game, setRect, someBlock, x, y);
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
