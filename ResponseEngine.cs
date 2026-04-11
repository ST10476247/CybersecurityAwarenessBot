using System;
using System.Collections.Generic;

namespace CyberBot
{
    /// <summary>
    /// Processes user input and returns appropriate cybersecurity responses.
    /// Uses string manipulation (Trim, ToLower, Contains, Replace) throughout.
    /// </summary>
    public class ResponseEngine
    {
        // ── Auto-property: tracks the last matched topic ──────────────────
        public string LastMatchedTopic { get; private set; }

        // ── Auto-property: counts how many unknown queries were received ───
        public int UnrecognisedCount { get; private set; }

        // ── Keyword → response map ─────────────────────────────────────────
        private readonly Dictionary<string, Func<string, string>> _handlers;

        public ResponseEngine()
        {
            LastMatchedTopic  = string.Empty;
            UnrecognisedCount = 0;
            _handlers         = BuildHandlers();
        }

        // ─────────────────────────────────────────────────────────────────
        // Public API
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns true if the input is an exit command.
        /// </summary>
        public bool IsExitCommand(string input)
        {
            string cleaned = CleanInput(input);
            return cleaned == "exit" || cleaned == "quit" || cleaned == "bye"
                || cleaned == "goodbye" || cleaned == "q";
        }

        /// <summary>
        /// Validates input and returns an appropriate response string.
        /// Applies string manipulation to normalise the input before matching.
        /// </summary>
        public string GetResponse(string rawInput)
        {
            // ── Input validation ────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(rawInput))
            {
                return "I didn't quite understand that — it looks like you sent an empty message. " +
                       "Could you rephrase?";
            }

            string input = CleanInput(rawInput);

            if (input.Length < 2)
            {
                return "That's a very short message! Could you give me a bit more detail?";
            }

            // ── Keyword matching ─────────────────────────────────────────
            foreach (KeyValuePair<string, Func<string, string>> handler in _handlers)
            {
                if (input.Contains(handler.Key))
                {
                    LastMatchedTopic = handler.Key;
                    return handler.Value(input);
                }
            }

            // ── Default fallback ─────────────────────────────────────────
            UnrecognisedCount++;
            LastMatchedTopic = string.Empty;
            return BuildFallbackResponse(rawInput);
        }

        // ─────────────────────────────────────────────────────────────────
        // Private helpers
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Normalises raw input: trims whitespace, lowercases, removes punctuation.
        /// Demonstrates string manipulation.
        /// </summary>
        private static string CleanInput(string raw)
        {
            return raw
                .Trim()
                .ToLower()
                .Replace("?", string.Empty)
                .Replace("!", string.Empty)
                .Replace(",", string.Empty)
                .Replace(".", string.Empty);
        }

        /// <summary>Builds the keyword-to-handler dictionary.</summary>
        private static Dictionary<string, Func<string, string>> BuildHandlers()
        {
            return new Dictionary<string, Func<string, string>>
            {
                // ── Conversational ───────────────────────────────────────
                ["how are you"] = _ =>
                    "I'm fully operational and threat-alert! 🛡️ " +
                    "Thanks for asking. How can I help you stay safe online today?",

                ["hello"]       = _ =>
                    "Hello there! Great to see you. " +
                    "Type 'topics' to see everything I can help you with.",

                ["hi"]          = _ =>
                    "Hey! 👋 I'm your Cybersecurity Awareness Bot. " +
                    "Ask me about passwords, phishing, safe browsing, and more!",

                ["thank"]       = _ =>
                    "You're very welcome! Staying informed is the first step to staying secure. 😊",

                // ── Bot purpose ──────────────────────────────────────────
                ["purpose"]     = _ =>
                    "My purpose is to educate and empower you with cybersecurity knowledge. 🎯\n" +
                    "  I can help you understand threats like phishing and malware,\n" +
                    "  give tips on strong passwords, safe browsing, and much more.\n" +
                    "  Type 'topics' to see the full list!",

                ["what can"]    = _ =>
                    "Great question! Here are my areas of expertise:\n" +
                    "  › Password safety    › Phishing awareness\n" +
                    "  › Safe browsing      › Malware prevention\n" +
                    "  › Two-factor auth    › VPN basics\n" +
                    "  › Social media safety\n" +
                    "  Just type any of these topics and I'll give you detailed tips!",

                ["topics"]      = _ =>
                    "TYPE ONE OF THESE TO LEARN MORE:\n" +
                    "  password | phishing | browsing | malware | 2fa | vpn | social media",

                // ── Cybersecurity topics ─────────────────────────────────
                ["password"]    = _ =>
                    "🔐 PASSWORD SAFETY TIPS:\n" +
                    "  1. Use at least 12 characters — mix letters, numbers, and symbols.\n" +
                    "  2. Never reuse passwords across different sites.\n" +
                    "  3. Use a password manager (e.g. Bitwarden, 1Password) to store them safely.\n" +
                    "  4. Avoid obvious info like your name or birthday.\n" +
                    "  5. Change passwords immediately if a breach is detected.\n" +
                    "  💡 Example of a strong password: T!gr3$-M0un7@in#42",

                ["phishing"]    = _ =>
                    "🎣 HOW TO SPOT PHISHING ATTACKS:\n" +
                    "  1. Check the sender's email address carefully — look for misspellings.\n" +
                    "  2. Hover over links before clicking to see the real URL.\n" +
                    "  3. Be suspicious of urgent messages like 'Your account will be closed!'.\n" +
                    "  4. Legitimate companies never ask for passwords via email.\n" +
                    "  5. When in doubt, go directly to the website by typing the URL.\n" +
                    "  ⚠️  If you suspect phishing, report it to your IT/security team.",

                ["browsing"]    = _ =>
                    "🌐 SAFE BROWSING HABITS:\n" +
                    "  1. Always check for HTTPS (🔒) in the address bar.\n" +
                    "  2. Keep your browser and extensions updated.\n" +
                    "  3. Avoid public Wi-Fi for sensitive activities — use a VPN instead.\n" +
                    "  4. Use an ad blocker to reduce malicious ad exposure.\n" +
                    "  5. Disable browser autofill for passwords on shared computers.\n" +
                    "  6. Clear your cache and cookies regularly.",

                ["malware"]     = _ =>
                    "🦠 MALWARE PREVENTION:\n" +
                    "  1. Install reputable antivirus software and keep it updated.\n" +
                    "  2. Never download software from untrusted sources.\n" +
                    "  3. Don't open email attachments from unknown senders.\n" +
                    "  4. Keep your OS and applications patched and up to date.\n" +
                    "  5. Back up your data regularly to an offline or cloud location.\n" +
                    "  ⚠️  Signs of infection: slow PC, unexpected pop-ups, crashes.",

                ["virus"]       = _ =>
                    "🦠 A virus is a type of malware that replicates itself to spread.\n" +
                    "  Protection tips:\n" +
                    "  › Use updated antivirus software.\n" +
                    "  › Avoid pirated software and suspicious downloads.\n" +
                    "  › Scan USB drives before opening files.",

                ["2fa"]         = _ =>
                    "🔑 TWO-FACTOR AUTHENTICATION (2FA):\n" +
                    "  2FA adds a second layer of security beyond just a password.\n" +
                    "  Even if your password is stolen, attackers can't log in without\n" +
                    "  the second factor (a code from your phone or an app).\n" +
                    "  Types of 2FA:\n" +
                    "  › SMS code (convenient but least secure)\n" +
                    "  › Authenticator app like Google Authenticator (recommended)\n" +
                    "  › Hardware key like YubiKey (most secure)\n" +
                    "  💡 Enable 2FA on every account that offers it!",

                ["two factor"]  = _ =>
                    "🔑 TWO-FACTOR AUTHENTICATION:\n" +
                    "  This means you need TWO things to log in: your password plus a code.\n" +
                    "  Use an authenticator app (Google Authenticator / Authy) for best security.\n" +
                    "  Enable it on email, banking, and social media accounts first.",

                ["vpn"]         = _ =>
                    "🌍 VPN (Virtual Private Network):\n" +
                    "  A VPN encrypts your internet traffic and hides your IP address.\n" +
                    "  When to use a VPN:\n" +
                    "  › On public Wi-Fi (coffee shops, airports)\n" +
                    "  › When accessing work systems remotely\n" +
                    "  › When you want privacy from your ISP\n" +
                    "  Recommended VPNs: ProtonVPN, Mullvad, NordVPN\n" +
                    "  ⚠️  Free VPNs often log and sell your data — avoid them!",

                ["social"]      = _ =>
                    "📱 SOCIAL MEDIA SAFETY:\n" +
                    "  1. Review your privacy settings — limit who sees your posts.\n" +
                    "  2. Never share personal details like your address or phone number publicly.\n" +
                    "  3. Be cautious of friend requests from strangers.\n" +
                    "  4. Don't click on links in DMs from people you don't know.\n" +
                    "  5. Log out of social media on shared or public devices.\n" +
                    "  6. Think before you post — once it's online, it can be permanent.",

                ["ransomware"]  = _ =>
                    "💰 RANSOMWARE:\n" +
                    "  Ransomware encrypts your files and demands payment for the key.\n" +
                    "  Prevention:\n" +
                    "  › Back up files to an offline location regularly.\n" +
                    "  › Don't open suspicious email attachments.\n" +
                    "  › Keep software updated.\n" +
                    "  ⚠️  If infected: disconnect from the internet immediately and contact IT.",

                ["encryption"]  = _ =>
                    "🔒 ENCRYPTION:\n" +
                    "  Encryption scrambles data so only authorised parties can read it.\n" +
                    "  Look for HTTPS, end-to-end encrypted messaging (Signal, WhatsApp),\n" +
                    "  and encrypted storage for sensitive files.",
            };
        }

        /// <summary>Builds a contextual fallback response.</summary>
        private string BuildFallbackResponse(string originalInput)
        {
            // String manipulation: extract first word from original input
            string[] words = originalInput.Trim().Split(' ');
            string firstWord = words.Length > 0 ? words[0] : "that";

            if (UnrecognisedCount >= 3)
            {
                return $"I'm still not sure what you mean by '{firstWord}'. " +
                       "Try typing 'topics' to see the full list of subjects I can help with.";
            }

            return $"I didn't quite understand '{firstWord}'. Could you rephrase? " +
                   "You can also type 'topics' to see what I know about.";
        }
    }
}
