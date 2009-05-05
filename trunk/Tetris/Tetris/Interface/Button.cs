using System;
using Microsoft.Xna.Framework;
using XnaTetris.Helpers;
using XnaTetris.Game;

namespace XnaTetris.Interface
{
	public enum ButtonState
	{
		ButtonGeneral,
		ButtonHilight,
	}

	public class Button : LinesControl
	{
		#region Variables

		private readonly SpriteHelper generalSprite;
		private readonly SpriteHelper hilightSptite;

		private ButtonState currentState;

		public event EventHandler ButtonAction;

		#endregion

		#region Constructors

		public Button(Microsoft.Xna.Framework.Game setGame, Rectangle setRect, SpriteHelper setGeneralSprite)
      : this(setGame, setRect, setGeneralSprite, setGeneralSprite)
		{
		}

    public Button(Microsoft.Xna.Framework.Game setGame, Rectangle setRect,
      SpriteHelper setGeneralSprite, SpriteHelper setHilightedSprite)
      : base(setGame, setRect)
    {
      generalSprite = setGeneralSprite;
      hilightSptite = setHilightedSprite;
    }

		#endregion

		#region Draw

		public override void Draw(GameTime gameTime)
		{
			switch (currentState)
			{
				case ButtonState.ButtonGeneral:
					{
						generalSprite.Render(BoundingRect);
						break;
					}
				case ButtonState.ButtonHilight:
					{
            hilightSptite.Render(BoundingRect);
						break;
					}
				default:
					break;
			}

			base.Draw(gameTime);
		}

		#endregion

    #region Events

    protected override void HandleMouseOver(bool isOver)
		{
      if (isOver)
      {
        if (currentState != ButtonState.ButtonHilight)
          currentState = ButtonState.ButtonHilight;
      }
      else
      {
        if (currentState != ButtonState.ButtonGeneral)
          currentState = ButtonState.ButtonGeneral;
      }
		}

    protected override void HandleMouseClick()
		{
			ButtonAction(this, EventArgs.Empty);
    }

    #endregion

  }
}
