using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PingPongLive.Helpers;

namespace PingPongLive.GameInterface
{
  class Ball : DrawableGameComponent
  {
    #region Variables

    private SpriteBatch spriteBatch;
    private Texture2D tex;
    private Rectangle spriteRect;
    private Rectangle boundRect;
    private readonly NetworkHelper networkHelper;

    #endregion

    #region Properties

    public Vector2 Position;
    public float Speed { get; set; }
    public Vector2 SpeedVector;

    #endregion

    public Ball(Game game, Texture2D tex, Vector2 pos, Rectangle rect)
      : base(game)
    {
      spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
      this.tex = tex;
      Position = pos;
      spriteRect = rect;
      boundRect = new Rectangle(0, 0, Game.Window.ClientBounds.Width,
                                Game.Window.ClientBounds.Height);
      SpeedVector = new Vector2(0.5f, 0.5f);
      Speed = 5;

      networkHelper = (NetworkHelper)Game.Services.GetService(typeof(NetworkHelper));
    }

    public override void Draw(GameTime gameTime)
    {
      spriteBatch.Draw(tex, Position, spriteRect, Color.White);
      base.Draw(gameTime);
    }

    public void UpdateNetworkData()
    {
      if ((networkHelper.NetworkGameSession == null) || (networkHelper.NetworkGameSession.IsHost))
      {
        networkHelper.ServerPacketWriter.Write('L');
        networkHelper.ServerPacketWriter.Write(Position);
        networkHelper.ServerPacketWriter.Write(Speed);
        networkHelper.ServerPacketWriter.Write(SpeedVector);
      }
    }

    public void MoveBall()
    {
      Position.X += Speed*SpeedVector.X;
      Position.Y += Speed*SpeedVector.Y;
    }

    public void HandleBoundField()
    {
      if (Position.Y >= boundRect.Bottom - 40 || Position.Y <= 10)
      {
        SpeedVector.Y = -SpeedVector.Y;
      }
    }

    public bool HandleLoose()
    {
      if (Position.X <= 0 || Position.X >= boundRect.Right)
        return true;
      return false;
    }

    public void HandleBoundPaddle(Player player)
    {
      Rectangle bbBall = new Rectangle((int)Position.X, (int)Position.Y, spriteRect.Width, spriteRect.Height);
      Rectangle bbPaddle = new Rectangle((int)player.Position.X, (int)player.Position.Y, 
        player.SpriteRect.Width, player.SpriteRect.Height);

      if (bbBall.Intersects(bbPaddle))
      {
        SpeedVector.X = -SpeedVector.X;
        Speed += 0.05f;
      }
    }
  }
}
