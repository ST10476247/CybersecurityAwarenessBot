using System;
using System.Threading;

namespace CyberBot
{
    /// <summary>
    /// Handles all console output formatting:
    /// colours, borders, typing effects, and section headers.
    /// </summary>
    public static class ConsoleUI
    {
        // ── Colour palette ────────────────────────────────────────────────
        private static readonly ConsoleColor ColourBanner   = ConsoleColor.Cyan;
        private static readonly ConsoleColor ColourBot      = ConsoleColor.Green;
        private static readonly ConsoleColor ColourUser     = ConsoleColor.Yellow;
        private static readonly ConsoleColor ColourError    = ConsoleColor.Red;
        private static readonly ConsoleColor ColourInfo     = ConsoleColor.DarkCyan;
        private static readonly ConsoleColor ColourHighlight= ConsoleColor.White;
        private static readonly ConsoleColor ColourDivider  = ConsoleColor.DarkGray;

        // ── Typing speed (ms per character) ──────────────────────────────
        private const int TypingDelayMs = 18;

        // ─────────────────────────────────────────────────────────────────
        // Public API
        // ─────────────────────────────────────────────────────────────────

        /// <summary>Clears the screen and sets a dark background.</summary>
        public static void Initialise()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorVisible = true;
        }

        /// <summary>Prints the full ASCII banner in cyan.</summary>
        public static void PrintBanner()
        {
            WriteColour(AsciiArt.GetBanner(), ColourBanner);
            Console.WriteLine();
        }

        /// <summary>Prints a divider line.</summary>
        public static void PrintDivider(bool thin = false)
        {
            WriteColour(thin ? AsciiArt.GetThinDivider() : AsciiArt.GetDivider(), ColourDivider);
            Console.WriteLine();
        }

        /// <summary>Prints a section header with surrounding dividers.</summary>
        public static void PrintSectionHeader(string title)
        {
            Console.WriteLine();
            PrintDivider();
            WriteColour($"  ▸ {title.ToUpper()}", ColourHighlight);
            Console.WriteLine();
            PrintDivider(thin: true);
        }

        /// <summary>
        /// Prints a bot message with a typing/streaming effect.
        /// </summary>
        public static void PrintBotMessage(string message, string name = null)
        {
            string prefix = name != null
                ? $"\n  🤖 CyberBot ► "
                : "\n  🤖 CyberBot ► ";

            WriteColour(prefix, ColourBot);
            TypeWrite(message, ColourHighlight);
            Console.WriteLine();
        }

        /// <summary>Prints a system/info message (no typing effect).</summary>
        public static void PrintInfo(string message)
        {
            WriteColour($"\n  ℹ  {message}", ColourInfo);
            Console.WriteLine();
        }

        /// <summary>Prints an error or warning message.</summary>
        public static void PrintWarning(string message)
        {
            WriteColour($"\n  ⚠  {message}", ColourError);
            Console.WriteLine();
        }

        /// <summary>Displays the user input prompt and reads a line.</summary>
        public static string PromptUser(string userName)
        {
            Console.WriteLine();
            WriteColour($"  👤 {userName} ► ", ColourUser);
            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine();
            Console.ResetColor();
            return input;
        }

        /// <summary>Prints a goodbye banner.</summary>
        public static void PrintGoodbye(UserProfile profile)
        {
            Console.WriteLine();
            PrintDivider();
            WriteColour(
                $"  Thanks for chatting, {profile.Name}! " +
                $"Stay safe online. Session: {profile.SessionDuration:mm\\:ss} min.",
                ColourBot);
            Console.WriteLine();
            PrintDivider();
            Console.WriteLine();
        }

        /// <summary>Prints a list of available topics in a formatted box.</summary>
        public static void PrintTopicList()
        {
            string[] topics = new[]
            {
                "password      – Tips on creating strong passwords",
                "phishing      – How to spot and avoid phishing attacks",
                "browsing      – Safe browsing habits",
                "malware       – Understanding and preventing malware",
                "2fa           – Two-factor authentication explained",
                "vpn           – What is a VPN and when to use it",
                "social media  – Staying safe on social platforms",
                "purpose       – What I can help you with",
                "how are you   – Just to say hi!",
                "exit / quit   – End the session"
            };

            Console.WriteLine();
            WriteColour("  ┌─────────────────────────────────────────────────────────────┐", ColourInfo);
            Console.WriteLine();
            WriteColour("  │  📋  TOPICS YOU CAN ASK ABOUT                               │", ColourInfo);
            Console.WriteLine();
            WriteColour("  ├─────────────────────────────────────────────────────────────┤", ColourInfo);
            Console.WriteLine();

            foreach (string topic in topics)
            {
                WriteColour($"  │  › {topic,-57}│", ColourInfo);
                Console.WriteLine();
            }

            WriteColour("  └─────────────────────────────────────────────────────────────┘", ColourInfo);
            Console.WriteLine();
        }

        // ─────────────────────────────────────────────────────────────────
        // Private helpers
        // ─────────────────────────────────────────────────────────────────

        /// <summary>Writes text in the specified colour, then resets.</summary>
        private static void WriteColour(string text, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.Write(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints text one character at a time with a short delay
        /// to simulate a conversational typing effect.
        /// </summary>
        private static void TypeWrite(string text, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            foreach (char c in text)
            {
                Console.Write(c);
                // Pause slightly longer after sentence-ending punctuation
                int delay = (c == '.' || c == '!' || c == '?') ? TypingDelayMs * 4 : TypingDelayMs;
                Thread.Sleep(delay);
            }
            Console.ResetColor();
        }
    }
}
