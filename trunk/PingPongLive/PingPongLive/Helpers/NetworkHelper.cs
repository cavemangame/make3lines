using System;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;

namespace PingPongLive.Helpers
{
  class NetworkHelper
  {
    #region Variables

    private NetworkSession networkSession;

    private readonly PacketWriter serverPacketWriter = new PacketWriter();
    private readonly PacketReader serverPacketReader = new PacketReader();

    private readonly PacketWriter clientPacketWriter = new PacketWriter();
    private readonly PacketReader clientPacketReader = new PacketReader();

    #endregion

    #region Properties

    /// <summary>
    /// Активная сетевая сессия
    /// </summary>
    public NetworkSession NetworkGameSession
    {
      get { return networkSession; }

      set { networkSession = value; }
    }

    /// <summary>
    /// Писатель для данных сервера
    /// </summary>
    public PacketWriter ServerPacketWriter
    {
      get { return serverPacketWriter; }
    }

    /// <summary>
    /// Писатель для данных клиента
    /// </summary>
    public PacketWriter ClientPacketWriter
    {
      get { return clientPacketWriter; }
    }

    /// <summary>
    /// Читатель для данных клиента
    /// </summary>
    public PacketReader ClientPacketReader
    {
      get { return clientPacketReader; }
    }

    /// <summary>
    /// Читатель для данных сервера
    /// </summary>
    public PacketReader ServerPacketReader
    {
      get { return serverPacketReader; }
    }

    #endregion

    /// <summary>
    /// Отправляем все данные сервера
    /// </summary>
    public void SendServerData()
    {
      if (ServerPacketWriter.Length > 0)
      {
        // Отправляем объединенные данные каждому в сессии.
        LocalNetworkGamer server = (LocalNetworkGamer)networkSession.Host;
        server.SendData(ServerPacketWriter, SendDataOptions.InOrder);
      }
    }

    /// <summary>
    /// Чтение данных сервера
    /// </summary>
    public NetworkGamer ReadServerData(LocalNetworkGamer gamer)
    {
      NetworkGamer sender;

      // Читаем единственный пакет из сети.
      gamer.ReceiveData(ServerPacketReader, out sender);

      return sender;
    }

    /// <summary>
    /// Отправка всех данных клиента
    /// </summary>
    public void SendClientData()
    {
      if (ClientPacketWriter.Length > 0)
      {
        // Первый игрок всегда запущен на сервере...
        networkSession.LocalGamers[0].SendData(clientPacketWriter, SendDataOptions.InOrder, networkSession.Host);
      }
    }

    /// <summary>
    /// Чтение данных клиента
    /// </summary>
    public NetworkGamer ReadClientData(LocalNetworkGamer gamer)
    {
      NetworkGamer sender;

      // Читаем единственный пакет из сети.
      gamer.ReceiveData(ClientPacketReader, out sender);

      return sender;
    }

  }
}
