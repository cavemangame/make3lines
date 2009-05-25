using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;


namespace XnaTetris.Interface
{
	public class Menu : GameScene
	{
		#region Variables
		private readonly Rectangle backgroundRect;
		private Button btnNew, btnExit, btnContinue, btnHiScores, btnHelp;
	  private TextBox textBox;
	  private SpriteFont helloFont;
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
      helloFont = ContentSpace.GetFont("SmallFont");
      if (LinesGame.CurrentPlayerName.Length == 0)
        btnContinue.Enabled = false;
		}

		private void InitButtons(Microsoft.Xna.Framework.Game setGame)
		{
			btnNew = new Button(setGame, new Rectangle(330, 200, 137, 30), ContentSpace.GetSprite("MenuButtonNew"),
        ContentSpace.GetSprite("MenuHiButtonNew"));
			btnNew.ButtonAction += btnNew_ButtonAction;
			Components.Add(btnNew);

      btnContinue = new Button(setGame, new Rectangle(260, 240, 287, 30), ContentSpace.GetSprite("MenuButtonContinue"),
        ContentSpace.GetSprite("MenuHiButtonContinue"));
      btnContinue.ButtonAction += btnContinue_ButtonAction;
      Components.Add(btnContinue);

      btnHiScores = new Button(setGame, new Rectangle(302, 280, 195, 30), ContentSpace.GetSprite("MenuButtonHiScore"),
        ContentSpace.GetSprite("MenuHiButtonHiScore"));
      btnHiScores.ButtonAction += btnHiScore_ButtonAction;
      Components.Add(btnHiScores);

      btnHelp = new Button(setGame, new Rectangle(310, 320, 180, 30), ContentSpace.GetSprite("MenuButtonHelp"),
        ContentSpace.GetSprite("MenuHiButtonHelp"));
      btnHelp.ButtonAction += btnHelp_ButtonAction;
      Components.Add(btnHelp);

      btnExit = new Button(setGame, new Rectangle(325, 360, 147, 30), ContentSpace.GetSprite("MenuButtonExit"),
        ContentSpace.GetSprite("MenuHiButtonExit"));
      btnExit.ButtonAction += btnExit_ButtonAction;
      Components.Add(btnExit);

      textBox = new TextBox(setGame, new Rectangle(200, 300, 400, 50), ContentSpace.GetSprite("TextBackground"), null,
        Color.WhiteSmoke, 1.5f);
      textBox.EnterKeyPressed += textBox_EnterKeyPressed;
      Components.Add(textBox);
      textBox.Hide();
    }
		#endregion

		#region Draw
		public override void Draw(GameTime gameTime)
		{
      ContentSpace.GetSprite("MenuBackground").Render(backgroundRect);
      if (LinesGame.CurrentPlayerName.Length > 0)
      {
        String hello = String.Format("Добро пожаловать, {0}", LinesGame.CurrentPlayerName);
        int len = (int)helloFont.MeasureString(hello).X;

        TextHelper.DrawShadowedText(ContentSpace.GetFont("SmallFont"),
          hello,
          400 - len/2, 100, Color.LightCoral);  
      }

			base.Draw(gameTime);
		}
		#endregion

    #region Events
    private void btnNew_ButtonAction(object sender, EventArgs e)
    {
      EnableButtons(false);
      textBox.Show();
    }

    private void btnExit_ButtonAction(object sender, EventArgs e)
    {
      if (LinesGame.CurrentPlayerName.Length > 0)
        Serv.PlayerName = LinesGame.CurrentPlayerName;
      Game.Exit();
    }

    private void btnContinue_ButtonAction(object sender, EventArgs e)
    {
      LinesGame.Player = new Player(LinesGame.CurrentPlayerName);
      LinesGame.Score.Copy(LinesGame.Player.PlayerScore);
      Hide();
      LinesGame.Start();
    }

    private void btnHiScore_ButtonAction(object sender, EventArgs e)
    {
      Hide();
      LinesGame.ShowHiScores();
    }

    private void btnHelp_ButtonAction(object sender, EventArgs e)
    {
      Hide();
      LinesGame.ShowHelp();
    }

    private void textBox_EnterKeyPressed(object sender, EventArgs e)
    {
      if (textBox.Text.Length > 0)
      {
        LinesGame.Player = new Player(textBox.Text);
        LinesGame.CurrentPlayerName = textBox.Text;
        LinesGame.Score.Copy(LinesGame.Player.PlayerScore);
        EnableButtons(true);
        textBox.Hide();
        Hide();
        LinesGame.Start();
      }
    }

    #endregion

    private void EnableButtons(bool isEnable)
    {
      btnNew.Enabled = btnExit.Enabled = btnContinue.Enabled = btnHelp.Enabled = btnHiScores.Enabled = isEnable;
    }
  }
}
