using Microsoft.Xna.Framework;
using XnaTetris.Blocks;
using XnaTetris.Game;

namespace XnaTetris.Algorithms
{
  class BlocksGridHelper
  {
    #region Variables
    private readonly BlocksGrid blocksGrid;
    #endregion

    #region Constructor
    public BlocksGridHelper(BlocksGrid blocksGrid)
    {
      this.blocksGrid = blocksGrid;
    }
    #endregion

    #region Finder functions
    private int SameTypeLeftBlocksCount(int x, int y)
    {
      Block block = blocksGrid.Grid[x, y];
      int xIndex = x - 1;
      int result = 0;

      while (xIndex >= 0 && block.Type.Equals(blocksGrid.Grid[xIndex, y].Type))
      {
        ++ result;
        -- xIndex;
      }
      return result;
    }

    private int SameTypeUpBlocksCount(int x, int y)
    {
      Block block = blocksGrid.Grid[x, y];
      int yIndex = y - 1;
      int result = 0;

      while (yIndex >= 0 && block.Type.Equals(blocksGrid.Grid[x, yIndex].Type))
      {
        ++ result;
        -- yIndex;
      }
      return result;
    }

    /// <summary>
    /// Tries to find any clicked block near current block
    /// </summary>
    /// <param name="x">current block x</param>
    /// <param name="y">current block y</param>
    /// <param name="xN">result block x</param>
    /// <param name="yN">result block y</param>
    /// <returns>true if there is such block</returns>
    public bool NeighbourClickedBlock(int x, int y, out int xN, out int yN)
    {
      if (x > 0 && blocksGrid.Grid[x - 1, y].IsClicked)
      {
        xN = x - 1;
        yN = y;
        return true;
      }
      if (x < BlocksGrid.GRID_WIDTH - 1 && blocksGrid.Grid[x + 1, y].IsClicked)
      {
        xN = x + 1;
        yN = y;
        return true;
      }
      if (y > 0 && blocksGrid.Grid[x, y - 1].IsClicked)
      {
        xN = x;
        yN = y - 1;
        return true;
      }
      if (y < BlocksGrid.GRID_HEIGHT - 1 && blocksGrid.Grid[x, y + 1].IsClicked)
      {
        xN = x;
        yN = y + 1;
        return true;
      }
      xN = yN = -1;
      return false;
    }

    /// <summary>
    /// Finds lines and mark lines' blocks as Destroyed
    /// </summary>
    /// <returns>true if any line was found</returns>
    public bool FindLines()
    {
      bool result = false;

      // for each block try to find line where this block is rightmost or bottommost
      for (int y = 0; y < BlocksGrid.GRID_HEIGHT; ++y)
      {
        for (int x = 0; x < BlocksGrid.GRID_WIDTH; ++x)
        {
          Block block = blocksGrid.Grid[x, y];
          int blocksForScoreCount = 0;

          // can this block be rightmost?
          if (x == BlocksGrid.GRID_WIDTH - 1 || !block.Type.Equals(blocksGrid.Grid[x + 1, y].Type))
          {
            int sameTypeLeftBlocksCount = SameTypeLeftBlocksCount(x, y);

            if (sameTypeLeftBlocksCount >= 2)
            {
              result = true;
              blocksForScoreCount += sameTypeLeftBlocksCount + 1;
              for (int i = 0; i <= sameTypeLeftBlocksCount; ++ i)
              {
                blocksGrid.Grid[x - i, y].IsDestroyed = true;
              }
            }
          }

          // can this block be bottommost?
          if (y == BlocksGrid.GRID_HEIGHT - 1 || !block.Type.Equals(blocksGrid.Grid[x, y + 1].Type))
          {
            int sameTypeUpBlocksCount = SameTypeUpBlocksCount(x, y);

            if (sameTypeUpBlocksCount >= 2)
            {
              result = true;
              blocksForScoreCount += sameTypeUpBlocksCount + 1;
              for (int i = 0; i <= sameTypeUpBlocksCount; ++ i)
              {
                blocksGrid.Grid[x, y - i].IsDestroyed = true;
              }
            }
          }

          blocksGrid.LinesGame.Score += blocksGrid.Grid[x, y].GetScore(blocksForScoreCount);
        }
      }
      return result;
    }
    #endregion

    #region Destroy Blocks
    /// <summary>
    /// Remove blocks marked as Destroyed with FindLines()
    /// </summary>
    /// <param name="gameTime"></param>
    public void RemoveLines(GameTime gameTime)
    {
      for (int x = 0; x < BlocksGrid.GRID_WIDTH; ++ x)
      {
        int bottommestDestroyedY = BlocksGrid.GRID_HEIGHT;

        while (-- bottommestDestroyedY >= 0 && !blocksGrid.Grid[x, bottommestDestroyedY].IsDestroyed) {}
        if (bottommestDestroyedY == -1)
        {
          continue;
        }

        int destroyedBlocksCount = 0;

        for (int y = bottommestDestroyedY; y >= 0; -- y)
        {
          Block block = blocksGrid.Grid[x, y];

          if (block.IsDestroyed)
          {
            block.X = -1;
            block.Y = -1;
            block.BlockRectangle = Serv.EmptyRect;
            ++ destroyedBlocksCount;
          }
          else
          {
            blocksGrid.Grid[x, bottommestDestroyedY] = blocksGrid.Grid[x, y];
            blocksGrid.Grid[x, y].MakeMove(gameTime, GetRectangle(x, bottommestDestroyedY), x, bottommestDestroyedY);
            -- bottommestDestroyedY;
          }
        }

        for (int y = destroyedBlocksCount - 1; y >= 0; -- y)
        {
          // TODO: add luck?
          Block block = blocksGrid.GetNewRandomBlock(x, y - destroyedBlocksCount);

          blocksGrid.Grid[x, y] = block;
          block.MakeMove(gameTime, GetRectangle(x, y), x, y);
        }

      }
    }
    #endregion

    /// <summary>
    /// Returns Rectangle instance for given block's coordinates
    /// </summary>
    /// <param name="x">block x coordinate</param>
    /// <param name="y">block y coordinate</param>
    /// <returns>Rectangle instance for given block's coordinates</returns>
    public Rectangle GetRectangle(int x, int y)
    {
      return new Rectangle(blocksGrid.GridRectangle.X + x * blocksGrid.BlockWidth,
                           blocksGrid.GridRectangle.Y + y * blocksGrid.BlockHeight,
                           blocksGrid.BlockWidth, blocksGrid.BlockHeight);
    }

    /// <summary>
    /// Calculates how many blocks are in clicked state on the board now
    /// </summary>
    /// <returns>amount of clicked blocks</returns>
    public int ClickedBlocksCount()
    {
      int count = 0;

      for (int x = 0; x < BlocksGrid.GRID_WIDTH; x++)
      {
        for (int y = 0; y < BlocksGrid.GRID_HEIGHT; y++)
        {
          if (blocksGrid.Grid[x, y].IsClicked)
            count++;
        }
      }
      return count;
    }
  }
}
