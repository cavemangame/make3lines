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
    #region  онстанты

    public const int GridWidth = 8;
    public const int GridHeight = 8;

    public static readonly int NumOfBlockTypes = EnumHelper.GetSize(typeof(BlockFactory.BlockType));

    #endregion

    #region Variables

    public readonly LinesGame game;
    private readonly Block[,] grid = new Block[GridWidth, GridHeight];
    private readonly FindLines finder;

    private int _x1, _y1, _x2, _y2; //используютс€, чтобы отметить свапающиес€ блоки

    #endregion

    #region Constructor

    public BlocksGrid(LinesGame setGame, Rectangle setGridRect)
      : base(setGame)
    {
      game = setGame;
      GridRect = setGridRect;
      finder = new FindLines(this, GridWidth, GridHeight);
    }

    #endregion

    #region Properties

    public Block[,] Grid
    {
      get { return grid; }
    }

    public Rectangle GridRect { get; set; }

    #endregion

    #region Initialize

    public override void Initialize()
    {
      Restart();

      base.Initialize();
    } // Initialize()

    #endregion

    #region Restart
    public void Restart()
    {
      for (int x = 0; x < GridWidth; x++)
      {
        for (int y = 0; y < GridHeight; y++)
        {
          // repeat until the board is in stable state
          while (true)
          {
            AddNewRandomBlock(x, y);

            Block curBlock = Grid[x, y];

            if ((x < 2 || !curBlock.Type.Equals(Grid[x - 1, y].Type) || !curBlock.Type.Equals(Grid[x - 2, y].Type)) &&
                (y < 2 || !curBlock.Type.Equals(Grid[x, y - 1].Type) || !curBlock.Type.Equals(Grid[x, y - 2].Type)))
            {
              break;
            }
          }
        }
      }
      //Sound.Play(Sound.Sounds.Fight);
    } // Restart()
    #endregion

    #region Update
    public override void Update(GameTime gameTime)
    {
      if (!game.IsMoving && game.GameState == Serv.GameState.GameStateRunning)
      {
        if (Input.MouseLeftButtonJustPressed)
        {
          UpdateClickedBlock(Input.MousePos, gameTime);
        }
      }
      foreach (Block block in Grid)
      {
        block.Update(gameTime);
      }
    } // Update()

    private void UpdateClickedBlock(Point point, GameTime gameTime)
    {
      foreach (Block block in Grid)
      {
        if (block.PointInBlock(Serv.CorrectPositionWithGameScale(point)))
        {
          int count = finder.ClickedBlocksCount();
          if (count == 0)
            block.ClickToBlock(gameTime); //TODO
          else if (count == 1)
          {
            int xN, yN;
            if (finder.NeighbourClickedBlock(block.X, block.Y, out xN, out yN))
            {
              block.ClickToBlock(gameTime);
              _x1 = block.X;
              _x2 = xN;
              _y1 = block.Y;
              _y2 = yN;
              MoveBlocks(_x1, _y1, _x2, _y2, gameTime);
            }
            else
              CleanClickedStates();
          }
          //else throw new Exception();
        }
      }
    }//UpdateClickedBlock(Point, GameTime)

    #endregion

    #region render
    public override void Draw(GameTime gameTime)
    {
      for (int x = 0; x < GridWidth; x++)
        for (int y = 0; y < GridHeight; y++)
        {
          Grid[x, y].Draw(gameTime);
        }
    }
    #endregion

    #region functions

    public void EndSwappingBlocks(GameTime gameTime)
    {
      finder.SwapBlocks(_x1, _y1, _x2, _y2);
      finder.FindAndDestroyLines(gameTime);
      CleanClickedStates();
    }

    private void MoveBlocks(int x, int y, int xN, int yN, GameTime gameTime)
    {
      Grid[x, y].MakeMove(gameTime, finder.GetRectangle(xN, yN), xN, yN, 0);
      Grid[xN, yN].MakeMove(gameTime, finder.GetRectangle(x, y), x, y, 0);
    }

    private void CleanClickedStates()
    {
      for (int x = 0; x < GridWidth; x++)
        for (int y = 0; y < GridHeight; y++)
          Grid[x, y].IsClicked = false;
    }//CleanClickedStates()

    public void AddNewRandomBlock(int x, int y)
    {
      Grid[x, y] = (BlockFactory.GetBlockFactory(game).GetNewBlock(BlockFactory.GetRandomBlockType(),
        finder.GetRectangle(x, y), x, y));
      Grid[x, y].EndMove += BlocksGrid_EndMove;
    }

    public void AddNewRandomBlock(int x, int y, BlockFactory.BlockType oldType, int luck)
    {
      Grid[x, y] = (BlockFactory.GetBlockFactory(game).GetNewBlock(
        RandomBlockHelper.GenerateNewBlockType(oldType, luck),
        finder.GetRectangle(x, y), x, y));
      Grid[x, y].EndMove += BlocksGrid_EndMove;
    }

    // если просто закончили движение - сдвигаем блок, если свапались - то вызываем EndSwappingBlocks,
    // второй свапающийс€ блок не вызовет EndSwappingBlocks, так как в первом все Clicked подчист€тс€
    void BlocksGrid_EndMove(object sender, EventArgs e)
    {
      if (!(sender is Block))
        return;
      Block temp = sender as Block;
      if (temp.IsClicked)
        EndSwappingBlocks(temp.blockGameTime);
      else
      {
        temp.BlockRectangle = finder.GetRectangle(temp.X, temp.Y);
        Grid[temp.X, temp.Y] = temp;
      }
    }

    #endregion
  }
}
