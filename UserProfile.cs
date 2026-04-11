using System;

namespace CyberBot
{
    /// <summary>
    /// Stores personalised data about the current chat session.
    /// Demonstrates automatic properties as required by the assignment.
    /// </summary>
    public class UserProfile
    {
        // ── Automatic properties ──────────────────────────────────────────
        public string Name { get; set; }
        public DateTime SessionStart { get; set; }
        public int QuestionCount { get; set; }
        public string FavouriteTopic { get; set; }

        // ── Computed property ─────────────────────────────────────────────
        public TimeSpan SessionDuration => DateTime.Now - SessionStart;

        // ── Constructor ───────────────────────────────────────────────────
        public UserProfile(string name)
        {
            // String manipulation: trim whitespace and capitalise first letter
            string trimmed = name.Trim();
            Name = trimmed.Length > 0
                ? char.ToUpper(trimmed[0]) + trimmed.Substring(1).ToLower()
                : "User";

            SessionStart    = DateTime.Now;
            QuestionCount   = 0;
            FavouriteTopic  = string.Empty;
        }

        /// <summary>Returns a friendly session summary string.</summary>
        public string GetSessionSummary()
        {
            return string.Format(
                "Session for {0} | Started: {1:HH:mm} | Questions asked: {2}",
                Name,
                SessionStart,
                QuestionCount);
        }
    }
}
