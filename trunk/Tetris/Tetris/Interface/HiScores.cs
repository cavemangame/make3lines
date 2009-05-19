using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;


namespace XnaTetris.Interface
{
  public class HiScores : GameScene
  {
    #region Variables

    private readonly Rectangle backgroundRect;
    private Button btnOk;
    private ListControls hiscores;

    #endregion

    #region Properties

    public LinesGame LinesGame { get { return Game as LinesGame; } }

    #endregion

    #region Constructor

    public HiScores(Microsoft.Xna.Framework.Game setGame)
			: base(setGame)
		{
      backgroundRect = new Rectangle(0, 0, LinesGame.Width, LinesGame.Height);
      InitButtons();
      InitScores();
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      // TODO: use own background
      ContentSpace.GetSprite("LevelBackground").Render(backgroundRect);

      base.Draw(gameTime);
    }

    #endregion

    private void InitButtons()
    {
      btnOk = new Button(LinesGame, new Rectangle(360, 550, 80, 40),
                         ContentSpace.GetSprite("OkButton"),
                         ContentSpace.GetSprite("HiOkButton"));
      btnOk.ButtonAction += btnOk_ButtonAction;
      Components.Add(btnOk);
    }

    private void InitScores()
    {
      hiscores = new ListControls(LinesGame, new Vector2(10,10));
      SpriteFont font = ContentSpace.GetFont("NormalFont");
      XmlDocument doc = new XmlDocument();
      doc.Load("Content/hiscores.xml");
      XmlNode node = doc.SelectSingleNode(String.Format("/hiscores"));
      int y = 10;
      foreach (XmlNode n in node.ChildNodes)
      {
        hiscores.Add(new StaticLabel(LinesGame, new Rectangle(10, y, LinesGame.Width - 20, 50),
         String.Format("{0}:   {1}", n.Attributes["text"].Value, n.Attributes["score"].Value), font, 
         ColorHelper.ColorFromString(n.Attributes["color"].Value), 1.4f));
        y += 50;
      }

      hiscores.Show();
      Components.Add(hiscores);
    }

    private void btnOk_ButtonAction(object sender, EventArgs e)
    {
      Hide();
      LinesGame.ShowMenu();
    }
  }
}
