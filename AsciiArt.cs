namespace CyberBot
{
    /// <summary>
    /// Provides all ASCII art used in the chatbot's UI.
    /// </summary>
    public static class AsciiArt
    {
        /// <summary>Returns the main title banner displayed at launch.</summary>
        public static string GetBanner()
        {
            return @"
  ╔══════════════════════════════════════════════════════════════════╗
  ║                                                                  ║
  ║    ██████╗██╗   ██╗██████╗ ███████╗██████╗    ██████╗  ██████╗  ║
  ║   ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗   ██╔══██╗██╔═══██╗ ║
  ║   ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝   ██████╔╝██║   ██║ ║
  ║   ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗   ██╔══██╗██║   ██║ ║
  ║   ╚██████╗   ██║   ██████╔╝███████╗██║  ██║   ██████╔╝╚██████╔╝ ║
  ║    ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝   ╚═════╝  ╚═════╝  ║
  ║                                                                  ║
  ║         ░█▀█░█░█░█▀█░█▀▄░█▀▀░█▀█░█▀▀░█▀▀░░░█▀▄░█▀█░▀█▀          ║
  ║         ░█▀█░█▄█░█▀█░█▀▄░█▀▀░█░█░█▀▀░▀▀█░░░█▀▄░█░█░░█░          ║
  ║         ░▀░▀░▀░▀░▀░▀░▀░▀░▀▀▀░▀░▀░▀▀▀░▀▀▀░░░▀▀░░▀▀▀░░▀░          ║
  ║                                                                  ║
  ║              [ SECURE ]  [ PROTECT ]  [ EDUCATE ]                ║
  ║                                                                  ║
  ╚══════════════════════════════════════════════════════════════════╝";
        }

        /// <summary>Returns a small shield icon used in section headers.</summary>
        public static string GetShield()
        {
            return @"
          /\
         /  \
        / /\ \
       /_/  \_\
       |  ██  |
       | ████ |
       |  ██  |
        \____/";
        }

        /// <summary>Returns a divider line sized to the banner width.</summary>
        public static string GetDivider()
        {
            return "  " + new string('═', 66);
        }

        /// <summary>Returns a thin divider for sub-sections.</summary>
        public static string GetThinDivider()
        {
            return "  " + new string('─', 66);
        }
    }
}
