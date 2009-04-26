#region Using directive

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Sounds;
using XnaTetris.Helpers;

#endregion

namespace XnaTetris.Game
{
  /// <summary>
  /// Base game class for all the basic game support.
  /// Connects all our helper classes together and makes our live easier!
  /// </summary>
  public class BaseGame : Microsoft.Xna.Framework.Game
  {
    #region Variables
    protected GraphicsDeviceManager graphics;
    protected ContentManager content;

    /// <summary>
    /// Font for rendering text
    /// </summary>
    TextureFont font;

    private TextHelper textHelper;
    #endregion

    #region Properties

    /// <summary>
    /// Resolution of our game
    /// </summary>
    public static int Width { get; private set; }
    public static int Height { get; private set; }

    #endregion

    #region Constructor
    public BaseGame()
    {
      graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = 800;
      graphics.PreferredBackBufferHeight = 600;
      graphics.IsFullScreen = false;
      Window.AllowUserResizing = true;

      graphics.ApplyChanges();

      content = new ContentManager(Services);
      Content.RootDirectory = "Content";

    } // BaseGame

    protected override void Initialize()
    {
      // Remember resolution

      Width = graphics.GraphicsDevice.Viewport.Width;
      Height = graphics.GraphicsDevice.Viewport.Height;

      base.Initialize();
    } // Initialize()



    protected override void LoadContent()
    {
      font = new TextureFont(graphics.GraphicsDevice, content);
      textHelper = new TextHelper(graphics.GraphicsDevice);
      base.LoadContent();
    } // LoadContent


    protected override void UnloadContent()
    {
      content.Unload();
      SpriteHelper.Dispose();

      base.UnloadContent();
    } // UnloadContent
    #endregion

    #region Update
    protected override void Update(GameTime gameTime)
    {
      Sound.Update();
      Input.Update();

      if (Input.KeyboardEscapeJustPressed ||
          Input.GamePadBackJustPressed)
      {
        Exit();
      }

      base.Update(gameTime);
    } // Update(gameTime)
    #endregion

    #region Draw
    protected override void Draw(GameTime gameTime)
    {
      // Draw all sprites and fonts
      SpriteHelper.DrawSprites();
      font.WriteAll();
      textHelper.WriteAll();
      base.Draw(gameTime);
    } // Draw(gameTime)
    #endregion
  } // class BaseGame
} // namespace XnaTetris.Game
