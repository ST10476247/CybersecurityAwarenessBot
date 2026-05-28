using System;
using System.IO;
using System.Media;
using System.Text;

namespace CyberBot
{
    /// <summary>
    /// Handles WAV file playback using System.Media.SoundPlayer.
    /// Automatically generates a placeholder WAV if the file is missing,
    /// so the project always runs — replace greeting.wav with your own recording.
    /// </summary>
    public static class AudioPlayer
    {
        private const string WavFileName = "greeting.wav";

        /// <summary>
        /// Plays the greeting WAV file. Generates a placeholder if not found.
        /// Falls back silently if playback fails (e.g. no audio device).
        /// </summary>
        public static void PlayGreeting()
        {
            try
            {
                string wavPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WavFileName);

                // If no WAV exists yet, generate a simple two-tone placeholder
                if (!File.Exists(wavPath))
                {
                    GeneratePlaceholderWav(wavPath);
                }

                using (SoundPlayer player = new SoundPlayer(wavPath))
                {
                    player.Load();
                    player.PlaySync(); // Play synchronously so banner appears after audio
                }
            }
            catch (Exception ex)
            {
                // Audio failure should never crash the bot; just skip playback
                System.Diagnostics.Debug.WriteLine($"Audio unavailable: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // Placeholder WAV generator
        // Produces a 2-second, 440 Hz + 550 Hz two-tone chime (PCM 16-bit mono).
        // Replace greeting.wav with your own recording at any time.
        // ─────────────────────────────────────────────────────────────────
        private static void GeneratePlaceholderWav(string filePath)
        {
            const int sampleRate  = 44100;
            const int durationSec = 2;
            const short amplitude = 14000;

            int numSamples = sampleRate * durationSec;
            int dataSize   = numSamples * 2; // 16-bit = 2 bytes per sample

            using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
            {
                // ── RIFF header ──────────────────────────────────────────
                writer.Write(Encoding.ASCII.GetBytes("RIFF"));
                writer.Write(36 + dataSize);
                writer.Write(Encoding.ASCII.GetBytes("WAVE"));

                // ── fmt chunk (PCM) ──────────────────────────────────────
                writer.Write(Encoding.ASCII.GetBytes("fmt "));
                writer.Write(16);            // chunk size
                writer.Write((short)1);      // PCM format
                writer.Write((short)1);      // mono
                writer.Write(sampleRate);
                writer.Write(sampleRate * 2);// byte rate
                writer.Write((short)2);      // block align
                writer.Write((short)16);     // bits per sample

                // ── data chunk ───────────────────────────────────────────
                writer.Write(Encoding.ASCII.GetBytes("data"));
                writer.Write(dataSize);

                for (int i = 0; i < numSamples; i++)
                {
                    double t = (double)i / sampleRate;

                    // Blend two frequencies for a pleasant chime
                    double freq = (t < 1.0) ? 440.0 : 550.0;

                    // Apply a short fade-in/fade-out envelope to smooth clicks
                    double envelope = Math.Min(1.0, Math.Min(t * 20, (durationSec - t) * 20));

                    short sample = (short)(amplitude * envelope * Math.Sin(2 * Math.PI * freq * t));
                    writer.Write(sample);
                }
            }
        }
    }
}
