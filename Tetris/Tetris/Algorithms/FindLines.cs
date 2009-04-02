using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaTetris.Blocks;
using XnaTetris.Game;

namespace XnaTetris.Algorithms
{
	class FindLines
	{
		#region Variables

		private readonly int GridHeight;
		private readonly int GridWidth;
		private readonly BlocksGrid bGrid;

		#endregion

		#region Constructor
		
		public FindLines(BlocksGrid setGrid, int GridWidth, int GridHeight)
		{
			bGrid = setGrid;
			this.GridWidth = GridWidth;
			this.GridHeight = GridHeight;
		}

		#endregion

		#region Finder funcrions

		private bool IsSameTypeNHorizBlock(int y, int x, int n)
		{
			Type type = bGrid.Grid[x, y].GetType();
			for (int i = 1; i < n; i++)
			{
				if (bGrid.Grid[x + i, y].IsDestroyed)
					return false;
				if (type != bGrid.Grid[x + i, y].GetType())
					return false;
			}
			return true;
		}

		private bool IsSameTypeNVertBlock(int x, int y, int n)
		{
			Type type = bGrid.Grid[x, y].GetType();
			for (int i = 1; i < n; i++)
			{
				if (bGrid.Grid[x, y + i].IsDestroyed)
					return false;
				if (type != bGrid.Grid[x, y + i].GetType())
					return false;
			}
			return true;
		}

		public bool NeighbourClickedBlock(int xx, int yy, out int xN, out int yN)
		{
			if (xx > 0 && bGrid.Grid[xx - 1, yy].IsClicked)
			{
				xN = xx - 1;
				yN = yy;
				return true;
			}
			if (xx < GridWidth - 1 && bGrid.Grid[xx + 1, yy].IsClicked)
			{
				xN = xx + 1;
				yN = yy;
				return true;
			}
			if (yy > 0 && bGrid.Grid[xx, yy - 1].IsClicked)
			{
				xN = xx;
				yN = yy - 1;
				return true;
			}
			if (yy < GridHeight - 1 && bGrid.Grid[xx, yy + 1].IsClicked)
			{
				xN = xx;
				yN = yy + 1;
				return true;
			}
			xN = yN = -1;
			return false;
		}//NeighbourClickedBlock(int,int)

		private bool FindNLines(int N)
		{
			bool result = false;
			// сначала по горизонтали
			for (int y = 0; y < GridHeight; y++)
			{
				for (int x = 0; x < GridWidth - (N-1); x++)
				{
					if (IsSameTypeNHorizBlock(y, x, N))
					{
						result = true;
						for (int xx = x; xx < x + N; xx++)
							bGrid.Grid[xx, y].IsDestroyed = true;

						bGrid.game.Score += bGrid.Grid[x, y].GetScore(N);
					}
				}
			}

			// по вертикали
			for (int x = 0; x < GridWidth; x++)
			{
				for (int y = 0; y < GridHeight - (N - 1); y++)
				{
					if (IsSameTypeNVertBlock(x, y, N))
					{
						result = true;
						for (int yy = y; yy < y + N; yy++)
							bGrid.Grid[x, yy].IsDestroyed = true;
						bGrid.game.Score += bGrid.Grid[x, y].GetScore(N);
					}
				}
			}
			return result;
		}

		#endregion

		#region Destroy Blocks

		public void FindAndDestroyLines(GameTime gameTime)
		{
			bool result = FindNLines(5) | FindNLines(4) | FindNLines(3);

			RemoveLines(gameTime);
			if (result)
				FindAndDestroyLines(gameTime);
		}

		private void RemoveLines(GameTime gameTime)
		{
			for (int j = GridHeight - 1; j >= 0; j--)
				RemoveHorizLine(gameTime, j);
		}

		private void RemoveHorizLine(GameTime gameTime, int j)
		{
			// подчищаем удаленные блоки с одновременным спуском столбцов, до тех пор, пока на линии останутся только
			// неудаленные
			bool flag = false;
			for (int i = 0; i < GridWidth; i++)
			{
				if (bGrid.Grid[i, j].IsDestroyed)
				{
					DownBlockColumn(gameTime, i, j);
					flag = true;
				}
			}
			if (flag) RemoveHorizLine(gameTime, j);
		}

		private void DownBlockColumn(GameTime gameTime, int i, int jj)
		{
			if (jj > 0)
			{
				bGrid.Grid[i, jj].X = bGrid.Grid[i, jj].Y = -1;
				bGrid.Grid[i, jj].BlockRectangle = Serv.EmptyRect;

				for (int j = jj - 1; j >= 0; j--)
				{
					bGrid.Grid[i, j + 1] = bGrid.Grid[i, j];
					bGrid.Grid[i, j].MakeMove(gameTime, GetRectangle(i, j + 1), i, j + 1, 0);
				}
			}
			// наверху сразу генерим новый
			bGrid.AddNewRandomBlock(i, 0, bGrid.Grid[i, jj].Type, 2);
		}

		public void SwapBlocks(int x1, int y1, int x2, int y2)
		{
			Block temp = bGrid.Grid[x1, y1];
			bGrid.Grid[x1, y1] = bGrid.Grid[x2, y2];
			bGrid.Grid[x2, y2] = temp;

			bGrid.Grid[x1, y1].BlockRectangle = GetRectangle(x1, y1);
			bGrid.Grid[x2, y2].BlockRectangle = GetRectangle(x2, y2);

			bGrid.Grid[x1, y1].IsClicked = bGrid.Grid[x2, y2].IsClicked = false;
		}//LazySwapBlocks(Block, Block)

		#endregion

		#region Some functions

		public Rectangle GetRectangle(int x, int y)
		{
			return new Rectangle(bGrid.GridRect.X + x * bGrid.GridRect.Width / GridWidth,
													 bGrid.GridRect.Y + y * bGrid.GridRect.Height / GridHeight,
													 bGrid.GridRect.Width / GridWidth,
													 bGrid.GridRect.Height / GridHeight);
		}//GetRectangle(int, int)

		public int ClickedBlocksCount()
		{
			int count = 0;
			for (int x = 0; x < GridWidth; x++)
				for (int y = 0; y < GridHeight; y++)
				{
					if (bGrid.Grid[x, y].IsClicked)
						count++;
				}
			return count;
		}//ClickedBlocksCount()

		#endregion
	}
}
