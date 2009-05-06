using Microsoft.Xna.Framework;
using XnaTetris.Game;

namespace XnaTetris.Interface
{
  class ListControls : GameScene
  {
    #region Variables

    private Rectangle BoundingRect;

    #endregion

    #region Constructor

    public ListControls(Microsoft.Xna.Framework.Game game) : base(game)
    {
      BoundingRect = Serv.EmptyRect;
    }

    public ListControls(Microsoft.Xna.Framework.Game game, Vector2 pos)
      : base(game)
    {
      BoundingRect = new Rectangle((int)pos.X, (int)pos.Y, 0, 0);
    }

    #endregion

    #region Add, Remove elements

    public ListControls Add(LinesControl control)
    {
      control.BoundingRect = new Rectangle(BoundingRect.X, BoundingRect.Y+BoundingRect.Height+1, 
        control.BoundingRect.Width, control.BoundingRect.Height-2);
      Components.Add(control);
      if (BoundingRect.Width < control.BoundingRect.Width)
        BoundingRect.Width = control.BoundingRect.Width;
      BoundingRect.Height += (control.BoundingRect.Height+2);
      return this;
    }

    public ListControls Remove(LinesControl control)
    {
      Components.Remove(control);
      if (BoundingRect.Width == control.BoundingRect.Width)
      {
        int width = 0;
        foreach (LinesControl ctrl in Components)
        {
          if (width < ctrl.BoundingRect.Width)
            width = ctrl.BoundingRect.Width;
        }
        BoundingRect.Width = width;
      }
      BoundingRect.Height -= control.BoundingRect.Height;
      return this;
    }

    #endregion
  }
}
