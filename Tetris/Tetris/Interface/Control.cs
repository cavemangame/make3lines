using System;
using Microsoft.Xna.Framework;
using XnaTetris.Game;
using XnaTetris.Helpers;

namespace XnaTetris.Interface
{
  public class LinesControl : DrawableGameComponent
  {
    #region Properties

    public Rectangle BoundingRect { get; set; }
    public LinesGame LinesGame { get { return Game as LinesGame; } }

    #endregion

    #region Constructors

    public LinesControl(Microsoft.Xna.Framework.Game game) : base(game)
    {
    }

    public LinesControl(Microsoft.Xna.Framework.Game game, Rectangle rect) :this (game)
    {
      BoundingRect = rect;
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
    }

    #endregion

    #region Update

    public override void Update(GameTime gameTime)
    {
      if (Input.MouseLeftButtonJustPressed)
      {
        if (HitTest(Input.MousePos))
          HandleMouseClick();
        else 
          HandleOutMouseClick();
      }
      else
      {
        HandleMouseOver(HitTest(Input.MousePos));
      }

      base.Update(gameTime);
    }
    #endregion

    public bool HitTest(Point p)
    {
      bool result;
      BoundingRect.Contains(ref p, out result);
      return result;
    }

    protected virtual void HandleOutMouseClick()
    {
    }

    protected virtual void HandleMouseOver(bool isOver)
    {
      
    }

    protected virtual void HandleMouseClick()
    {
     // ButtonAction(this, EventArgs.Empty);
    }

    public void Show()
    {
      Visible = true;
      Enabled = true;
    }

    public void Hide()
    {
      Visible = false;
      Enabled = false;
    }
  }
}
