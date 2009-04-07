#region Using directives
using Microsoft.Xna.Framework.Audio;
using System;
using XnaTetris.Game;
using XnaTetris.Helpers;
using Microsoft.Xna.Framework.Input;
using System.IO;
#endregion

namespace XnaTetris.Sounds
{
  class Sound
  {
    #region Variables
    /// <summary>
    /// Sound stuff for XAct
    /// </summary>
    static readonly AudioEngine audioEngine;
    static readonly WaveBank waveBank;
    static readonly SoundBank soundBank;
    #endregion

    #region Enums
    public enum Sounds
    {
      BlockMove,
      BlockRotate,
      BlockFalldown,
      LineKill,
      Fight,
      Victory,
      Lose,
    }
    #endregion

    #region Constructor
    static Sound()
    {
      try
      {
        string dir = Directories.SoundsDirectory;

        audioEngine = new AudioEngine(Path.Combine(dir, "TetrisSound.xgs"));
        waveBank = new WaveBank(audioEngine, Path.Combine(dir, "Wave Bank.xwb"));

        // Dummy wavebank call to get rid of the warning that waveBank is
        // never used (well it is used, but only inside of XNA).
        if (waveBank != null)
          soundBank = new SoundBank(audioEngine, Path.Combine(dir, "Sound Bank.xsb"));
      }
      catch (Exception ex)
      {
        // Audio creation crashes in early xna versions, log it and ignore it!
        Log.Write("Failed to create sound class: " + ex);
      }
    }
    #endregion

    #region Play
    public static void Play(string soundName)
    {
      if (soundBank == null)
        return;

      try
      {
        soundBank.PlayCue(soundName);
      }
      catch (Exception ex)
      {
        Log.Write("Playing sound " + soundName + " failed: " + ex);
      }
    }

    public static void Play(Sounds sound)
    {
      Play(sound.ToString());
    }
    #endregion

    #region Update
    public static void Update()
    {
      if (audioEngine != null)
        audioEngine.Update();
    }
    #endregion

    #region Unit Testing
#if DEBUG
    /// <summary>
    /// Test play click sound
    /// </summary>
    //[Test]
    public static void TestPlayClickSound()
    {
      //int crazyCounter = 0;

      TestGame.Start(
        delegate
        {
          if (Input.MouseLeftButtonJustPressed ||
            Input.GamePadAJustPressed)
            Play(Sounds.BlockMove);
          else if (Input.MouseRightButtonJustPressed ||
            Input.GamePadBJustPressed)
            Play(Sounds.BlockRotate);
          else if (Input.KeyboardKeyJustPressed(Keys.D1))
            Play(Sounds.BlockFalldown);
          else if (Input.KeyboardKeyJustPressed(Keys.D2))
            Play(Sounds.LineKill);
          else if (Input.KeyboardKeyJustPressed(Keys.D3))
            Play(Sounds.Fight);
          else if (Input.KeyboardKeyJustPressed(Keys.D4))
            Play(Sounds.Victory);
          else if (Input.KeyboardKeyJustPressed(Keys.D5))
            Play(Sounds.Lose);

          TextureFont.WriteText(2, 30,
            "Press 0-5 or A/B or left/right mouse buttons to play back " +
            "sounds!");
        });
    }
#endif
    #endregion
  }
}
