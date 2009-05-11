using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using PingPongLive.Helpers;

namespace PingPongLive
{
  public class PingPongLive : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    SpriteFont Arial;
    private int lineHeight = 20;
    private NetworkHelper networkHelper;


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
      Arial = Content.Load<SpriteFont>("Arial");

    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      // регилка
      if (Keyboard.GetState().IsKeyDown(Keys.F1))
        networkHelper.SignInGamer();

      // Создание сессии
      if (Keyboard.GetState().IsKeyDown(Keys.F2))
        networkHelper.CreateSession();

      // СИнхронный поиск сессии
      if (Keyboard.GetState().IsKeyDown(Keys.F3))
        networkHelper.FindSession();

      // Асинхронный поиск сессии
      if (Keyboard.GetState().IsKeyDown(Keys.F4))
        networkHelper.AsyncFindSession();

      if (networkHelper.SessionState == NetworkSessionState.Playing)
      {
        // Отправляем нажатые клавиши удаленному игроку
        foreach (Keys key in Keyboard.GetState().GetPressedKeys())
          networkHelper.SendMessage(key.ToString());

        // Получаем клавиши от удаленного игрока
        networkHelper.ReceiveMessage();
      }

      networkHelper.Update();

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin();

      spriteBatch.DrawString(Arial, "Game State: " + networkHelper.Message, new Vector2(20, 20), Color.Yellow);
      spriteBatch.DrawString(Arial, "Press:", new Vector2(20, 100), Color.Snow);
      spriteBatch.DrawString(Arial, " - F1 to sign in", new Vector2(20, 120), Color.Snow);
      spriteBatch.DrawString(Arial, " - F2 to create a session", new Vector2(20, 140), Color.Snow);
      spriteBatch.DrawString(Arial, " - F3 to find a session", new Vector2(20, 160), Color.Snow);
      spriteBatch.DrawString(Arial, " - F4 to asynchronously find a session",
        new Vector2(20, 180), Color.Snow);
      spriteBatch.DrawString(Arial, @"After the game starts, press other keys to send messages", 
        new Vector2(20, 220), Color.Snow);

      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
