using System;
using Microsoft.Win32;
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

    // for network game
    private readonly NetworkHelper networkHelper;
    private const int maxLocalPlayers = 1;
    private const int maxSessionPlayers = 2;


    private GameScene activeScene;
    private MenuScene menuScene;
    private ActionScene actionScene;
    private NetworkScene networkScene;

    private KeyboardState oldKeyboardState;

    private Texture2D startBackgroundTexture, actionBackgroundTexture, networkBackgroundTexture, gameTexture;

    #region Constructor

    public PingPongLive()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      Components.Add(new GamerServicesComponent(this));

      networkHelper = new NetworkHelper();
      Services.AddService(typeof(NetworkHelper), networkHelper);
    }

    #endregion

    #region LoadContent

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

      actionScene = new ActionScene(this, actionBackgroundTexture, gameTexture, ArialBig);
      Components.Add(actionScene);
      actionScene.Hide();

      networkBackgroundTexture = Content.Load<Texture2D>("NetworkBackground");
      networkScene = new NetworkScene(this, Arial, ArialBig, networkBackgroundTexture);
      Components.Add(networkScene);

      activeScene = menuScene;
    }

    #endregion

    protected override void UnloadContent()
    {
    }

    #region Update

    protected override void Update(GameTime gameTime)
    {
      if (!Guide.IsVisible)
        HandleScenesInput();

      if (networkHelper.NetworkGameSession != null)
      {
        // Только отправляем, если мы не сервер.
        // Нет никакого смысла отправлять пакеты самому себе, поскольку мы уже знаем, что они будут содержать!
        if (!networkHelper.NetworkGameSession.IsHost)
        {
          networkHelper.SendClientData();
        }
        else
        {
          // Если мы сервер, передаем состояние игры
          networkHelper.SendServerData();
        }

        // Проталкиваем данные
        networkHelper.NetworkGameSession.Update();

        // Читаем любые входящие сетевые пакеты
        foreach (LocalNetworkGamer gamer in networkHelper.NetworkGameSession.LocalGamers)
        {
          // Продолжаем чтение, пока есть входящие пакеты.
          while (gamer.IsDataAvailable)
          {
            NetworkGamer sender;

            if (gamer.IsHost)
            {
              sender = networkHelper.ReadClientData(gamer);
              if (!sender.IsLocal)
              {
                actionScene.HandleClientData();
              }
            }
            else
            {
              sender = networkHelper.ReadServerData(gamer);
              if (!sender.IsLocal)
              {
                actionScene.HandleServerData();
              }
            }
          }
        }
      }

      base.Update(gameTime);
    }

    private void HandleScenesInput()
    {
      // Обработка ввода начальной сцены
      if (activeScene == menuScene)
        HandleMenuSceneInput();
      // Обработка ввода для сцены игры
      else if (activeScene == actionScene)
        HandleActionInput();
      else
        // Обработка ввода сетевой сцены
        HandleNetworkSceneInput();
    }

    #region Network Handles
    private void HandleNetworkSceneInput()
    {
      if (CheckEnter())
      {
        if (Gamer.SignedInGamers.Count == 0)
          HandleNotSigned();
        else
          HandleSigned();
      }
    }

    private void HandleNotSigned()
    {
      switch (networkScene.SelectedMenuIndex)
      {
        case 0:
          if (!Guide.IsVisible)
          {
            Guide.ShowSignIn(1, false);
            break;
          }
          break;
        case 1:
          ShowScene(menuScene);
          break;
      }
    }

    private void HandleSigned()
    {
      switch (networkScene.State)
      {
        case NetworkGameState.Idle:
          switch (networkScene.SelectedMenuIndex)
          {
            case 0:
              // Присоединение к сетевой игре
              JoinSession();
              break;
            case 1:
              // Создание сетевой игры
              CreateSession();
              break;
            case 2:
              // Отображение мастера для
              // смены пользователя
              if (!Guide.IsVisible)
              {
                Guide.ShowSignIn(1, false);
                break;
              }
              break;
            case 3:
              // Возврат к начальной сцене
              ShowScene(menuScene);
              break;
          }
          break;
        case NetworkGameState.Creating:
          // Закрытие созданной сессии
          CloseSession();

          // Ожидание новой команды
          networkScene.State = NetworkGameState.Idle;
          networkScene.Message = "";
          break;
      }
    }

    private void CreateSession()
    {
      networkHelper.NetworkGameSession = NetworkSession.Create(NetworkSessionType.SystemLink,
                                    maxLocalPlayers, maxSessionPlayers);
      HookSessionEvents();
      networkScene.State = NetworkGameState.Creating;
      networkScene.Message = "Waiting another player...";
    }

    private void CloseSession()
    {
      networkHelper.NetworkGameSession.Dispose();
      networkHelper.NetworkGameSession = null;
    }

    void JoinSession()
    {
      networkScene.Message = "Joining a game...";
      networkScene.State = NetworkGameState.Joining;

      try
      {
        // Поиск сессий.
        using (AvailableNetworkSessionCollection availableSessions =
                  NetworkSession.Find(NetworkSessionType.SystemLink, maxLocalPlayers, null))
        {
          if (availableSessions.Count == 0)
          {
            networkScene.Message = "No network sessions found.";
            networkScene.State = NetworkGameState.Idle;
            return;
          }

          // Присоединение к первой найденной сессии.
          networkHelper.NetworkGameSession = NetworkSession.Join(availableSessions[0]);
          HookSessionEvents();
        }
      }
      catch (Exception e)
      {
        networkScene.Message = e.Message;
        networkScene.State = NetworkGameState.Idle;
      }
    }

    private void HookSessionEvents()
    {
      networkHelper.NetworkGameSession.GamerJoined += GamerJoinedEventHandler;
      networkHelper.NetworkGameSession.SessionEnded += SessionEndedEventHandler;
    }

    private void SessionEndedEventHandler(object sender, NetworkSessionEndedEventArgs e)
    {
      networkScene.Message = e.EndReason.ToString();
      networkScene.State = NetworkGameState.Idle;

      CloseSession();

      if (activeScene != networkScene)
      {
        ShowScene(networkScene);
      }
    }

    void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
    {
      if (actionScene.Player1.Gamer == null)
        actionScene.Player1.Gamer = e.Gamer;
      else
        actionScene.Player2.Gamer = e.Gamer;

      if (networkHelper.NetworkGameSession.AllGamers.Count == maxSessionPlayers)
      {
        ShowScene(actionScene);
      }
    }

    #endregion

    private void HandleActionInput()
    {
      if (actionScene.IsGameOver)
      {
        ShowScene(menuScene);
      }
      else if (CheckKey(Keys.Space))
      {
        actionScene.Paused = !actionScene.Paused;

        // Отправляем команду паузы другому игроку
        if (networkHelper.NetworkGameSession != null)
        {
          // Если мы сервер, отправляем используя пакеты сервера
          if (networkHelper.NetworkGameSession.IsHost)
          {
            networkHelper.ServerPacketWriter.Write('P');
            networkHelper.ServerPacketWriter.Write(actionScene.Paused);
          }
          else
          {
            networkHelper.ClientPacketWriter.Write('P');
            networkHelper.ClientPacketWriter.Write(actionScene.Paused);
          }
        }
      }

      if (CheckKey(Keys.Back))
      {
        if (networkHelper.NetworkGameSession != null)
        {
          CloseSession();
          networkScene.State = NetworkGameState.Idle;
          networkScene.Message = "";
          ShowScene(networkScene);
        }
        else
        {
          ShowScene(menuScene);
        }
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
            ShowScene(networkScene);
            break;
          case 3:
            // help
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

      bool result = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));

      oldKeyboardState = keyboardState;
      return result;
    }

    private bool CheckKey(Keys key)
    {
      KeyboardState keyboardState = Keyboard.GetState();

      bool result = (oldKeyboardState.IsKeyDown(key) && (keyboardState.IsKeyUp(key)));

      oldKeyboardState = keyboardState;
      return result;
    }
    #endregion


    #region Draw

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin();
      base.Draw(gameTime);
      spriteBatch.End();
    }

    #endregion
  }
}
