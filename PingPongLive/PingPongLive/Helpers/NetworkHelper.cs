using System;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;

namespace PingPongLive.Helpers
{
  class NetworkHelper
  {
    #region Variables

    private NetworkSession session; // Игровая сессия
    private const int maximumGamers = 2;
    private const int maximumLocalPlayers = 2;
    private IAsyncResult AsyncSessionFind; //асинхронный финдер для сессии

    PacketWriter packetWriter = new PacketWriter();
    PacketReader packetReader = new PacketReader();

    #endregion

    #region Properties

    public string Message { get; private set; }
    public NetworkSessionState SessionState
    {
      get
      {
        if (session == null)
          return NetworkSessionState.Ended;
        else
          return session.SessionState;
      }
    }

    #endregion

    public NetworkHelper()
    {
      Message = "Waiting for user command...";
    }

    // возможность законнектиться в LIVE
    public void SignInGamer()
    {
      if (!Guide.IsVisible)
      {
        Guide.ShowSignIn(1, false);
      }
    }

    // создание сессии
    public void CreateSession()
    {
      if (session == null)
      {
        // по локалочке; 2 игрока, один из которых локальный (сервак)
        session = NetworkSession.Create(NetworkSessionType.SystemLink,
                                        maximumLocalPlayers,
                                        maximumGamers);

        // Если главный узел отключается, новым главным узлом становится другая машина
        session.AllowHostMigration = true;

        // Игрокам НЕ разрешено подключаться в ходе игры
        session.AllowJoinInProgress = true;

        session.GamerJoined +=
              new EventHandler<GamerJoinedEventArgs>(session_GamerJoined);
        session.GamerLeft +=
              new EventHandler<GamerLeftEventArgs>(session_GamerLeft);
        session.GameStarted +=
              new EventHandler<GameStartedEventArgs>(session_GameStarted);
        session.GameEnded +=
              new EventHandler<GameEndedEventArgs>(session_GameEnded);
        session.SessionEnded +=
              new EventHandler<NetworkSessionEndedEventArgs>(session_SessionEnded);
        session.HostChanged +=
              new EventHandler<HostChangedEventArgs>(session_HostChanged);

      }
    }

    public void FindSession()
    {
      // Все найденные сессии
      AvailableNetworkSessionCollection availableSessions;

      // Сессия для подключения
      AvailableNetworkSession availableSession = null;
      availableSessions = NetworkSession.Find(NetworkSessionType.SystemLink,
                                              maximumLocalPlayers, null);

      // Получаем сессию с доступными игровыми слотами
      foreach (AvailableNetworkSession curSession in availableSessions)
      {
        int TotalSessionSlots = curSession.OpenPublicGamerSlots +
                                curSession.OpenPrivateGamerSlots;
        if (TotalSessionSlots > curSession.CurrentGamerCount)
          availableSession = curSession;
      }

      // Если сессия найдена, подключаемся к ней
      if (availableSession != null)
      {
        Message = "Found an available session at host " +
                  availableSession.HostGamertag;
        session = NetworkSession.Join(availableSession);
      }
      else
        Message = "No sessions found!";
    }


    public void AsyncFindSession()
    {
      Message = "Asynchronous search started!";
      if (AsyncSessionFind == null)
      {
        AsyncSessionFind = NetworkSession.BeginFind(
            NetworkSessionType.SystemLink, maximumLocalPlayers, null,
            new AsyncCallback(session_SessionFound), null);
      }
    }

    public void SetPlayerReady()
    {
      foreach (LocalNetworkGamer gamer in session.LocalGamers)
        gamer.IsReady = true;
    }

    public void SendMessage(string key)
    {
      foreach (LocalNetworkGamer localPlayer in session.LocalGamers)
      {
        packetWriter.Write(key);
        localPlayer.SendData(packetWriter, SendDataOptions.None);
        Message = "Sending message: " + key;
      }
    }

    public void ReceiveMessage()
    {
      NetworkGamer remotePlayer; // Отправитель сообщения
      foreach (LocalNetworkGamer localPlayer in session.LocalGamers)
      {
        // Читаем, пока есть доступные данные
        while (localPlayer.IsDataAvailable)
        {
          localPlayer.ReceiveData(packetReader, out remotePlayer);

          // Игнорируем ввод от локального игрока
          if (!remotePlayer.IsLocal)
            Message = "Received message: " + packetReader.ReadString();
        }
      }
    }



    #region Events

    private void session_GamerJoined(object sender, GamerJoinedEventArgs e)
    {
      if (e.Gamer.IsHost)
      {
        Message = "The Host started the session!";
      }
      else
      {
        Message = "Gamer " + e.Gamer.Tag + " joined the session!";

        // Другой игрок присоединился, начинаем игру
        session.StartGame();
      }

    }

    private void session_GamerLeft(object sender, GamerLeftEventArgs e)
    {
      Message = "Gamer " + e.Gamer.Tag + " left the session!";
    }

    private void session_GameStarted(object sender, GameStartedEventArgs e)
    {
      Message = "Game Started";
    }

    private void session_HostChanged(object sender, HostChangedEventArgs e)
    {
      Message = "Host changed from " + e.OldHost.Tag + " to " + e.NewHost.Tag;
    }

    private void session_SessionEnded(object sender, NetworkSessionEndedEventArgs e)
    {
      Message = "The session has ended";
    }

    private void session_GameEnded(object sender, GameEndedEventArgs e)
    {
      Message = "Game Over";
    }

    public void session_SessionFound(IAsyncResult result)
    {
      // Все найденные сессии
      AvailableNetworkSessionCollection availableSessions;

      // Сессия, к которой будем подключаться
      AvailableNetworkSession availableSession = null;

      if (AsyncSessionFind.IsCompleted)
      {
        availableSessions = NetworkSession.EndFind(result);

        // Ищем сессии с доступными слотами для игроков
        foreach (AvailableNetworkSession curSession in
                 availableSessions)
        {
          int TotalSessionSlots = curSession.OpenPublicGamerSlots +
                                  curSession.OpenPrivateGamerSlots;
          if (TotalSessionSlots > curSession.CurrentGamerCount)
            availableSession = curSession;
        }

        // Если сессия найдена, подключаемся к ней
        if (availableSession != null)
        {
          Message = "Found an available session at host" +
                    availableSession.HostGamertag;
          session = NetworkSession.Join(availableSession);
        }
        else
          Message = "No sessions found!";

        // Сбрасываем результаты поиска сессий
        AsyncSessionFind = null;
      }
    }


    #endregion

    #region Update

    public void Update()
    {
      // обновим сессию
      if (session != null)
        session.Update();
    }

    #endregion
  }
}
