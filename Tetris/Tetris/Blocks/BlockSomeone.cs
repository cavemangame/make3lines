using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockSomeone : Block
	{
		public BlockSomeone(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, int x, int y)
			: base(setGame, setBlockRect, setBlock, x, y)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Someone; }
		}

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 50;
				case 4:
					return 105;
				case 5:
					return 210;
				default:
					return 0;
			}
		}
	}
}
