using Microsoft.Xna.Framework;
using XnaTetris.Helpers;

namespace XnaTetris.Particles
{
  public class Particle
  {
    #region Properties

    public Vector2 Position { get; set; } 
    public Vector2 Velocity { get; set; } //скорость
    public Vector2 Acceleration { get; set; } //ускорение

    public float LifeTime { get; set; } // время жизни частицы
    public float TimeSinceStart { get; set; } // время, прошедшее с начала жизни частицы
   
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public float RotationSpeed { get; set; }

    public bool Active // еще жива, моя частица?
    {
      get { return TimeSinceStart < LifeTime; }
    }

    #endregion

    #region Init

    public void Initialize(Vector2 position, Vector2 velocity, Vector2 acceleration,
           float lifetime, float scale, float rotationSpeed)
    {
      Position = position;
      Velocity = velocity;
      Acceleration = acceleration;
      LifeTime = lifetime;
      Scale = scale;
      RotationSpeed = rotationSpeed;

      TimeSinceStart = 0.0f;

      // set rotation to some random value between 0 and 360 degrees.
      Rotation = RandomHelper.GetRandomFloat(0, MathHelper.TwoPi);
    }

    #endregion

    #region Update

    public void Update(float dt)
    {
      Velocity += Acceleration * dt;
      Position += Velocity * dt;
      Rotation += RotationSpeed * dt;

      TimeSinceStart += dt;
    }

    #endregion
  }
}
