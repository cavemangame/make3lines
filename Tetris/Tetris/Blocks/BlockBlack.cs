using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockBlack : Block
	{
		public BlockBlack(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, SpriteHelper setHiBlock, int x, int y)
			: base(setGame, setBlockRect, setBlock, setHiBlock, x, y)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Black; }
		}

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 55;
				case 4:
					return 145;
				case 5:
					return 245;
				default:
					return 0;
			}
		}
	}
}
