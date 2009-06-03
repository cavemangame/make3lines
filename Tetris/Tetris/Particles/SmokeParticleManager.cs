using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Particles
{
  class SmokeParticleManager : ParticleManager
  {
    public SmokeParticleManager(LinesGame game, int howManyEffects)
      : base(game, howManyEffects)
    {
    }

    protected override void InitializeConstants()
    {
      textureFilename = "smoke";

      minInitialSpeed = 20;
      maxInitialSpeed = 100;

      minAcceleration = 0; // без ускорения
      maxAcceleration = 0;

      // живуч
      minLifetime = 5.0f;
      maxLifetime = 7.0f;

      minScale = .5f;
      maxScale = 1.0f;

      minNumParticles = 7;
      maxNumParticles = 15;

      // поворачиваем медленно
      minRotationSpeed = -MathHelper.PiOver4 / 2.0f;
      maxRotationSpeed = MathHelper.PiOver4 / 2.0f;

      spriteBlendMode = SpriteBlendMode.AlphaBlend;

      DrawOrder = AlphaBlendDrawOrder;
    }

    protected override Vector2 PickRandomDirection()
    {
      // Эффект выпускания дыма
      float radians = RandomHelper.GetRandomFloat(MathHelper.ToRadians(80), MathHelper.ToRadians(100));

      Vector2 direction = Vector2.Zero;

      direction.X = (float)Math.Cos(radians);
      direction.Y = -(float)Math.Sin(radians); // вверх => уменьшение у
      return direction;
    }

    protected override void InitializeParticle(Particle p, Vector2 where)
    {
      base.InitializeParticle(p, where);

      // the base is mostly good, but we want to simulate a little bit of wind
      // heading to the right.
      p.Acceleration = new Vector2(p.Acceleration.X + RandomHelper.GetRandomInt(-20, 20), 
        p.Acceleration.Y);
    }
  }
}
