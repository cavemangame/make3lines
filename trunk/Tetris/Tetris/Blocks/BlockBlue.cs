using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockBlue : Block
	{
		public BlockBlue(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, int x, int y)
			: base(setGame, setBlockRect, setBlock, x, y)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Blue; }
		}

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 30;
				case 4:
					return 100;
				case 5:
					return 260;
				default:
					return 0;
			}
		}
	}
}
