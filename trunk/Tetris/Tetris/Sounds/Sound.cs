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
  }
}
