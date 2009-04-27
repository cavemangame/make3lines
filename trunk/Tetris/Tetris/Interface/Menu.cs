using System;
using Microsoft.Xna.Framework;
using XnaTetris.Game;


namespace XnaTetris.Interface
{
	public class Menu : GameScene
	{
		#region Variables

		private readonly Rectangle rect;

		private Button btnStart, btnExit, btnHelp, btnHiScore, btnAuthors;

		#endregion

    #region Properties

    public LinesGame LinesGame { get { return Game as LinesGame; } }

    #endregion

    #region Constructor

    public Menu(Microsoft.Xna.Framework.Game setGame, Rectangle setRect)
			: base(setGame)
		{
			rect = setRect;
			InitButtons(setGame);
		}

		private void InitButtons(Microsoft.Xna.Framework.Game setGame)
		{
			btnStart = new Button(setGame, new Rectangle(300, 150, 200, 50), ContentSpace.GetSprite("MenuButtonStart"),
        ContentSpace.GetSprite("MenuHiButtonStart"));
			btnStart.ButtonAction += btnStart_ButtonAction;
			Components.Add(btnStart);

      btnHelp = new Button(setGame, new Rectangle(300, 220, 200, 50), ContentSpace.GetSprite("MenuButtonHelp"),
        ContentSpace.GetSprite("MenuHiButtonHelp"));
      btnHelp.ButtonAction += btnHelp_ButtonAction;
      Components.Add(btnHelp);

      btnHiScore = new Button(setGame, new Rectangle(300, 290, 200, 50), ContentSpace.GetSprite("MenuButtonHiScore"),
        ContentSpace.GetSprite("MenuHiButtonHiScore"));
      btnHiScore.ButtonAction += btnHiScore_ButtonAction;
      Components.Add(btnHiScore);

      btnAuthors = new Button(setGame, new Rectangle(300, 360, 200, 50), ContentSpace.GetSprite("MenuButtonAuthors"),
        ContentSpace.GetSprite("MenuHiButtonAuthors"));
      btnAuthors.ButtonAction += btnAuthors_ButtonAction;
      Components.Add(btnAuthors);

      btnExit = new Button(setGame, new Rectangle(300, 430, 200, 50), ContentSpace.GetSprite("MenuButtonExit"),
        ContentSpace.GetSprite("MenuHiButtonExit"));
      btnExit.ButtonAction += btnExit_ButtonAction;
      Components.Add(btnExit);
		}

		#endregion

		#region Draw

		public override void Draw(GameTime gameTime)
		{
      ContentSpace.GetSprite("MenuBackground").Render(rect);

			base.Draw(gameTime);
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
      LinesGame.ShowHiScores();
    }

    private void btnAuthors_ButtonAction(object sender, EventArgs e)
    {
      //throw new NotImplementedException();
    }

    #endregion
  }
}
