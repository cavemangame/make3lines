using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;
using XnaTetris.Game;

namespace XnaTetris.Interface
{
	public class Menu : GameScene
	{
		#region Variables

		private readonly Rectangle rect;


	  private readonly SpriteHelper background; 

		private Button btnStart, btnExit, btnHelp, btnHiScore, btnAuthors;

		#endregion

    #region Properties

    public LinesGame LinesGame { get { return Game as LinesGame; } }

    #endregion

    #region Constructor

    public Menu(LinesGame setGame, Rectangle setRect, SpriteHelper setBackground)
			: base(setGame)
		{
			rect = setRect;
			background = setBackground;
			InitButtons(setGame);
		}

		private void InitButtons(Microsoft.Xna.Framework.Game setGame)
		{
			btnStart = new Button(setGame, new Rectangle(420, 200, 200, 50), ContentSpace.menuButtonStart,
        ContentSpace.menuHiButtonStart);
			btnStart.ButtonAction += btnStart_ButtonAction;
			Components.Add(btnStart);

      btnHelp = new Button(setGame, new Rectangle(420, 270, 200, 50), ContentSpace.menuButtonHelp,
        ContentSpace.menuHiButtonHelp);
      btnHelp.ButtonAction += btnHelp_ButtonAction;
      Components.Add(btnHelp);

      btnHiScore = new Button(setGame, new Rectangle(420, 340, 200, 50), ContentSpace.menuButtonHiScore,
        ContentSpace.menuHiButtonHiScore);
      btnHiScore.ButtonAction += btnHiScore_ButtonAction;
      Components.Add(btnHiScore);

      btnAuthors = new Button(setGame, new Rectangle(420, 410, 200, 50), ContentSpace.menuButtonAuthors,
        ContentSpace.menuHiButtonAuthors);
      btnAuthors.ButtonAction += btnAuthors_ButtonAction;
      Components.Add(btnAuthors);

      btnExit = new Button(setGame, new Rectangle(420, 480, 200, 50), ContentSpace.menuButtonExit,
        ContentSpace.menuHiButtonExit);
      btnExit.ButtonAction += btnExit_ButtonAction;
      Components.Add(btnExit);
		}

		#endregion

		#region Draw

		public override void Draw(GameTime gameTime)
		{
			background.Render(rect);

			base.Draw(gameTime);
		}

		#endregion

		#region Update

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		#endregion

    #region Events

    private void btnStart_ButtonAction(object sender, EventArgs e)
    {
      LinesGame.Start();
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
