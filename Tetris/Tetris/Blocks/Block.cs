using System;
using Microsoft.Xna.Framework;
using XnaTetris.Game;
using XnaTetris.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTetris.Blocks
{
  abstract class Block : DrawableGameComponent
  {
    #region Переменные

    private readonly SpriteHelper block;
    private BlockAnimator blockAnimator;

    private bool isMoving;

    public event EventHandler StartMove;
    public event EventHandler EndMove;
    public GameTime blockGameTime;

    #endregion

    #region Конструктор

    protected Block(Microsoft.Xna.Framework.Game setGame, Rectangle setBlockRect, SpriteHelper setBlock, int x, int y)
      : base(setGame)
    {
      BlockRectangle = setBlockRect;
      block = setBlock;
      X = x;
      Y = y;
    }

    #endregion

    #region Свойства
    public abstract BlockFactory.BlockType Type { get; }

    public bool IsClicked { get; set; }
    public Rectangle BlockRectangle { get; set; }
    public bool IsDestroyed { get; set; }
    public Serv.MoveDirection CurrentDir { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    #endregion

    public override void Draw(GameTime gameTime)
    {
      Color color = IsClicked ? Color.LightGray : Color.White;

      block.Render(BlockRectangle, color);

      base.Draw(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      blockGameTime = gameTime;

      if (isMoving)
      {
        BlockRectangle = blockAnimator.CurrentRectangle(gameTime);
        if (blockAnimator.IsMoveEnded(gameTime))
        {
          isMoving = false;
          EndMove(this, EventArgs.Empty);
        }
      }
    }

    public bool PointInBlock(Point point)
    {
      bool result;
      BlockRectangle.Contains(ref point, out result);
      return result;
    }

    public void ClickToBlock(GameTime gameTime)
    {
      IsClicked = !IsClicked;
    }

    public void MakeMove(GameTime gameTime, Rectangle destRectangle, int destX, int destY)
    {
      int movePathLength = Math.Max(Math.Abs(X - destX), Math.Abs(Y - destY));

      X = destX;
      Y = destY;
      isMoving = true;
      blockAnimator = new BlockAnimator(BlockRectangle, destRectangle, gameTime, movePathLength * BlockAnimator.DEFAULT_MOVE_TIME);
      StartMove(this, EventArgs.Empty);
    }

    public abstract int GetScore(int N);
  }
}
