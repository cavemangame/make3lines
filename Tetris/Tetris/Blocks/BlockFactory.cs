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
        ContentSpace.greenBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(0, 0, 64, 64));
        ContentSpace.redBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64, 0, 64, 64));
        ContentSpace.blueBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 2, 0, 64, 64));
        ContentSpace.yellowBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 3, 0, 64, 64));
        ContentSpace.blackBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 4, 0, 64, 64));
        ContentSpace.someBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksGeneral"), new Rectangle(64 * 5, 0, 64, 64));

        ContentSpace.greenHiBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksHilight"), new Rectangle(0, 0, 64, 64));
        ContentSpace.redHiBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksHilight"), new Rectangle(64, 0, 64, 64));
        ContentSpace.blueHiBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 2, 0, 64, 64));
        ContentSpace.yellowHiBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 3, 0, 64, 64));
        ContentSpace.blackHiBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 4, 0, 64, 64));
        ContentSpace.someHiBlock = new SpriteHelper(game.Content.Load<Texture2D>("BlocksHilight"), new Rectangle(64 * 5, 0, 64, 64));

        ContentSpace.selectionBlock = new SpriteHelper(game.Content.Load<Texture2D>("Selection"), null);
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
          return new BlockGreen(game, setRect, ContentSpace.greenBlock, ContentSpace.greenHiBlock, x, y);
        case BlockType.Red:
          return new BlockRed(game, setRect, ContentSpace.redBlock, ContentSpace.redHiBlock, x, y);
        case BlockType.Blue:
          return new BlockBlue(game, setRect, ContentSpace.blueBlock, ContentSpace.blueHiBlock, x, y);
        case BlockType.Yellow:
          return new BlockYellow(game, setRect, ContentSpace.yellowBlock, ContentSpace.yellowHiBlock, x, y);
        case BlockType.Black:
          return new BlockBlack(game, setRect, ContentSpace.blackBlock, ContentSpace.blackHiBlock, x, y);
        case BlockType.Someone:
          return new BlockSomeone(game, setRect, ContentSpace.someBlock, ContentSpace.someHiBlock, x, y);
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
