# 🛡️ Cybersecurity Awareness Bot

A C# console chatbot that educates users on cybersecurity topics including passwords, phishing, safe browsing, malware, VPNs, and two-factor authentication.

---

## 📋 Features

| Feature | Description |
|---|---|
| 🔊 Voice Greeting | Plays a WAV greeting on launch via `System.Media.SoundPlayer` |
| 🎨 ASCII Art Logo | Cybersecurity-themed banner displayed at startup |
| 👤 Personalisation | Asks for the user's name and uses it throughout the session |
| 💬 Q&A Responses | Keyword-matched responses for 12+ cybersecurity topics |
| ✅ Input Validation | Gracefully handles empty input, nonsense queries, and numeric-only names |
| 🎨 Rich Console UI | Coloured text, typing effect, bordered sections, and emoji |
| 🏗️ Clean Code | Split across 7 classes — nothing crammed into `Program.cs` |

---

## 🗂️ Project Structure

```
CyberBot/
├── .github/
│   └── workflows/
│       └── ci.yml          ← GitHub Actions CI workflow
├── CyberBot/
│   ├── Program.cs          ← Entry point only
│   ├── Bot.cs              ← Main lifecycle controller
│   ├── ResponseEngine.cs   ← Cybersecurity Q&A logic
│   ├── ConsoleUI.cs        ← All UI rendering (colours, typing effect, borders)
│   ├── AudioPlayer.cs      ← WAV playback via System.Media
│   ├── AsciiArt.cs         ← ASCII art definitions
│   ├── UserProfile.cs      ← Auto-properties for session data
│   ├── greeting.wav        ← Voice greeting audio file
│   └── CyberBot.csproj
├── .gitignore
└── README.md
```

---

## 🚀 Running the Project

### Prerequisites
- Visual Studio 2022
- .NET Framework 4.8 (pre-installed on Windows 10/11)
- Windows OS (required for `System.Media.SoundPlayer`)

### Steps
1. Open `CyberBot.csproj` in Visual Studio 2022
2. Press **F5** or click **Start** to run
3. The chatbot will play the greeting and launch in a console window

### Replacing the Voice Greeting
1. Record your voice message in **WAV format**
2. Name it `greeting.wav`
3. Copy it into the `CyberBot/` folder (next to `CyberBot.csproj`)
4. In Visual Studio, right-click the file → **Properties** → set **Copy to Output Directory** to **Copy always**

---

## 💬 Topics You Can Ask About

| Keyword | Topic |
|---|---|
| `password` | Strong password tips |
| `phishing` | How to spot phishing attacks |
| `browsing` | Safe browsing habits |
| `malware` | Malware prevention |
| `2fa` | Two-factor authentication |
| `vpn` | VPN basics |
| `social` | Social media safety |
| `ransomware` | Ransomware explained |
| `encryption` | What encryption is |
| `topics` | Full topic list |
| `exit` | End the session |

---

## ✅ CI / CD — GitHub Actions

This project uses **GitHub Actions** for Continuous Integration. Every push triggers an automated build on a Windows runner.

### CI Workflow (`.github/workflows/ci.yml`)
- **Trigger**: On every push / pull request to `main` or `master`
- **Runner**: `windows-latest` (required for .NET Framework + System.Media)
- **Steps**: Checkout → Setup .NET → Restore → Build (Release) → Verify EXE

### ✅ Successful CI Run
<!-- Replace the image below with a screenshot of your GitHub Actions green check mark -->
![CI Workflow Green Check](ci_screenshot.png)

---

## 🔖 Commit History

| # | Commit Message |
|---|---|
| 1 | `Initial commit: Set up project structure and main files` |
| 2 | `feat: Add ASCII art logo and ConsoleUI with colour formatting` |
| 3 | `feat: Add voice greeting playback via System.Media` |
| 4 | `feat: Add user interaction, name personalisation, and UserProfile` |
| 5 | `feat: Implement cybersecurity response engine with keyword matching` |
| 6 | `feat: Add input validation and default fallback responses` |
| 7 | `ci: Add GitHub Actions workflow for automated build checks` |
| 8 | `docs: Add README with project overview and CI screenshot` |

---

## 👨‍💻 Author

Student — Cybersecurity Awareness Bot Assignment  
Built with C# / .NET Framework 4.8 / Visual Studio 2022
