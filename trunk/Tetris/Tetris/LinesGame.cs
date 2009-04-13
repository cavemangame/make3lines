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
    public const int PENALTY_FOR_WRONG_SWAP = 5000;
    public const int PENALTY_FOR_RESTART = 5000;
    private readonly Rectangle rectPauseButton = new Rectangle(55, 600, 200, 50);
    private readonly Rectangle rectExitButton = new Rectangle(55, 670, 200, 50);
    #endregion

    #region Variables

    private readonly BlocksGrid blocksGrid;
    private Menu menu;


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
    public Serv.GameState GameState { get; private set; }
    public SpriteFont NormalFont { get; private set; }
    public SpriteFont BigFont { get; private set; }
    public SpriteFont SmallFont { get; private set; }

    public double ElapsedGameMs { get; private set; }

    /// <summary>
    /// true if the game has just found lines
    /// </summary>
    public bool IsRemoveProcess { get; set; }
    public bool IsRestartProcess { get; set; }
    #endregion

    #region Constructor

    public LinesGame()
    {
      blocksGrid = new BlocksGrid(this, new Rectangle(GRID_RECTANGLE_X_COORDINATE, GRID_RECTANGLE_Y_COORDINATE,
                                                      GRID_RECTANGLE_WIDTH, GRID_RECTANGLE_HEIGHT));
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
      ContentSpace.background = new SpriteHelper(content.Load<Texture2D>("skybackground"), null);
      ContentSpace.backgroundSmallBox = new SpriteHelper(content.Load<Texture2D>("BackgroundSmallBox"), null);
      ContentSpace.backgroundBigBox = new SpriteHelper(content.Load<Texture2D>("BackgroundBigBox"), null);
      ContentSpace.buttonPause = new SpriteHelper(content.Load<Texture2D>("PauseButton"), null);
      ContentSpace.buttonExit = new SpriteHelper(content.Load<Texture2D>("ExitButton"), null);

      NormalFont = content.Load<SpriteFont>("normalfont");
      BigFont = content.Load<SpriteFont>("bigfont");
      SmallFont = content.Load<SpriteFont>("smallfont");

      // Create interface elements
      btnPause = new Button(this, rectPauseButton, ContentSpace.buttonPause);
      btnPause.ButtonAction += btnPause_ButtonAction;
      btnExit = new Button(this, rectExitButton, ContentSpace.buttonExit);
      btnExit.ButtonAction += btnExit_ButtonAction;

      base.LoadContent();
    }

    #endregion

    #region Update
    protected override void Update(GameTime gameTime)
    {
      if (IsBoardInStableState())
      {
        long frameMs = (long)gameTime.ElapsedGameTime.TotalMilliseconds;

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
      ElapsedGameMs = gameTime.TotalRealTime.TotalMilliseconds;

      if (GameState == Serv.GameState.GameStateMenu)
      {
        menu.Draw(gameTime);
      }

      else
      {
        // Render background
        ContentSpace.background.Render();

        // Draw background boxes for all the components
        ContentSpace.backgroundBigBox.Render(new Rectangle(300, 25, 720, 720));
        ContentSpace.backgroundSmallBox.Render(new Rectangle(25, 25, 260, 720));

        if (GameState == Serv.GameState.GameStateRunning || GameState == Serv.GameState.GameStatePause)
          blocksGrid.Draw(gameTime);

        btnPause.Draw(gameTime);
        btnExit.Draw(gameTime);

        TextureFont.WriteText(40, 50, String.Format("Score: {0}", Score));
        TextureFont.WriteText(40, 90, String.Format("Remain: {0}", 
          Math.Max(currentLevel.maxScore - Score, 0)));
        TextureFont.WriteText(40, 140, currentLevel.LevelString);
        TextureFont.WriteText(40, 180, Serv.GetTimeString(Timer));

        //TextHelper.DrawText(NormalFont, String.Format("Pos {0},{1}", Input.MousePos.X, Input.MousePos.Y), 40, 220, Color.SteelBlue, 0.76f);

        if (GameState == Serv.GameState.GameStatePause)
          TextureFont.WriteText(610, 370, "PAUSE", Color.AliceBlue);
      }

      base.Draw(gameTime);
    }

    #endregion

    #region Loose and win level

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

    private void StartNextLevel()
    {
      curLevelNumber++;
      currentLevel = LevelGenerator.GetLevel(curLevelNumber);
      GameState = Serv.GameState.GameStateRunning;
      Timer = currentLevel.time;
      blocksGrid.Restart();
    }

    #endregion

    #region Interface Events

    void btnExit_ButtonAction(object sender, EventArgs e)
    {
      ExitToMenu();
    }

    void btnPause_ButtonAction(object sender, EventArgs e)
    {
      SetPauseUnpause();
    }

    internal void Start()
    {
      GameState = Serv.GameState.GameStateRunning;
      Score = 0;
      menu.EnableComponents(false);
      Components.Add(blocksGrid);
      Components.Add(btnPause);
      Components.Add(btnExit);
      StartNextLevel();
    }

    private void ExitToMenu()
    {
      Score = 0;
      Timer = 0;
      curLevelNumber = 0;
      menu.EnableComponents(true);
      Components.Remove(blocksGrid);
      Components.Remove(btnPause);
      Components.Remove(btnExit);
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


    #region Some help methods

    /// <summary>
    /// Determines if there are moving blocks on the board or not
    /// </summary>
    /// <returns>true if there are no moving blocks</returns>
    public bool IsBoardInStableState()
    {
      return blocksGrid.ActiveBlocks == 0 && !IsRemoveProcess && !IsRestartProcess;
    }

    /// <summary>
    /// It's time to remove them all
    /// </summary>
    /// <param name="gameTime"></param>
    public void RemoveVisualizationWasFinished(GameTime gameTime)
    {
      if (IsRemoveProcess)
      {
        IsRemoveProcess = false;
        blocksGrid.RemoveLines(gameTime);
      }
      if (IsRestartProcess)
      {
        IsRestartProcess = false;
        blocksGrid.ReadyToRestart();
      }
    }

    #endregion
  }
}
