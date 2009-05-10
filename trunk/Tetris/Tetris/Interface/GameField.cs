using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Algorithms;
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

    private LevelWindow startWindow, endWindow;
    public Level CurrentLevel { get; private set; }
    public Scores LevelScore  = new Scores();

    #region Properties
    public LinesGame LinesGame { get { return Game as LinesGame; } }
    public BlocksGrid BlockGrid { get; private set; }
    public bool Paused { get; private set; }
    #endregion

    #region Constructor

    public GameField(Microsoft.Xna.Framework.Game game)
      : base(game)
    {
      BlockGrid = new BlocksGrid(LinesGame, 
        new Rectangle(GRID_RECTANGLE_X_COORDINATE, GRID_RECTANGLE_Y_COORDINATE,
                      GRID_RECTANGLE_WIDTH, GRID_RECTANGLE_HEIGHT));
      Components.Add(BlockGrid);

      btnPause = new Button(LinesGame, rectPauseButton, ContentSpace.GetSprite("PauseButton"), ContentSpace.GetSprite("PauseHiButton"));
      btnPause.ButtonAction += btnPause_ButtonAction;
      Components.Add(btnPause);
      
      btnExit = new Button(LinesGame, rectExitButton, ContentSpace.GetSprite("ExitButton"), ContentSpace.GetSprite("ExitHiButton"));
      btnExit.ButtonAction += btnExit_ButtonAction;
      Components.Add(btnExit);
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime) 
    {
      CurrentLevel.BackgroundSprite.Render(new Rectangle(0, 0, LinesGame.Width, LinesGame.Height));
      // Draw background boxes for all the components
      ContentSpace.GetSprite("BackgroundBigBox").Render(new Rectangle(270, 10, 512, 512));
      ContentSpace.GetSprite("BackgroundSmallBox").Render(new Rectangle(10, 10, 240, 512));

      if (LinesGame.GameState == Serv.GameState.GameStateRunning)
      {
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    CurrentLevel.LevelString, 60, 20, Color.LightCoral);
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    Serv.GetTimeString(LinesGame.Timer), 30, 60, Color.LightPink);

        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    String.Format("�����: {0}", LinesGame.Score.OverallScore), 30, 120, Color.White);
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    String.Format("�� �������: {0}", LinesGame.LevelScore), 30, 160, Color.WhiteSmoke);
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    String.Format("��������: {0}",
                                                  Math.Max(CurrentLevel.LevelScore - LinesGame.LevelScore, 0)),
                                    30, 200, Color.WhiteSmoke);

        DrawScores();

        if (Paused)
          TextHelper.DrawShadowedText(ContentSpace.GetFont("BigFont"), "�����", 340, 280, Color.LightPink);
      }

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

    #endregion

    #region Start and End Level Events

    public void StartNewGame()
    {
      CurrentLevel = new Level(LinesGame.Player.PlayerLevel, LinesGame);

      startWindow = CurrentLevel.StartWindow;
      startWindow.BtnOkClick += startWindow_BtnOkClick;
      Components.Add(startWindow);
      startWindow.Show();

      LinesGame.LevelScore = 0;
      LinesGame.GameState = Serv.GameState.GameStateBetweenLevel;
      LinesGame.Timer = CurrentLevel.Time * 1000;

      BlockGrid.Enabled = false;
      Show();
    }

    public void StartLevel()
    {
      CurrentLevel = new Level(++LinesGame.Player.PlayerLevel, LinesGame);
      startWindow = CurrentLevel.StartWindow;
      startWindow.BtnOkClick += startWindow_BtnOkClick;
      startWindow.Show();
    }

    public void EndLevel()
    {
      endWindow = new LevelWindow(LinesGame, new Rectangle(200, 200, 300, 150), CreateEndDialog());
      endWindow.BtnOkClick += endWindow_BtnOkClick;
      Components.Add(endWindow);
      endWindow.Show();
    }

    private void startWindow_BtnOkClick(object sender, EventArgs e)
    {
      startWindow.Hide();
      LevelScore.Reset();
      BlockGrid.Enabled = true;
      BlockGrid.Restart();
      LinesGame.GameState = Serv.GameState.GameStateRunning;
    }

    private void endWindow_BtnOkClick(object sender, EventArgs e)
    {
      endWindow.Hide();
      StartLevel();
    }

    #endregion

    #region Button Events

    private void btnExit_ButtonAction(object sender, EventArgs e)
    {
      Hide();
      LinesGame.Player.Save();
      LinesGame.ShowMenu();
    }

    private void btnPause_ButtonAction(object sender, EventArgs e)
    {
      PauseAction();
    }

    public void PauseAction()
    {
      Paused = !Paused;
      BlockGrid.Enabled = !Paused;
    }

    #endregion 

    private ConvertTaggedTextHelper CreateEndDialog()
    {
      var helper = new ConvertTaggedTextHelper(new Rectangle(100, 200, 400, 200));

      return helper;
    }
  }
}
