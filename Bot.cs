using System;
using System.Threading;

namespace CyberBot
{
    /// <summary>
    /// Main controller that orchestrates the chatbot's full lifecycle:
    /// startup, user interaction loop, and shutdown.
    /// </summary>
    public class Bot
    {
        private UserProfile    _user;
        private ResponseEngine _engine;

        // ─────────────────────────────────────────────────────────────────
        // Entry point called from Program.cs
        // ─────────────────────────────────────────────────────────────────

        public void Run()
        {
            // Step 1: Initialise the console (encoding, colours, clear screen)
            ConsoleUI.Initialise();

            // Step 2: Display the ASCII banner
            ConsoleUI.PrintBanner();

            // Step 3: Play the voice greeting
            PlayVoiceGreeting();

            // Step 4: Collect the user's name
            _user   = CollectUserName();
            _engine = new ResponseEngine();

            // Step 5: Show the personalised welcome message
            ShowWelcomeMessage();

            // Step 6: Enter the main chat loop
            ChatLoop();

            // Step 7: Say goodbye
            ConsoleUI.PrintGoodbye(_user);
            Thread.Sleep(1500);
        }

        // ─────────────────────────────────────────────────────────────────
        // Private methods
        // ─────────────────────────────────────────────────────────────────

        /// <summary>Plays the voice greeting WAV and shows a status message.</summary>
        private void PlayVoiceGreeting()
        {
            ConsoleUI.PrintInfo("Playing voice greeting...");
            Thread.Sleep(300);
            AudioPlayer.PlayGreeting();
        }

        /// <summary>
        /// Prompts for and validates the user's name (input validation loop).
        /// Returns a populated UserProfile.
        /// </summary>
        private UserProfile CollectUserName()
        {
            ConsoleUI.PrintSectionHeader("Welcome — Let's Get Started");

            ConsoleUI.PrintBotMessage(
                "Hello! I'm your Cybersecurity Awareness Bot. " +
                "I'm here to help you stay safe in the digital world. 🛡️");

            ConsoleUI.PrintBotMessage("Before we begin — what's your name?");

            string name;
            while (true)
            {
                Console.Write("\n  👤 Your name ► ");
                Console.ForegroundColor = ConsoleColor.White;
                name = Console.ReadLine();
                Console.ResetColor();

                // Input validation: reject empty or whitespace-only names
                if (string.IsNullOrWhiteSpace(name))
                {
                    ConsoleUI.PrintWarning("Please enter your name so I can personalise our chat!");
                    continue;
                }

                // Input validation: reject names that are purely numeric
                bool hasLetter = false;
                foreach (char c in name)
                {
                    if (char.IsLetter(c)) { hasLetter = true; break; }
                }

                if (!hasLetter)
                {
                    ConsoleUI.PrintWarning("That doesn't look like a name. Please use letters.");
                    continue;
                }

                break; // Valid name received
            }

            return new UserProfile(name);
        }

        /// <summary>Displays the post-name welcome message and topic hint.</summary>
        private void ShowWelcomeMessage()
        {
            ConsoleUI.PrintSectionHeader($"Welcome, {_user.Name}!");

            ConsoleUI.PrintBotMessage(
                $"Great to meet you, {_user.Name}! 🎉 " +
                "I'm the Cybersecurity Awareness Bot — " +
                "your personal guide to staying safe online.");

            ConsoleUI.PrintBotMessage(
                "You can ask me about passwords, phishing, safe browsing, " +
                "malware, VPNs, two-factor authentication, and more. " +
                "Type 'topics' for the full list, or 'exit' to end the session.");

            ConsoleUI.PrintTopicList();
        }

        /// <summary>
        /// Main conversation loop — reads input, validates, and prints responses
        /// until the user types an exit command.
        /// </summary>
        private void ChatLoop()
        {
            ConsoleUI.PrintSectionHeader("Chat — Ask Me Anything");

            while (true)
            {
                string input = ConsoleUI.PromptUser(_user.Name);

                // Check for exit intent
                if (_engine.IsExitCommand(input))
                    break;

                // Get and display the response
                string response = _engine.GetResponse(input);
                _user.QuestionCount++;

                ConsoleUI.PrintBotMessage(response, _user.Name);

                // Show a tip after every 5 questions
                if (_user.QuestionCount > 0 && _user.QuestionCount % 5 == 0)
                {
                    ConsoleUI.PrintInfo(
                        $"🏆 You've asked {_user.QuestionCount} questions, {_user.Name}! " +
                        "Great curiosity — knowledge is your best defence.");
                }
            }
        }
    }
}
