using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTetris.Helpers;

namespace XnaTetris.Particles
{
  public abstract class ParticleManager : DrawableGameComponent
  {
    private LinesGame LinesGame { get { return Game as LinesGame; } }

    private static SpriteBatch partSprite;
    private Texture2D texture;
    private Vector2 origin; //из текстуры

    public const int AlphaBlendDrawOrder = 100;
    public const int AdditiveDrawOrder = 200;


    // Максимальное кол-во нужных нам эффектов
    private int howManyEffects;


    /// <summary>
    /// Тут мутим с частицами, чтобы их не удалять (не мучать GC), складываем в очередь, по необходимости
    /// тащим в массив
    /// </summary>
    Particle[] particles;
    Queue<Particle> freeParticles;
    public int FreeParticleCount
    {
      get { return freeParticles.Count; }
    }

    #region Для классов-потомков

    // Везде, где min и max, происходит рандом в этих пределах
    protected int minNumParticles;
    protected int maxNumParticles;

    protected string textureFilename;

    protected float minInitialSpeed;
    protected float maxInitialSpeed;

    protected float minAcceleration;
    protected float maxAcceleration;

    protected float minRotationSpeed;
    protected float maxRotationSpeed;

    protected float minLifetime;
    protected float maxLifetime;

    protected float minScale;
    protected float maxScale;

    protected SpriteBlendMode spriteBlendMode;

    #endregion

    protected ParticleManager(LinesGame game, int howManyEffects)
      : base(game)
    {
      partSprite = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
      this.howManyEffects = howManyEffects;
    }

    public override void Initialize()
    {
      InitializeConstants();

      particles = new Particle[howManyEffects * maxNumParticles];
      freeParticles = new Queue<Particle>(howManyEffects * maxNumParticles);
      for (int i = 0; i < particles.Length; i++)
      {
        particles[i] = new Particle();
        freeParticles.Enqueue(particles[i]);
      }
      base.Initialize();
    }

    // тут инитим из #region Для классов-потомков
    protected abstract void InitializeConstants();

    protected override void LoadContent()
    {
      // make sure sub classes properly set textureFilename.
      if (string.IsNullOrEmpty(textureFilename))
      {
        string message = "textureFilename wasn't set properly, so the " +
            "particle system doesn't know what texture to load. Make " +
            "sure your particle system's InitializeConstants function " +
            "properly sets textureFilename.";
        throw new InvalidOperationException(message);
      }
      texture = LinesGame.Content.Load<Texture2D>(@"Content\" + textureFilename);

      // ставим орижин по умолчания к центру текстуры
      origin.X = texture.Width / 2;
      origin.Y = texture.Height / 2;

      base.LoadContent();
    }

    public void AddParticles(Vector2 where)
    {
      int numParticles = RandomHelper.GetRandomInt(minNumParticles, maxNumParticles);

      for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
      {
        // если есть свободная частица - берем ее из очереди и инитим
        Particle p = freeParticles.Dequeue();
        InitializeParticle(p, where);
      }
    }

    protected virtual void InitializeParticle(Particle p, Vector2 where)
    {
      // рандомим направление движения частицы
      Vector2 direction = PickRandomDirection();

      // рандомим остальное
      float velocity = RandomHelper.GetRandomFloat(minInitialSpeed, maxInitialSpeed);
      float acceleration = RandomHelper.GetRandomFloat(minAcceleration, maxAcceleration);
      float lifetime = RandomHelper.GetRandomFloat(minLifetime, maxLifetime);
      float scale = RandomHelper.GetRandomFloat(minScale, maxScale);
      float rotationSpeed = RandomHelper.GetRandomFloat(minRotationSpeed, maxRotationSpeed);

      p.Initialize(where, velocity * direction, acceleration * direction,
          lifetime, scale, rotationSpeed);
    }

    protected virtual Vector2 PickRandomDirection()
    {
      float angle = RandomHelper.GetRandomFloat(0, MathHelper.TwoPi);
      return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
    }

    public override void Update(GameTime gameTime)
    {
      float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

      foreach (Particle p in particles)
      {
        if (p.Active)
        {
          p.Update(dt);
          if (!p.Active) //если на шаге жизнь частицы прервалась - закинем её в очередь
          {
            freeParticles.Enqueue(p);
          }
        }
      }

      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      // тут могут быть разные моды
     // partSprite.Begin(spriteBlendMode);

      foreach (Particle p in particles)
      {
        if (!p.Active)
          continue;

        // нормализуем Lifetime, потом кидаем это значение в альфу и scale
        float normalizedLifetime = p.TimeSinceStart / p.LifeTime;

        // так normalizedLifetime * (1 - normalizedLifetime) <= 0.25
        float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
        Color color = new Color(new Vector4(1, 1, 1, alpha));

        // make particles grow as they age. they'll start at 75% of their size,
        // and increase to 100% once they're finished.
        float scale = p.Scale * (.75f + .25f * normalizedLifetime);

        partSprite.Draw(texture, p.Position, null, color,
            p.Rotation, origin, scale, SpriteEffects.None, 0.0f);
      }

      //partSprite.End();

      base.Draw(gameTime);
    }

    public void Show()
    {
      this.Enabled = this.Visible = true;
    }

    public void Hide()
    {
      this.Enabled = this.Visible = false;
    }
  }
}
