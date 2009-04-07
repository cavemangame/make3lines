using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;
using XnaTetris.Game;
using XnaTetris.Algorithms;
using XnaTetris.Interface;

namespace XnaTetris
{
  public class LinesGame : BaseGame
  {
    #region Constants
    public const int GRID_RECTANGLE_X_COORDINATE = 310;
    public const int GRID_RECTANGLE_Y_COORDINATE = 35;
    public const int GRID_RECTANGLE_HEIGHT = 700;
    public const int GRID_RECTANGLE_WIDTH = 700;
    private readonly Rectangle rectPauseButton = new Rectangle(55, 600, 200, 50);
    private readonly Rectangle rectExitButton = new Rectangle(55, 670, 200, 50);
    #endregion

    #region Variables

    private readonly BlocksGrid blocksGrid;
    private Menu menu;

    // graphics
    SpriteHelper background, backgroundSmallBox, backgroundBigBox, buttonPause, buttonExit;

    private Level currentLevel;
    private int curLevelNumber;

    /// <summary>
    /// Interface
    /// </summary>
    private Button btnPause, btnExit;

    #endregion

    #region Properties
    public long Timer { get; set; }
    public int Score { get; set; }
    public Serv.GameState GameState { get; set; }
    public SpriteFont NormalFont { get; set; }

    /// <summary>
    /// true if the game has just found lines
    /// </summary>
    public bool IsRemoveProcess { get; set; }
    #endregion

    #region Constructor

    public LinesGame()
    {
      blocksGrid = new BlocksGrid(this, new Rectangle(GRID_RECTANGLE_X_COORDINATE, GRID_RECTANGLE_Y_COORDINATE,
                                                      GRID_RECTANGLE_WIDTH, GRID_RECTANGLE_HEIGHT));
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

      menu = new Menu(this, new Rectangle(0, 0, 1024, 768),
        new SpriteHelper(content.Load<Texture2D>("MenuBackground"), null));
      Components.Add(menu);

      // Create all sprites
      background = new SpriteHelper(content.Load<Texture2D>("skybackground"), null);
      backgroundSmallBox = new SpriteHelper(content.Load<Texture2D>("BackgroundSmallBox"), null);
      backgroundBigBox = new SpriteHelper(content.Load<Texture2D>("BackgroundBigBox"), null);
      buttonPause = new SpriteHelper(content.Load<Texture2D>("PauseButton"), null);
      buttonExit = new SpriteHelper(content.Load<Texture2D>("ExitButton"), null);

      NormalFont = content.Load<SpriteFont>("normalfont");

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
      if (IsBoardInStableState())
      {
        int frameMs = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (Input.KeyboardSpaceJustPressed)
        {
          SetPauseUnpause();
        }

        if (GameState == Serv.GameState.GameStateRunning)
        {
          Timer -= frameMs;
        }
        if (GameState == Serv.GameState.GameStateRunning)
        {
          CheckForLoose();
        }
      }

      base.Update(gameTime);
    }
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

        TextHelper.DrawText(NormalFont, "FACK OFF", 40, 220);

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
      }
    }

    #endregion

    internal void Start()
    {
      GameState = Serv.GameState.GameStateRunning;
      Score = 0;
      menu.EnableComponents(false);
      blocksGrid.EnableComponents(true);
      StartNextLevel();
    }

    /// <summary>
    /// Determines if there are moving blocks on the board or not
    /// </summary>
    /// <returns>true if there are no moving blocks</returns>
    public bool IsBoardInStableState()
    {
      return blocksGrid.ActiveBlocks == 0 && !IsRemoveProcess;
    }

    /// <summary>
    /// It's time to remove lines
    /// </summary>
    /// <param name="gameTime"></param>
    public void RemoveVisualizationWasFinished(GameTime gameTime)
    {
      IsRemoveProcess = false;
      blocksGrid.RemoveLines(gameTime);
    }
  }
}
