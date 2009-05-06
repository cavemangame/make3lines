using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;


namespace XnaTetris.Interface
{
	public class Menu : GameScene
	{
		#region Variables
		private readonly Rectangle backgroundRect;
		private Button btnStart, btnExit, btnHelp, btnHiScores, btnAuthors;
	  private TextBox textBox, textBox2;
	  private ListControls profiles;
		#endregion

    #region Properties
    public LinesGame LinesGame { get { return Game as LinesGame; } }
    #endregion

    #region Constructor
    public Menu(Microsoft.Xna.Framework.Game setGame, Rectangle setRect)
			: base(setGame)
		{
			backgroundRect = setRect;
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

      btnHiScores = new Button(setGame, new Rectangle(300, 290, 200, 50), ContentSpace.GetSprite("MenuButtonHiScore"),
        ContentSpace.GetSprite("MenuHiButtonHiScore"));
      btnHiScores.ButtonAction += btnHiScore_ButtonAction;
      Components.Add(btnHiScores);

      btnAuthors = new Button(setGame, new Rectangle(300, 360, 200, 50), ContentSpace.GetSprite("MenuButtonAuthors"),
        ContentSpace.GetSprite("MenuHiButtonAuthors"));
      btnAuthors.ButtonAction += btnAuthors_ButtonAction;
      Components.Add(btnAuthors);

      btnExit = new Button(setGame, new Rectangle(300, 430, 200, 50), ContentSpace.GetSprite("MenuButtonExit"),
        ContentSpace.GetSprite("MenuHiButtonExit"));
      btnExit.ButtonAction += btnExit_ButtonAction;
      Components.Add(btnExit);

      textBox = new TextBox(setGame, new Rectangle(200, 300, 400, 50), null,
        Color.WhiteSmoke, 1.5f);
      textBox.EnterKeyPressed += textBox_EnterKeyPressed;

      textBox2 = new TextBox(setGame, new Rectangle(200, 300, 400, 50), null,
        Color.WhiteSmoke, 1.5f);
      textBox2.EnterKeyPressed += textBox_EnterKeyPressed;

      profiles = new ListControls(setGame, new Vector2(200, 300));
		  profiles.Add(textBox).Add(textBox2);
      profiles.Show();
	    Components.Add(profiles);
      
		  EnableButtons(false);
    }
		#endregion

		#region Draw
		public override void Draw(GameTime gameTime)
		{
      ContentSpace.GetSprite("MenuBackground").Render(backgroundRect);

			base.Draw(gameTime);
		}
		#endregion

    #region Events
    private void btnStart_ButtonAction(object sender, EventArgs e)
    {
      Hide();
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
      Hide();
      LinesGame.ShowHiScores();
    }

    private void btnAuthors_ButtonAction(object sender, EventArgs e)
    {
      //throw new NotImplementedException();
    }

    private void textBox_EnterKeyPressed(object sender, EventArgs e)
    {
      LinesGame.Player = new Player(textBox.Text);
      LinesGame.Score.Copy( LinesGame.Player.PlayerScore);
      profiles.Hide();
      EnableButtons(true);
    }
    #endregion

    private void EnableButtons(bool isEnable)
    {
      btnStart.Enabled = btnExit.Enabled = btnHelp.Enabled = btnAuthors.Enabled = btnHiScores.Enabled = isEnable;
    }
  }
}
