using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Game;
using XnaTetris.Helpers;


namespace XnaTetris.Interface
{
  public class GameField : GameScene
  {
    public const int GRID_RECTANGLE_X_COORDINATE = 270;
    public const int GRID_RECTANGLE_Y_COORDINATE = 10;
    public const int GRID_RECTANGLE_HEIGHT = 512;
    public const int GRID_RECTANGLE_WIDTH = 512;
    private readonly Rectangle rectPauseButton = new Rectangle(40, 400, 180, 50);
    private readonly Rectangle rectExitButton = new Rectangle(40, 450, 180, 50);

    private readonly Button btnPause;
    private readonly Button btnExit;

    #region Properties

    public LinesGame LinesGame { get { return Game as LinesGame; } }
    public BlocksGrid BlockGrid { get; private set; }
    public bool Paused { get; private set; }

    #endregion

    public GameField(LinesGame game)
      : base(game)
    {
      BlockGrid = new BlocksGrid(LinesGame, 
        new Rectangle(GRID_RECTANGLE_X_COORDINATE, GRID_RECTANGLE_Y_COORDINATE,
                      GRID_RECTANGLE_WIDTH, GRID_RECTANGLE_HEIGHT));

      btnPause = new Button(LinesGame, rectPauseButton, ContentSpace.GetSprite("PauseButton"), ContentSpace.GetSprite("PauseHiButton"));
      btnPause.ButtonAction += btnPause_ButtonAction;
      btnExit = new Button(LinesGame, rectExitButton, ContentSpace.GetSprite("ExitButton"), ContentSpace.GetSprite("ExitHiButton"));
      btnExit.ButtonAction += btnExit_ButtonAction;

      Components.Add(BlockGrid);
      Components.Add(btnPause);
      Components.Add(btnExit);
    }

    public override void Draw(GameTime gameTime) 
    {
      LinesGame.CurrentLevel.BackgroundSprite.Render(new Rectangle(0, 0, LinesGame.Width, LinesGame.Height));
      // Draw background boxes for all the components
      ContentSpace.GetSprite("BackgroundBigBox").Render(new Rectangle(270, 10, 512, 512));
      ContentSpace.GetSprite("BackgroundSmallBox").Render(new Rectangle(10, 10, 240, 512));

      TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"), 
        String.Format("Score: {0}", LinesGame.Score.OverallScore), 20, 20, Color.White);
      TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
        String.Format("Level Score: {0}", LinesGame.LevelScore), 20, 60, Color.WhiteSmoke);
      TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
        String.Format("Remain: {0}", Math.Max(LinesGame.CurrentLevel.LevelScore - LinesGame.LevelScore, 0)), 
        20, 100, Color.WhiteSmoke);
      TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
         LinesGame.CurrentLevel.LevelString, 20, 140, Color.LightCoral);
      TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
         Serv.GetTimeString(LinesGame.Timer), 20, 180, Color.LightPink);

      DrawScores();
      
      if (Paused)
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"), "PAUSE", 340, 280, Color.LightPink);

      base.Draw(gameTime);
    }

    private void DrawScores()
    {
      SpriteFont font = ContentSpace.GetFont("SmallFont");

      DrawScore(font, ContentSpace.GetSprite("BlueBlock"), new Rectangle(10, 530, 32, 32), 
        LinesGame.Score.BlueScore, Color.Blue);
      DrawScore(font, ContentSpace.GetSprite("GreenBlock"), new Rectangle(10, 565, 32, 32),
        LinesGame.Score.GreenScore, Color.Green);
      DrawScore(font, ContentSpace.GetSprite("RedBlock"), new Rectangle(200, 530, 32, 32),
        LinesGame.Score.RedScore, Color.Red);
      DrawScore(font, ContentSpace.GetSprite("YellowBlock"), new Rectangle(200, 565, 32, 32),
        LinesGame.Score.YellowScore, Color.Yellow);
      DrawScore(font, ContentSpace.GetSprite("WhiteBlock"), new Rectangle(390, 530, 32, 32),
        LinesGame.Score.WhiteScore, Color.White);
      DrawScore(font, ContentSpace.GetSprite("GrayBlock"), new Rectangle(390, 565, 32, 32),
        LinesGame.Score.GrayScore, Color.LightGray);
    }

    private static void DrawScore(SpriteFont font, SpriteHelper block, Rectangle rect, long score, Color col)
    {
      block.Render(rect);
      // shadow
      TextHelper.DrawShadowedText(font, String.Format("= {0}", score),
        rect.Right + 8, rect.Top + 5, col);
    }

    void btnExit_ButtonAction(object sender, EventArgs e)
    {
      LinesGame.ExitToMenu();
    }

    void btnPause_ButtonAction(object sender, EventArgs e)
    {
      PauseAction();
    }

    public void PauseAction()
    {
      Paused = !Paused;
      BlockGrid.Enabled = !Paused;
    }
  }
}