using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using XnaTetris.Game;
using XnaTetris.Sounds;
using XnaTetris.Graphics;
using XnaTetris.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTetris.Blocks
{
	class Block : DrawableGameComponent
	{
		#region константы

		#endregion

		#region Переменные

		readonly LinesGame game;
		private Rectangle blockRect;
		private SpriteHelper block;
		private BlockAnimator blockAnimator;

		private Serv.MoveDirection currentDir;

		private bool isClicked;
		private bool isDestroyed;
		private bool isMoving;

		private float depth;

		private int xpos;
		private int ypos;

		public event EventHandler EndMove;
		public GameTime blockGameTime;

		#endregion

		#region Конструктор

		public Block(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, int x, int y)
			: base(setGame)
		{
			game = setGame;
			blockRect = setBlockRect;
			block = setBlock;
			xpos = x;
			ypos = y;
		}

		#endregion

		#region Свойства
		public virtual BlockFactory.BlockType Type
		{
			get { return BlockFactory.BlockType.Black;}
		}

		public bool IsClicked
		{
			get { return isClicked; }
			set { isClicked = value; }
		}

		public Rectangle BlockRectangle
		{
			get { return blockRect; }
			set { blockRect = value; }
		}

		public bool IsDestroyed
		{
			get { return isDestroyed; }
			set { isDestroyed = value; }
		}

		public Serv.MoveDirection CurrentDir
		{
			get { return currentDir; }
			set { currentDir = value; }
		}

		public int X
		{
			get { return xpos; }
			set { xpos = value; }
		}

		public int Y
		{
			get { return ypos; }
			set { ypos = value; }
		}

		#endregion

		public override void Draw(GameTime gameTime)
		{
			if (!IsClicked)
				block.Render(BlockRectangle);
			else 
				block.Render(BlockRectangle, Color.LightGray);

			base.Draw(gameTime);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			this.blockGameTime = gameTime;

			if (isMoving)
			{
				if (blockAnimator.IsMoveEnded(gameTime))
				{
					isMoving = false;
					EndMove(this, EventArgs.Empty);
				}
				else
				{
					blockRect = blockAnimator.CurrentRectangle(gameTime);
				}
			}
		}

		#region virtual methods

		public virtual void SetBlockSprite()
		{
			
		}

		public virtual int GetScore(int N)
		{
			return (N - 3)*5 + N*5;
		}

		public virtual bool PointInBlock(Point point)
		{
			bool result;
			BlockRectangle.Contains(ref point, out result);
			return result;
		}

		public virtual void ClickToBlock(GameTime gameTime)
		{
			IsClicked = !IsClicked;
		}
		
		public virtual void MakeMove(GameTime gameTime, Rectangle newRect, float setDepth)
		{
			isMoving = true;
			blockAnimator = new BlockAnimator(blockRect, newRect, gameTime);
			depth = setDepth;
		}

		public virtual void MakeMove(GameTime gameTime, Rectangle newRect, int setx, int sety, float setDepth)
		{
			xpos = setx;
			ypos = sety;
			MakeMove(gameTime, newRect, setDepth);
		}

		#endregion
	}
}