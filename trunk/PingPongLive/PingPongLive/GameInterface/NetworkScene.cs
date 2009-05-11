using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace PingPongLive.GameInterface
{
  // Состояния сцены
  public enum NetworkGameState
  {
    Idle = 1,
    Joining = 2,
    Creating = 3
  }

  class NetworkScene : GameScene
  {
    #region Variables 

    protected TextMenuComponent menu;
    private readonly SpriteFont messageFont;
    private Vector2 messagePosition, messageShadowPosition;
    private string message;
    protected TimeSpan elapsedTime = TimeSpan.Zero;

    protected SpriteBatch spriteBatch = null;

    // Состояние сцены
    private NetworkGameState state;

    // Используется для мерцания сообщения
    private bool showMessage = true;

    #endregion

    #region Properties

    public string Message
    {
      get { return message; }

      set
      {
        message = value;

        // Вычисляем местоположение сообщения
        messagePosition = new Vector2();
        messagePosition.X = (Game.Window.ClientBounds.Width - messageFont.MeasureString(message).X) / 2;
        messagePosition.Y = 130;

        // Вычисляем местоположение тени сообщения
        messageShadowPosition = messagePosition;
        messageShadowPosition.Y++;
        messageShadowPosition.X--;
      }
    }

    public int SelectedMenuIndex
    {
      get { return menu.SelectedIndex; }
    }

    public NetworkGameState State
    {
      get { return state; }

      set
      {
        state = value;
        menu.SelectedIndex = 0;
      }
    }

    #endregion

    #region Constructor

    public NetworkScene(Game game, SpriteFont smallFont, SpriteFont largeFont, Texture2D background)
      : base(game)
    {
      messageFont = largeFont;
      Components.Add(new ImageComponent(game, background, DrawMode.Stretch, null));

      menu = new TextMenuComponent(game, smallFont, largeFont);
      Components.Add(menu);

      spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
    }

    #endregion

    public override void Show()
    {
      state = NetworkGameState.Idle;

      base.Show();
    }

    #region Draw

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);

      if (!string.IsNullOrEmpty(message) && showMessage)
      {
        DrawMessage();
      }
    }

    void DrawMessage() // с тенью
    {
      spriteBatch.DrawString(messageFont, message, messageShadowPosition,
                             Color.Black);
      spriteBatch.DrawString(messageFont, message, messagePosition,
                             Color.DarkOrange);
    }

    #endregion

    #region Update

    public override void Update(GameTime gameTime)
    {
      elapsedTime += gameTime.ElapsedGameTime;

      if (elapsedTime > TimeSpan.FromSeconds(1))
      {
        elapsedTime -= TimeSpan.FromSeconds(1);
        showMessage = !showMessage;
      }

      // Устанавливаем меню для текущего состояния
      UpdateMenus();

      base.Update(gameTime);
    }

    private void UpdateMenus()
    {
      if (Gamer.SignedInGamers.Count == 0)
      {
        string[] items = { "Sign in", "Back" };
        menu.SetMenuItems(items);
      }
      else
      {
        if (state == NetworkGameState.Idle)
        {
          string[] items = {"Join a System Link Game", "Create a System Link Game", "Sign out", "Back"};
          menu.SetMenuItems(items);
        }
        if (state == NetworkGameState.Creating)
        {
          string[] items = { "Cancel" };
          menu.SetMenuItems(items);
        }
      }

      // Помещаем меню в центр экрана
      menu.Position = new Vector2((Game.Window.ClientBounds.Width - menu.Width) / 2, 330);
    }

    #endregion

  }
}
