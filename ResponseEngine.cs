using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly Dictionary<string, Func<UserProfile, string, string>> _handlers;
        private readonly Random _random = new Random();

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
        /// </summary>
        public string GetResponse(string rawInput, UserProfile user)
        {
            if (string.IsNullOrWhiteSpace(rawInput))
            {
                return "I didn't quite understand that — it looks like you sent an empty message. Could you rephrase?";
            }

            string input = CleanInput(rawInput);

            // ── Sentiment Detection ──────────────────────────────────────
            string sentimentResponse = DetectSentiment(input, user);
            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                // If sentiment detected, we prepend it to the topic response if a topic is found
                string topicResponse = FindTopicResponse(input, user);
                if (!string.IsNullOrEmpty(topicResponse))
                {
                    return sentimentResponse + " " + topicResponse;
                }
                return sentimentResponse;
            }

            // ── Topic Matching ───────────────────────────────────────────
            string response = FindTopicResponse(input, user);
            if (!string.IsNullOrEmpty(response))
            {
                // Occasional personalization recall
                if (_random.Next(10) < 2 && !string.IsNullOrEmpty(user.FavouriteTopic) && !response.Contains(user.FavouriteTopic))
                {
                    response = $"As someone interested in {user.FavouriteTopic}, you might find this relevant: " + response;
                }
                return response;
            }

            // ── Conversation Flow (Follow-up) ────────────────────────────
            if (IsFollowUpRequest(input))
            {
                return HandleFollowUp(user);
            }

            // ── Default fallback ─────────────────────────────────────────
            UnrecognisedCount++;
            LastMatchedTopic = string.Empty;
            return BuildFallbackResponse(rawInput);
        }

        // ─────────────────────────────────────────────────────────────────
        // Private helpers
        // ─────────────────────────────────────────────────────────────────

        private string FindTopicResponse(string input, UserProfile user)
        {
            foreach (var handler in _handlers)
            {
                if (input.Contains(handler.Key))
                {
                    LastMatchedTopic = handler.Key;
                    
                    string prefix = "";
                    // Memory: store interest
                    if (IsCybersecurityTopic(handler.Key))
                    {
                        if (string.IsNullOrEmpty(user.FavouriteTopic) || user.FavouriteTopic != handler.Key)
                        {
                            user.FavouriteTopic = handler.Key;
                            prefix = $"Great! I'll remember that you're interested in {handler.Key}. It's a crucial part of staying safe online. ";
                        }
                    }

                    return prefix + handler.Value(user, input);
                }
            }
            return null;
        }

        private bool IsCybersecurityTopic(string topic)
        {
            string[] topics = { "password", "phishing", "scam", "privacy", "malware", "2fa", "vpn", "social" };
            return topics.Any(t => topic.Contains(t));
        }

        private bool IsFollowUpRequest(string input)
        {
            string[] followUps = { "more", "another", "explain", "tell me more", "give me another" };
            return followUps.Any(f => input.Contains(f));
        }

        private string HandleFollowUp(UserProfile user)
        {
            if (string.IsNullOrEmpty(user.FavouriteTopic))
            {
                return "I'm not sure what you'd like to hear more about. Try asking about passwords, phishing, or privacy!";
            }

            // Return a more detailed tip or another tip for the last topic
            return $"Sure! Since you're interested in {user.FavouriteTopic}, here's some more info: " + 
                   GetRandomResponse(user.FavouriteTopic + "_extra") ?? $"I've told you quite a bit about {user.FavouriteTopic} already! What else can I help with?";
        }

        private string DetectSentiment(string input, UserProfile user)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("afraid"))
            {
                return "It's completely understandable to feel worried. Cybersecurity can be complex, but I'm here to help you stay safe. Let's look at some simple steps you can take.";
            }
            if (input.Contains("curious") || input.Contains("interested") || input.Contains("learn"))
            {
                return "I love that curiosity! Staying informed is the best way to protect yourself online.";
            }
            if (input.Contains("frustrated") || input.Contains("annoyed") || input.Contains("hard"))
            {
                return "I hear you. Dealing with security settings can be frustrating sometimes. Let's try to break it down into easy steps.";
            }
            return null;
        }

        private static string CleanInput(string raw)
        {
            if (raw == null) return string.Empty;
            return raw.Trim().ToLower()
                .Replace("?", "").Replace("!", "").Replace(",", "").Replace(".", "");
        }

        private Dictionary<string, Func<UserProfile, string, string>> BuildHandlers()
        {
            return new Dictionary<string, Func<UserProfile, string, string>>
            {
                ["hello"] = (u, i) => $"Hello there, {u.Name}! How can I help you with cybersecurity today?",
                ["hi"] = (u, i) => $"Hi {u.Name}! I'm your CyberBot. Ready to learn some safety tips?",
                
                ["password"] = (u, i) => {
                    u.FavouriteTopic = "password";
                    return GetRandomResponse("password");
                },
                ["phishing"] = (u, i) => {
                    u.FavouriteTopic = "phishing";
                    return GetRandomResponse("phishing");
                },
                ["scam"] = (u, i) => {
                    u.FavouriteTopic = "scam";
                    return GetRandomResponse("scam");
                },
                ["privacy"] = (u, i) => {
                    u.FavouriteTopic = "privacy";
                    return GetRandomResponse("privacy");
                },
                ["topics"] = (u, i) => "You can ask me about: passwords, phishing, scams, privacy, malware, 2FA, VPNs, and social media safety.",
                
                // Memory recall example
                ["what do i like"] = (u, i) => !string.IsNullOrEmpty(u.FavouriteTopic) 
                    ? $"You mentioned being interested in {u.FavouriteTopic} earlier! It's a great topic to focus on."
                    : "I'm not sure yet! Tell me what cybersecurity topics interest you.",
                
                ["my name"] = (u, i) => $"Your name is {u.Name}! I'll remember that throughout our chat."
            };
        }

        private string GetRandomResponse(string category)
        {
            var responses = new Dictionary<string, string[]>()
            {
                ["password"] = new[] {
                    "Make sure to use strong, unique passwords for each account. Avoid using personal details like birthdays.",
                    "A strong password should be at least 12 characters long and include a mix of symbols, numbers, and letters.",
                    "Using a password manager is a great way to keep track of complex passwords without having to remember them all."
                },
                ["phishing"] = new[] {
                    "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                    "Always hover over links in emails to see the actual destination URL before clicking.",
                    "If an email feels urgent or creates a sense of panic, it might be a phishing attempt. Take a breath and verify the sender."
                },
                ["scam"] = new[] {
                    "If an offer online sounds too good to be true, it's probably a scam. Always verify the source.",
                    "Never share your banking details or OTPs with anyone over the phone or via email.",
                    "Scammers often use 'urgent' requests to pressure you. Take your time and think before you act."
                },
                ["privacy"] = new[] {
                    "Review your social media privacy settings regularly to control who can see your personal information.",
                    "Be careful about what apps you give permission to access your location or contacts.",
                    "Privacy is about more than just passwords; it's about being mindful of what you share online."
                },
                ["password_extra"] = new[] {
                    "You should also enable Two-Factor Authentication (2FA) for an extra layer of security.",
                    "Consider using 'passphrases' — long strings of random words — which are easier for humans to remember but hard for computers to crack."
                },
                ["phishing_extra"] = new[] {
                    "Check for grammatical errors or generic greetings like 'Dear Customer' as these are common red flags.",
                    "Remember that legitimate companies will never ask for your password via email."
                },
                ["scam_extra"] = new[] {
                    "Always look for the 'https' and the padlock icon in your browser when entering sensitive information.",
                    "If you're unsure about a website, search for reviews or reports of scams associated with it."
                },
                ["privacy_extra"] = new[] {
                    "Using a VPN can help hide your IP address and keep your browsing activity private from your ISP.",
                    "Consider using a privacy-focused browser or search engine to reduce tracking."
                }
            };

            if (responses.ContainsKey(category))
            {
                var list = responses[category];
                return list[_random.Next(list.Length)];
            }
            return null;
        }

        private string BuildFallbackResponse(string originalInput)
        {
            string[] words = originalInput.Trim().Split(' ');
            string firstWord = words.Length > 0 ? words[0] : "that";

            return $"I'm not sure I understand what you mean by '{firstWord}'. Can you try rephrasing? You can also type 'topics' to see what I can help with.";
        }
    }
}

