using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace CyberBot
{
    public partial class MainForm : Form
    {
        private UserProfile _user;
        private ResponseEngine _engine;
        private bool _isAwaitingName = true;

        public MainForm()
        {
            InitializeComponent();
            _engine = new ResponseEngine();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Display ASCII banner
            lblBanner.Text = AsciiArt.GetBanner();

            // Play voice greeting
            Task.Run(() => AudioPlayer.PlayGreeting());

            // Initial bot message
            AppendBotMessage("Hello! I'm your Cybersecurity Awareness Bot. 🛡️");
            AppendBotMessage("Before we begin — what's your name?");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            ProcessInput();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent beep
                ProcessInput();
            }
        }

        private void ProcessInput()
        {
            string input = txtInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendUserMessage(input);
            txtInput.Clear();

            if (_isAwaitingName)
            {
                HandleNameInput(input);
            }
            else
            {
                HandleChatMessage(input);
            }
        }

        private void HandleNameInput(string name)
        {
            // Simple validation
            bool hasLetter = false;
            foreach (char c in name)
            {
                if (char.IsLetter(c)) { hasLetter = true; break; }
            }

            if (!hasLetter)
            {
                AppendBotMessage("That doesn't look like a name. Please use letters.");
                return;
            }

            _user = new UserProfile(name);
            _isAwaitingName = false;

            AppendBotMessage($"Great to meet you, {_user.Name}! 🎉");
            AppendBotMessage("I'm here to help you stay safe online. You can ask me about passwords, phishing, scams, privacy, and more.");
            AppendBotMessage("Type 'topics' for a full list.");
        }

        private void HandleChatMessage(string input)
        {
            if (_engine.IsExitCommand(input))
            {
                AppendBotMessage($"Thanks for chatting, {_user.Name}! Stay safe online.");
                AppendBotMessage(_user.GetSessionSummary());
                txtInput.Enabled = false;
                btnSend.Enabled = false;
                return;
            }

            string response = _engine.GetResponse(input, _user);
            _user.QuestionCount++;

            AppendBotMessage(response);

            // Periodic tips
            if (_user.QuestionCount > 0 && _user.QuestionCount % 5 == 0)
            {
                AppendBotMessage($"🏆 You've asked {_user.QuestionCount} questions, {_user.Name}! Knowledge is your best defence.");
            }
        }

        private void AppendBotMessage(string message)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = Color.LightGreen;
            rtbChat.AppendText("\n🤖 CyberBot ► ");
            
            rtbChat.SelectionColor = Color.White;
            rtbChat.AppendText(message + "\n");
            
            ScrollToBottom();
        }

        private void AppendUserMessage(string message)
        {
            string userName = _user?.Name ?? "User";
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = Color.Yellow;
            rtbChat.AppendText($"\n👤 {userName} ► ");
            
            rtbChat.SelectionColor = Color.White;
            rtbChat.AppendText(message + "\n");
            
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            rtbChat.SelectionStart = rtbChat.Text.Length;
            rtbChat.ScrollToCaret();
        }
    }
}
