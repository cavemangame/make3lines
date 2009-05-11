using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using PingPongLive.GameInterface;
using PingPongLive.Helpers;

namespace PingPongLive
{
  public class PingPongLive : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    SpriteFont Arial, ArialBig;
    private int lineHeight = 20;
    private NetworkHelper networkHelper;

    private GameScene activeScene;
    private MenuScene menuScene;
    private ActionScene actionScene;

    private KeyboardState oldKeyboardState;

    private Texture2D startBackgroundTexture, actionBackgroundTexture, gameTexture;


    public PingPongLive()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      Components.Add(new GamerServicesComponent(this));
      networkHelper = new NetworkHelper();

      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      Services.AddService(typeof(SpriteBatch), spriteBatch);

      Arial = Content.Load<SpriteFont>("Arial");
      ArialBig = Content.Load<SpriteFont>("ArialBig");

      startBackgroundTexture = Content.Load<Texture2D>("startbackground");
      actionBackgroundTexture = Content.Load<Texture2D>("spacebackground");
      gameTexture = Content.Load<Texture2D>("PongGame");

      menuScene = new MenuScene(this, Arial, ArialBig, startBackgroundTexture);
      Components.Add(menuScene);
      menuScene.Show();

      actionScene = new ActionScene(this, actionBackgroundTexture, gameTexture);
      Components.Add(actionScene);
      actionScene.Hide();

      activeScene = menuScene;
    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
      HandleScenesInput();
      base.Update(gameTime);
    }

    private void HandleScenesInput()
    {
      // Обработка ввода начальной сцены
      if (activeScene == menuScene)
      {
        HandleMenuSceneInput();
      }

      // Обработка ввода для сцены игры
      else if (activeScene == actionScene)
      {
        //HandleActionInput();
      }
    }

    private void HandleMenuSceneInput()
    {
      if (CheckEnter())
      {
        switch (menuScene.SelectedMenuIndex)
        {
          case 0:
            ShowScene(actionScene);
            break;
          case 1:
            ShowScene(actionScene);
            break;
          case 2:
            //ShowScene(helpScene);
            break;
          case 3:
            // net
            break;
          case 4:
            Exit();
            break;
        }
      }
    }

    protected void ShowScene(GameScene scene)
    {
      activeScene.Hide();
      activeScene = scene;
      scene.Show();
    }

    private bool CheckEnter()
    {
      KeyboardState keyboardState = Keyboard.GetState();

      bool result = (oldKeyboardState.IsKeyDown(Keys.Enter) &&
                    (keyboardState.IsKeyUp(Keys.Enter)));

      oldKeyboardState = keyboardState;
      return result;
    }


    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin();
      base.Draw(gameTime);
      spriteBatch.End();
    }
  }
}
