using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		private Rectangle rect;

		private SpriteHelper generalSprite;
		private SpriteHelper hilightSptite;
		private SpriteHelper pressedSprite;

		private ButtonState currentState;

		public event EventHandler ButtonAction;

		#endregion

		#region Constructor

		public Button(LinesGame setGame, Rectangle setRect, SpriteHelper setGeneralSprite)
			 : base(setGame)
		{
			rect = setRect;
			generalSprite = hilightSptite = pressedSprite = setGeneralSprite;
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
				case ButtonState.ButtonPressed:
					{
						pressedSprite.Render(rect);
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
			else if (Serv.PointInRectangle(Serv.CorrectPositionWithGameScale(Input.MousePos), rect))
				HandleMouseOver();

			base.Update(gameTime);
		}

		private void HandleMouseOver()
		{
			if (currentState != ButtonState.ButtonHilight)
				currentState = ButtonState.ButtonHilight;
		}

		private void HandleMouseClick()
		{
			ButtonAction(this, EventArgs.Empty);
		}

		#endregion
	}
}
