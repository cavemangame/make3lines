using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaTetris.Helpers;
using XnaTetris.Game;

namespace XnaTetris.Interface
{
	public class Menu : DrawableGameComponent
	{
		#region Variables

		private Rectangle rect;

    private SpriteHelper background; 

		private Button btnStart, btnExit, btnHelp, btnHiScore, btnAuthors;

		#endregion

		#region Constructor

		public Menu(LinesGame setGame, Rectangle setRect, SpriteHelper setBackground)
			: base(setGame)
		{
			rect = setRect;
			background = setBackground;
			InitButtons(setGame);
		}

		private void InitButtons(LinesGame setGame)
		{
			btnStart = new Button(setGame, new Rectangle(420, 200, 200, 50), ContentSpace.menuButtonStart,
        ContentSpace.menuHiButtonStart);
			btnStart.ButtonAction += btnStart_ButtonAction;
			this.Game.Components.Add(btnStart);

      btnHelp = new Button(setGame, new Rectangle(420, 270, 200, 50), ContentSpace.menuButtonHelp,
        ContentSpace.menuHiButtonHelp);
      btnHelp.ButtonAction += btnHelp_ButtonAction;
      this.Game.Components.Add(btnHelp);

      btnHiScore = new Button(setGame, new Rectangle(420, 340, 200, 50), ContentSpace.menuButtonHiScore,
        ContentSpace.menuHiButtonHiScore);
      btnHiScore.ButtonAction += btnHiScore_ButtonAction;
      this.Game.Components.Add(btnHiScore);

      btnAuthors = new Button(setGame, new Rectangle(420, 410, 200, 50), ContentSpace.menuButtonAuthors,
        ContentSpace.menuHiButtonAuthors);
      btnAuthors.ButtonAction += btnAuthors_ButtonAction;
      this.Game.Components.Add(btnAuthors);

      btnExit = new Button(setGame, new Rectangle(420, 480, 200, 50), ContentSpace.menuButtonExit,
        ContentSpace.menuHiButtonExit);
      btnExit.ButtonAction += btnExit_ButtonAction;
      this.Game.Components.Add(btnExit);

		}

		#endregion

		#region Draw

		public override void Draw(GameTime gameTime)
		{
			background.Render(rect);

			// draw buttons
			btnStart.Draw(gameTime);
			btnExit.Draw(gameTime);
			btnHiScore.Draw(gameTime);
			btnHelp.Draw(gameTime);
			btnAuthors.Draw(gameTime);

			base.Draw(gameTime);
		}

		#endregion

		#region Update

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

    public void EnableComponents(bool isEnable)
    {
      Enabled = btnStart.Enabled = btnExit.Enabled = btnHiScore.Enabled = btnHelp.Enabled = 
      btnAuthors.Enabled = isEnable;
    }

		#endregion

    #region Events

    private void btnStart_ButtonAction(object sender, EventArgs e)
    {
      (Game as LinesGame).Start();
    }

    private void btnExit_ButtonAction(object sender, EventArgs e)
    {
      Game.Exit();
    }

    private void btnHelp_ButtonAction(object sender, EventArgs e)
    {
      //throw new NotImplementedException();
    }

    private void btnHiScore_ButtonAction(object sender, EventArgs e)
    {
      //throw new NotImplementedException();
    }

    private void btnAuthors_ButtonAction(object sender, EventArgs e)
    {
      //throw new NotImplementedException();
    }

    #endregion
  }
}
