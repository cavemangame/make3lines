using System;
using Microsoft.Xna.Framework;
using XnaTetris.Blocks;
using XnaTetris.Game;
using XnaTetris.Helpers;
using XnaTetris.Algorithms;

namespace XnaTetris
{
  class BlocksGrid : DrawableGameComponent
  {
    #region Constants
    public const int GRID_WIDTH = 8;
    public const int GRID_HEIGHT = 8;
    #endregion

    #region Variables
    private readonly BlocksGridHelper finder;
    private int _x1, _y1, _x2, _y2;

    /// <summary>
    /// how much blocks are moving now
    /// </summary>
    private int activeBlocks;

    /// <summary>
    /// true if current movement is swap (user action)
    /// </summary>
    private bool isSwap;

    /// <summary>
    /// true if current movement is undo after unsuccessful movement
    /// </summary>
    private bool isUndo;
    #endregion

    #region Properties
    public Block[,] Grid { get; private set; }
    public Rectangle GridRectangle { get; private set; }
    public int BlockWidth { get; private set; }
    public int BlockHeight { get; private set; }
    public LinesGame LinesGame { get { return Game as LinesGame; }}
    #endregion

    #region Constructor

    public BlocksGrid(Microsoft.Xna.Framework.Game game, Rectangle GridRectangle)
      : base(game)
    {
      this.GridRectangle = GridRectangle;
      BlockWidth = this.GridRectangle.Width / GRID_WIDTH;
      BlockHeight = this.GridRectangle.Height / GRID_HEIGHT;
      Grid = new Block[GRID_WIDTH, GRID_HEIGHT];
      finder = new BlocksGridHelper(this);
    }
    #endregion

    #region Initialize
    public override void Initialize()
    {
      Restart();

      base.Initialize();
    }
    #endregion

    #region Restart
    public void Restart()
    {
      for (int x = 0; x < GRID_WIDTH; x++)
      {
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
          // repeat until the board is in stable state
          while (true)
          {
            AddNewRandomBlock(x, y);

            Block curBlock = Grid[x, y];

            // TODO: use finder
            if ((x < 2 || !curBlock.Type.Equals(Grid[x - 1, y].Type) || !curBlock.Type.Equals(Grid[x - 2, y].Type)) &&
                (y < 2 || !curBlock.Type.Equals(Grid[x, y - 1].Type) || !curBlock.Type.Equals(Grid[x, y - 2].Type)))
            {
              break;
            }
          }
        }
      }
    }
    #endregion

    #region Update
    public override void Update(GameTime gameTime)
    {
      if (LinesGame.GameState == Serv.GameState.GameStateRunning && Input.MouseLeftButtonJustPressed)
      {
        UpdateClickedBlock(Input.MousePos, gameTime);
      }
      foreach (Block block in Grid)
      {
        block.Update(gameTime);
      }
    }

    private void UpdateClickedBlock(Point point, GameTime gameTime)
    {
      // don't allow to do anything until the board is in stable state
      if (activeBlocks != 0)
      {
        return;
      }

      foreach (Block block in Grid)
      {
        if (block.PointInBlock(Serv.CorrectPositionWithGameScale(point)))
        {
          int count = finder.ClickedBlocksCount();

          if (count == 0)
          {
            block.ClickToBlock(gameTime);
          }
          else if (count == 1)
          {
            int xN, yN;

            if (finder.NeighbourClickedBlock(block.X, block.Y, out xN, out yN))
            {
              isSwap = true;
              block.ClickToBlock(gameTime);
              _x1 = block.X;
              _x2 = xN;
              _y1 = block.Y;
              _y2 = yN;
              SwapBlocks(_x1, _y1, _x2, _y2, gameTime);
            }
            else
            {
              CleanClickedStates();
            }
          }
          else
          {
            throw new Exception("ClickedBlocksCount() should not return value greater then 1");
          }

          break;
        }
      }
    }

    #endregion

    #region render
    public override void Draw(GameTime gameTime)
    {
      for (int x = 0; x < GRID_WIDTH; x++)
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
          Grid[x, y].Draw(gameTime);
        }
    }
    #endregion

    #region functions
    private void SwapBlocks(int x1, int y1, int x2, int y2, GameTime gameTime)
    {
      Grid[x1, y1].MakeMove(gameTime, finder.GetRectangle(x2, y2), x2, y2);
      Grid[x2, y2].MakeMove(gameTime, finder.GetRectangle(x1, y1), x1, y1);

      Block temp = Grid[x1, y1];

      Grid[x1, y1] = Grid[x2, y2];
      Grid[x2, y2] = temp;

      Grid[x1, y1].IsClicked = false;
      Grid[x2, y2].IsClicked = false;
    }

    private void CleanClickedStates()
    {
      for (int x = 0; x < GRID_WIDTH; x++)
      {
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
          Grid[x, y].IsClicked = false;
        }
      }
    }

    public void AddNewRandomBlock(int x, int y)
    {
      Grid[x, y] = (BlockFactory.GetBlockFactory(LinesGame).GetNewBlock(BlockFactory.GetRandomBlockType(),
                                                                        finder.GetRectangle(x, y), x, y));
      Grid[x, y].StartMove += BlocksGrid_StartMove;
      Grid[x, y].EndMove += BlocksGrid_EndMove;
    }

    public void AddNewRandomBlock(int x, int y, BlockFactory.BlockType oldType, int luck)
    {
      Grid[x, y] = (BlockFactory.GetBlockFactory(LinesGame).GetNewBlock(
        RandomBlockHelper.GenerateNewBlockType(oldType, luck),
        finder.GetRectangle(x, y), x, y));
      Grid[x, y].StartMove += BlocksGrid_StartMove;
      Grid[x, y].EndMove += BlocksGrid_EndMove;
    }

    void BlocksGrid_StartMove(object sender, EventArgs e)
    {
      if (!(sender is Block))
      {
        return;
      }

      activeBlocks += 1;
    }

    void BlocksGrid_EndMove(object sender, EventArgs e)
    {
      if (!(sender is Block))
      {
        return;
      }

      activeBlocks -= 1;
      if (activeBlocks != 0)
      {
        return;
      }

      if (isUndo)
      {
        isUndo = false;
        return;
      }

      Block block = sender as Block;
      GameTime gameTime = block.blockGameTime;
      bool successfulMovement = finder.FindAndDestroyLines(gameTime);

      if (isSwap && !successfulMovement)
      {
        // undo the movement
        isUndo = true;
        LinesGame.Timer -= 5000;
        SwapBlocks(_x1, _y1, _x2, _y2, gameTime);
      }
      isSwap = false;
    }

    public void EnableComponents(bool isEnable)
    {
      Enabled = isEnable;
      for (int x = 0; x < GRID_WIDTH; x++)
        for (int y = 0; y < GRID_HEIGHT; y++)
          Grid[x, y].Enabled = isEnable;
    }
    #endregion
  }
}
