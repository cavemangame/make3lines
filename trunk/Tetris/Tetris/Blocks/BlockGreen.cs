using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockGreen : Block
	{
		public BlockGreen(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, SpriteHelper setHiBlock, int x, int y)
			: base(setGame, setBlockRect, setBlock, setHiBlock, x, y)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Green; }
		}

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 40;
				case 4:
					return 150;
				case 5:
					return 230;
				default:
					return 0;
			}
		}
	}
}
