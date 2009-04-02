using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockFactory
	{
		private Texture2D greenTexture, redTexture, blueTexture, yellowTexture, blackTexture, someTexture;
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

				// Load block textures
				greenTexture = game.Content.Load<Texture2D>("GreenBlock");
				redTexture = game.Content.Load<Texture2D>("RedBlock");
				blueTexture = game.Content.Load<Texture2D>("BlueBlock");
				yellowTexture = game.Content.Load<Texture2D>("YellowBlock");
				blackTexture = game.Content.Load<Texture2D>("BlackBlock");
				someTexture = game.Content.Load<Texture2D>("SomeBlock");

				// Create block sprites
				greenBlock = new SpriteHelper(greenTexture, null);
				redBlock = new SpriteHelper(redTexture, null);
				blueBlock = new SpriteHelper(blueTexture, null);
				yellowBlock = new SpriteHelper(yellowTexture, null);
				blackBlock = new SpriteHelper(blackTexture, null);
				someBlock = new SpriteHelper(someTexture, null);
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
			switch(blockType)
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

		internal static BlockType GetNewBlockType()
		{
			return (BlockType)RandomHelper.GetRandomInt(EnumHelper.GetSize(typeof(BlockType)));
		}
	}
}
