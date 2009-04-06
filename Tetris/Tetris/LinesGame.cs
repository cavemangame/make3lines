using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;
using XnaTetris.Graphics;
using XnaTetris.Game;
using XnaTetris.Algorithms;
using XnaTetris.Interface;

namespace XnaTetris
{
  public class LinesGame : BaseGame
  {
    #region Constants

    private readonly Rectangle rectPauseButton = new Rectangle(55, 600, 200, 50);
    private readonly Rectangle rectExitButton = new Rectangle(55, 670, 200, 50);

    #endregion

    #region Variables

    private readonly BlocksGrid blocksGrid;
    private Menu menu;

    // graphics
    Texture2D backgroundTexture, backgroundSmallBoxTexture, backgroundBigBoxTexture,
      buttonPauseTexture, buttonExitTexture;
    SpriteHelper background, backgroundSmallBox, backgroundBigBox, buttonPause, buttonExit;

    private Level currentLevel;
    private int curLevelNumber;
    private int elapsedGameMs;

    /// <summary>
    /// Interface
    /// </summary>
    private Button btnPause, btnExit;

    #endregion

    #region Properties

    public long Timer { get; set; }
    public int Score { get; set; }
    public Serv.GameState GameState { get; set; }

    #endregion

    #region Constructor

    public LinesGame()
    {
      blocksGrid = new BlocksGrid(this, new Rectangle(310, 35, 700, 700));
      Components.Add(blocksGrid);
    }

    #endregion

    #region Init and Load methods

    protected override void Initialize()
    {
      IsMouseVisible = true;
      GameState = Serv.GameState.GameStateMenu;

      base.Initialize();
    }

    protected override void LoadContent()
    {
      // Load all our content
      content.RootDirectory = "Content";
      backgroundTexture = content.Load<Texture2D>("skybackground");
      backgroundSmallBoxTexture = content.Load<Texture2D>("BackgroundSmallBox");
      backgroundBigBoxTexture = content.Load<Texture2D>("BackgroundBigBox");
      buttonPauseTexture = content.Load<Texture2D>("PauseButton");
      buttonExitTexture = content.Load<Texture2D>("ExitButton");

      // Load menu content
      ContentSpace.menuButtonStart = new SpriteHelper(content.Load<Texture2D>("MenuButtonStart"),
        new Rectangle(0, 0, 200, 50));
      ContentSpace.menuHiButtonStart = new SpriteHelper(content.Load<Texture2D>("MenuButtonStart"),
        new Rectangle(200, 0, 200, 50));
      ContentSpace.menuButtonExit = new SpriteHelper(content.Load<Texture2D>("MenuButtonExit"), 
        new Rectangle(0, 0, 200, 50));
      ContentSpace.menuHiButtonExit = new SpriteHelper(content.Load<Texture2D>("MenuButtonExit"),
        new Rectangle(200, 0, 200, 50));
      ContentSpace.menuButtonHelp = new SpriteHelper(content.Load<Texture2D>("MenuButtonHelp"), 
        new Rectangle(0, 0, 200, 50));
      ContentSpace.menuHiButtonHelp = new SpriteHelper(content.Load<Texture2D>("MenuButtonHelp"),
        new Rectangle(200, 0, 200, 50));
      ContentSpace.menuButtonHiScore = new SpriteHelper(content.Load<Texture2D>("MenuButtonHiScore"),
        new Rectangle(0, 0, 200, 50));
      ContentSpace.menuHiButtonHiScore = new SpriteHelper(content.Load<Texture2D>("MenuButtonHiScore"),
        new Rectangle(200, 0, 200, 50));
      ContentSpace.menuButtonAuthors = new SpriteHelper(content.Load<Texture2D>("MenuButtonAuthors"),
        new Rectangle(0, 0, 200, 50));
      ContentSpace.menuHiButtonAuthors = new SpriteHelper(content.Load<Texture2D>("MenuButtonAuthors"),
        new Rectangle(200, 0, 200, 50));
      menu = new Menu(this, new Rectangle(0, 0, 1024, 768),
        new SpriteHelper(content.Load<Texture2D>("MenuBackground"), null));
      Components.Add(menu);

      // Create all sprites
      background = new SpriteHelper(backgroundTexture, null);
      backgroundSmallBox = new SpriteHelper(backgroundSmallBoxTexture, null);
      backgroundBigBox = new SpriteHelper(backgroundBigBoxTexture, null);
      buttonPause = new SpriteHelper(buttonPauseTexture, null);
      buttonExit = new SpriteHelper(buttonExitTexture, null);

      // Create interface elements
      btnPause = new Button(this, rectPauseButton, buttonPause);
      btnPause.ButtonAction += btnPause_ButtonAction;
      btnExit = new Button(this, rectExitButton, buttonExit);
      btnExit.ButtonAction += btnExit_ButtonAction;
      Components.Add(btnPause);
      Components.Add(btnExit);

      base.LoadContent();
    } // LoadGraphicsContent(loadAllContent)

    #endregion

    #region Update

    protected override void Update(GameTime gameTime)
    {
      int frameMs = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
      elapsedGameMs += frameMs;

      if (Input.KeyboardSpaceJustPressed)
        SetPauseUnpause();

      if (GameState == Serv.GameState.GameStateRunning)
        Timer -= frameMs;
      if (GameState == Serv.GameState.GameStateRunning)
        CheckForLoose();

      base.Update(gameTime);
    }// Update(gameTime)

    #endregion

    #region Draw

    protected override void Draw(GameTime gameTime)
    {
      if (GameState == Serv.GameState.GameStateMenu)
      {
        menu.Draw(gameTime);
      }

      else
      {
        // Render background
        background.Render();

        // Draw background boxes for all the components
        backgroundBigBox.Render(new Rectangle(300, 25, 720, 720));
        backgroundSmallBox.Render(new Rectangle(25, 25, 260, 720));

        if (GameState == Serv.GameState.GameStateRunning || GameState == Serv.GameState.GameStatePause)
          blocksGrid.Draw(gameTime);

        btnPause.Draw(gameTime);
        btnExit.Draw(gameTime);

        TextureFont.WriteText(40, 50, String.Format("Score: {0}", Score));
        TextureFont.WriteText(40, 90, String.Format("Remain: {0}", 
          Math.Max(currentLevel.maxScore - Score, 0)));
        TextureFont.WriteText(40, 140, currentLevel.LevelString);
        TextureFont.WriteText(40, 180, Serv.GetTimeString(Timer));

        if (GameState == Serv.GameState.GameStatePause)
          TextureFont.WriteText(610, 370, "PAUSE", Color.AliceBlue);
      }

      base.Draw(gameTime);
    }

    #endregion

    private void CheckForLoose()
    {
      if (Timer <= 0)
      {
        if (Score <= currentLevel.maxScore)
          ExitToMenu();
        else
        {
          GameState = Serv.GameState.GameStateLevelEnd;
          StartNextLevel();
        }
      }
    }

    private void ExitToMenu()
    {
      Score = 0;
      Timer = 0;
      curLevelNumber = 0;
      menu.EnableComponents(true);
      blocksGrid.EnableComponents(false);
      GameState = Serv.GameState.GameStateMenu;
    }

    private void SetPauseUnpause()
    {
      if (GameState == Serv.GameState.GameStateRunning)
        PauseGame();
      else if (GameState == Serv.GameState.GameStatePause)
        RunGame();
    }

    private void PauseGame()
    {
      GameState = Serv.GameState.GameStatePause;
    }

    private void RunGame()
    {
      GameState = Serv.GameState.GameStateRunning;
    }

    private void StartNextLevel()
    {
      curLevelNumber++;
      currentLevel = LevelGenerator.GetLevel(curLevelNumber);
      blocksGrid.Restart();
      GameState = Serv.GameState.GameStateRunning;
      Timer = currentLevel.time;
    }

    #region Interface Events

    void btnExit_ButtonAction(object sender, EventArgs e)
    {
      ExitToMenu();
    }

    void btnPause_ButtonAction(object sender, EventArgs e)
    {
      SetPauseUnpause();
    }

    #endregion

    #region Start game

    public static void StartGame()
    {
      using (LinesGame game = new LinesGame())
      {
        game.Run();
      } // using
    } // StartGame()

    #endregion

    internal void Start()
    {
      GameState = Serv.GameState.GameStateRunning;
      Score = 0;
      menu.EnableComponents(false);
      blocksGrid.EnableComponents(true);
      StartNextLevel();
    }
  }
}
