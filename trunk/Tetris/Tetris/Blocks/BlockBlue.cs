using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Blocks
{
	class BlockBlue : Block
	{
		public BlockBlue(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, SpriteHelper setHiBlock, int x, int y, int multiplier)
			: base(setGame, setBlockRect, setBlock, setHiBlock, x, y, multiplier)
		{
		}

		public override BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Blue; }
		}

    public override Color ScoreColor
    {
      get { return Color.DarkBlue; }
    }

		public override int GetScore(int N)
		{
			switch (N)
			{
				case 3:
					return 40;
				case 4:
					return 110;
				case 5:
					return 210;
				default:
					return 0;
			}
		}

	  public override void AddScore(int N)
	  {
	    LinesGame.Score.BlueScore += N;
	  }
	}
}
