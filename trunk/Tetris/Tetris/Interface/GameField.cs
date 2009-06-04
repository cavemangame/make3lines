using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Algorithms;
using XnaTetris.Game;
using XnaTetris.Helpers;
using XnaTetris.Particles;


namespace XnaTetris.Interface
{
  public class GameField : GameScene
  {
    public const int GRID_RECTANGLE_X_COORDINATE = 270;
    public const int GRID_RECTANGLE_Y_COORDINATE = 10;
    public const int GRID_RECTANGLE_HEIGHT = 512;
    public const int GRID_RECTANGLE_WIDTH = 512;
    private readonly Rectangle rectPauseButton = new Rectangle(60, 415, 137, 30);
    private readonly Rectangle rectExitButton = new Rectangle(55, 450, 147, 30);
    private readonly Rectangle rectEndDialog = new Rectangle(200, 200, 400, 200);
    private readonly Rectangle rectWinDialog = new Rectangle(100, 100, 600, 400);

    private const int MAX_LEVEL = 21;

    private readonly Button btnPause;
    private readonly Button btnExit;

    private LevelWindow startWindow, endWindow, gameoverWindow, gamewinWindow;
    public Level CurrentLevel { get; private set; }
    public Scores LevelScore  = new Scores();

    #region Properties
    public LinesGame LinesGame { get { return Game as LinesGame; } }
    public BlocksGrid BlockGrid { get; private set; }
    public bool Paused { get; private set; }

    public ExplosionParticleManager Explosion { get; set; }

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

      Explosion = new ExplosionParticleManager(LinesGame, 16);
      Explosion.Initialize();
      Components.Add(Explosion);
    }

    #endregion

    #region Draw

    public override void Draw(GameTime gameTime) 
    {
      CurrentLevel.BackgroundSprite.Render(spriteBatch, new Rectangle(0, 0, LinesGame.Width, LinesGame.Height));
      // Draw background boxes for all the components
      ContentSpace.GetSprite("BackgroundBigBox").Render(spriteBatch, new Rectangle(270, 10, 512, 512));
      ContentSpace.GetSprite("BackgroundSmallBox").Render(spriteBatch, new Rectangle(10, 10, 240, 512));

      if (LinesGame.GameState == Serv.GameState.GameStateRunning)
      {
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    CurrentLevel.LevelString, 60, 20, Color.LightCoral);
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    Serv.GetTimeString(LinesGame.Timer), 30, 60, Color.LightPink);

        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    String.Format("Очков: {0}", LinesGame.Score.OverallScore), 30, 120, Color.White);
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    String.Format("За уровень: {0}", LinesGame.LevelScore), 30, 160, Color.WhiteSmoke);
        TextHelper.DrawShadowedText(ContentSpace.GetFont("NormalFont"),
                                    String.Format("Осталось: {0}",
                                                  Math.Max(CurrentLevel.LevelScore - LinesGame.LevelScore, 0)),
                                    30, 200, Color.WhiteSmoke);

        DrawScores();

        if (Paused)
          TextHelper.DrawShadowedText(ContentSpace.GetFont("BigFont"), "ПАУЗА", 340, 280, Color.LightPink);
      }

      base.Draw(gameTime);

      if (startWindow != null && startWindow.Visible)
      {
        startWindow.Draw(gameTime);
      }
      if (endWindow != null && endWindow.Visible)
      {
        endWindow.Draw(gameTime);
      }
      if (gameoverWindow != null && gameoverWindow.Visible)
      {
        gameoverWindow.Draw(gameTime);
      }
      if (gamewinWindow != null && gamewinWindow.Visible)
      {
        gamewinWindow.Draw(gameTime);
      }

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

    private void DrawScore(SpriteFont font, SpriteHelper block, Rectangle rect, long score, Color col)
    {
      block.Render(spriteBatch, rect);
      // shadow
      TextHelper.DrawShadowedText(font, String.Format("= {0}", score),
        rect.Right + 8, rect.Top + 5, col);
    }

    #endregion

    #region Update

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (startWindow != null && startWindow.Enabled)
      {
        startWindow.Update(gameTime);
      }
      if (endWindow != null && endWindow.Enabled)
      {
        endWindow.Update(gameTime);
      }
      if (gameoverWindow != null && gameoverWindow.Enabled)
      {
        gameoverWindow.Update(gameTime);
      }
      if (gamewinWindow != null && gamewinWindow.Enabled)
      {
        gamewinWindow.Update(gameTime);
      }
    }
    #endregion

    #region Start and End Level Events

    public void StartNewGame()
    {
      CurrentLevel = new Level(LinesGame.Player.PlayerLevel, LinesGame);

      startWindow = CurrentLevel.StartWindow;
      startWindow.BtnOkClick += startWindow_BtnOkClick;
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

      LinesGame.LevelScore = 0;
      LinesGame.GameState = Serv.GameState.GameStateBetweenLevel;
      LinesGame.Timer = CurrentLevel.Time*1000;
      BlockGrid.Enabled = false;
      Show();
    }

    public void EndLevel()
    {
      endWindow = new LevelWindow(LinesGame, rectEndDialog, CreateEndDialog());
      endWindow.BtnOkClick += endWindow_BtnOkClick;
      BlockGrid.Enabled = false;
      endWindow.Show();
    }

    public void GameOver()
    {
      gameoverWindow = new LevelWindow(LinesGame, rectEndDialog, CreateGameOverDialog());
      gameoverWindow.BtnOkClick += gameoverWindow_BtnOkClick;
      BlockGrid.Enabled = false;
      gameoverWindow.Show();
    }

    public void GameWin()
    {
      gamewinWindow = new LevelWindow(LinesGame, rectWinDialog, CreateGameWinDialog());
      gamewinWindow.BtnOkClick += gamewinWindow_BtnOkClick;
      Serv.SaveHiScoreIfNeeded(LinesGame.Score);
      BlockGrid.Enabled = false;
      gamewinWindow.Show();
    }

    private void startWindow_BtnOkClick(object sender, EventArgs e)
    {
      startWindow.Hide();
      LevelScore.Reset();
      BlockGrid.Enabled = true;
      BlockGrid.ActiveBlocks = 0;
      BlockGrid.Restart();
      LinesGame.GameState = Serv.GameState.GameStateRunning;
    }

    private void endWindow_BtnOkClick(object sender, EventArgs e)
    {
      endWindow.Hide();
      if (CurrentLevel.Number >= MAX_LEVEL)
      {
        GameWin();
      }
      else
        StartLevel();
    }

    private void gameoverWindow_BtnOkClick(object sender, EventArgs e)
    {
      gameoverWindow.Hide();
      Hide();
      LinesGame.ShowMenu();
    }

    private void gamewinWindow_BtnOkClick(object sender, EventArgs e)
    {
      gamewinWindow.Hide();
      Hide();
      LinesGame.ShowMenu();
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

    #region Dialogs Text

    private ConvertTaggedTextHelper CreateEndDialog()
    {
      var helper = new ConvertTaggedTextHelper(rectEndDialog);
      var font = ContentSpace.GetFont("SmallFont");
      helper.Texts.Add(new TextToRender(font, new Vector2(210, 205), 1.5f, Color.Red, 
        String.Format("Результаты уровня {0}:", LinesGame.Player.PlayerLevel)));

      helper.Texts.Add(new TextToRender(font, new Vector2(210, 240), 1f, Color.Black,
        String.Format("Очков набрано {0}:", LevelScore.OverallScore)));
      helper.Texts.Add(new TextToRender(font, new Vector2(210, 260), 1f, Color.Black,
        String.Format("Блоков убито:")));

      helper.Texts.Add(new TextToRender(font, new Vector2(210, 280), 1f, Color.Blue,
        String.Format("Синих: {0}", LevelScore.BlueScore)));
      helper.Texts.Add(new TextToRender(font, new Vector2(410, 280), 1f, Color.Red,
        String.Format("Красных: {0}", LevelScore.RedScore)));
      helper.Texts.Add(new TextToRender(font, new Vector2(210, 300), 1f, Color.Green,
        String.Format("Зеленых: {0}", LevelScore.GreenScore)));
      helper.Texts.Add(new TextToRender(font, new Vector2(410, 300), 1f, Color.Yellow,
        String.Format("Желтых: {0}", LevelScore.YellowScore)));
      helper.Texts.Add(new TextToRender(font, new Vector2(210, 320), 1f, Color.White,
        String.Format("Белых: {0}", LevelScore.WhiteScore)));
      helper.Texts.Add(new TextToRender(font, new Vector2(410, 320), 1f, Color.Gray,
        String.Format("Серых: {0}", LevelScore.GrayScore)));

      return helper;
    }

    private ConvertTaggedTextHelper CreateGameOverDialog()
    {
      var helper = new ConvertTaggedTextHelper(rectEndDialog);
      var font = ContentSpace.GetFont("SmallFont");

      helper.Texts.Add(new TextToRender(font, new Vector2(310, 205), 1.5f, Color.Red,
                                        String.Format("Вы проиграли!")));

      helper.Texts.Add(new TextToRender(font, new Vector2(210, 240), 1f, Color.Red,
                                        String.Format("Вы проиграли, но ваш результат")));
      helper.Texts.Add(new TextToRender(font, new Vector2(210, 260), 1f, Color.Red,
                                        String.Format("до этого уровня сохранен.")));
      helper.Texts.Add(new TextToRender(font, new Vector2(210, 280), 1f, Color.Red,
                                        String.Format("Попробуйте еще раз.")));

      return helper;
    }

    private ConvertTaggedTextHelper CreateGameWinDialog()
    {
      var helper = new ConvertTaggedTextHelper(rectEndDialog);
      var font = ContentSpace.GetFont("SmallFont");

      helper.Texts.Add(new TextToRender(font, new Vector2(210, 205), 1.5f, Color.Silver,
        String.Format(" Вы прошли все испытания!")));

      helper.Texts.Add(new TextToRender(font, new Vector2(210, 240), 1f, Color.Silver,
         String.Format(" По результатам испытаний вы получаете")));
      helper.Texts.Add(new TextToRender(font, new Vector2(210, 260), 1f, Color.Silver,
        String.Format("звание   {0}.", Title.GetTitle(LinesGame.Score.OverallScore))));

      bool HasWonCompetition = (LinesGame.Score.OverallScore >= 70000);
      helper.Texts.Add(new TextToRender(font, new Vector2(210, 280), 1f, Color.Silver,
        String.Format(" Боги посовещались и решили, что")));

      if (!HasWonCompetition)
      {
        helper.Texts.Add(new TextToRender(font, new Vector2(210, 300), 1f, Color.Red,
          String.Format("Ты не подходишь никому :(")));
      }
      else
      {
        helper.Texts.Add(new TextToRender(font, new Vector2(210, 300), 1f, Color.Gold,
          String.Format("Тебя берет в помощники   {0}", Serv.GetGodNameByScore(LinesGame.Score))));
       /* helper.Texts.Add(new TextToRender(font, new Vector2(210, 320), 1.5f, Color.Gold,
          String.Format("ПОЗДРАВЛЯЕМ!!!")));*/

      }

      return helper;
    }

    #endregion
  }
}
