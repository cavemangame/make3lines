using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockRed : Block
	{
		public BlockRed(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, int x, int y)
			: base(setGame, setBlockRect, setBlock, x, y)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Red; }
		}

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 50;
				case 4:
					return 120;
				case 5:
					return 200;
				default:
					return 0;
			}
		}
	}
}
