# Cybersecurity Awareness Bot

This is my Part 1 submission for the chatbot assignment. Its a C# console application that teaches users about cybersecurity topics like passwords, phishing and safe browsing. When it launches it plays a voice greeting, shows an ASCII art logo and then lets the user ask questions.

---

## How to run it

1. Open `CyberBot.csproj` in Visual Studio 2022
2. Press F5 to build and run
3. The bot will play the greeting sound and show the banner, then ask for your name

You need to be on Windows because the voice greeting uses `System.Media.SoundPlayer` which is Windows only.

---

## Project structure

I split the code across multiple classes instead of putting everything in Program.cs:

- `Program.cs` - just starts the bot, nothing else
- `Bot.cs` - controls the flow of the whole conversation
- `ResponseEngine.cs` - handles all the question and answer logic, also does the input validation
- `ConsoleUI.cs` - everything to do with how things look on screen (colours, typing effect, borders)
- `AudioPlayer.cs` - plays the WAV greeting file when the app starts
- `AsciiArt.cs` - stores the ASCII art banner
- `UserProfile.cs` - holds the users session info using automatic properties

---

## What you can ask the bot

Just type any of these topics and it will give you tips:

- password
- phishing
- browsing
- malware
- 2fa
- vpn
- social media
- ransomware
- encryption

Type `topics` to see the list in the app, or `exit` to quit.

---

## GitHub Actions CI

I set up a CI workflow using GitHub Actions. Every time I push code it automatically tries to build the project and checks for errors. The workflow file is saved in `.github/workflows/ci.yml` and it runs on a windows runner since the project needs .NET Framework.

Screenshot of a successful CI run:

![CI green check](ci_screenshot.png)

---

## Commits

I made 8 commits throughout the project to show my progress:

1. Initial commit: Set up project structure and main files
2. feat: Add ASCII art logo and ConsoleUI with colour formatting
3. feat: Add voice greeting playback via System.Media
4. feat: Add user interaction, name personalisation, and UserProfile
5. feat: Implement cybersecurity response engine with keyword matching
6. feat: Add input validation and default fallback responses
7. ci: Add GitHub Actions workflow for automated build checks
8. docs: Add README with project overview and CI screenshot
