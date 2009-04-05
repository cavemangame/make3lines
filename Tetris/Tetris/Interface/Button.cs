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
		ButtonPressed,
	}

	public class Button : DrawableGameComponent
	{
		#region Variables

		private readonly Rectangle rect;

		private readonly SpriteHelper generalSprite;
		private readonly SpriteHelper hilightSptite;

		private ButtonState currentState;

		public event EventHandler ButtonAction;

		#endregion

		#region Constructor

		public Button(Microsoft.Xna.Framework.Game setGame, Rectangle setRect, SpriteHelper setGeneralSprite)
      : this(setGame, setRect, setGeneralSprite, setGeneralSprite)
		{
		}

    public Button(Microsoft.Xna.Framework.Game setGame, Rectangle setRect,
      SpriteHelper setGeneralSprite, SpriteHelper setHilightedSprite)
      : base(setGame)
    {
      rect = setRect;
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
						generalSprite.Render(rect);
						break;
					}
				case ButtonState.ButtonHilight:
					{
						hilightSptite.Render(rect);
						break;
					}
				default:
					break;
			}

			base.Draw(gameTime);
		}

		#endregion

		#region Update

		public override void Update(GameTime gameTime)
		{
			if (Input.MouseLeftButtonJustPressed)
			{
				if (Serv.PointInRectangle(Serv.CorrectPositionWithGameScale(Input.MousePos), rect))
					HandleMouseClick();
			}
			else 
      {
        HandleMouseOver(Serv.PointInRectangle(Serv.CorrectPositionWithGameScale(Input.MousePos), rect));
      }

			base.Update(gameTime);
		}

		private void HandleMouseOver(bool isOver)
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

		private void HandleMouseClick()
		{
			ButtonAction(this, EventArgs.Empty);
		}

		#endregion
	}
}
