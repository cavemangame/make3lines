using System;
using Microsoft.Xna.Framework;
using XnaTetris.Game;
using XnaTetris.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTetris.Blocks
{
  abstract class Block : DrawableGameComponent
  {
    #region ����������

    private readonly SpriteHelper block;
    private BlockAnimator blockAnimator;

    private bool isMoving;

    public event EventHandler EndMove;
    public GameTime blockGameTime;

    #endregion

    #region �����������

    public Block(LinesGame setGame, Rectangle setBlockRect, SpriteHelper setBlock, int x, int y)
      : base(setGame)
    {
      BlockRectangle = setBlockRect;
      block = setBlock;
      X = x;
      Y = y;
    }

    #endregion

    #region ��������
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
        if (blockAnimator.IsMoveEnded(gameTime))
        {
          isMoving = false;
          EndMove(this, EventArgs.Empty);
        }
        else
        {
          BlockRectangle = blockAnimator.CurrentRectangle(gameTime);
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

    public void MakeMove(GameTime gameTime, Rectangle newRectangle)
    {
      isMoving = true;
      blockAnimator = new BlockAnimator(BlockRectangle, newRectangle, gameTime);
    }

    public void MakeMove(GameTime gameTime, Rectangle newRect, int setx, int sety)
    {
      X = setx;
      Y = sety;
      MakeMove(gameTime, newRect);
    }

    public abstract int GetScore(int N);
  }
}
