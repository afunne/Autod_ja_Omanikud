using Autod_ja_Omanikud.Data;
using Autod_ja_Omanikud.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;

namespace Autod_ja_Omanikud.Forms
{
    public class LoginForm : Form
    {
        private readonly AutoDbContext _context;

        private TextBox _txtUsername = null!;
        private TextBox _txtPassword = null!;
        private Button _btnLogin = null!;
        private Button _btnCancel = null!;

        public User? AuthenticatedUser { get; private set; }
        public bool LaunchMinesweeper { get; private set; }

        // localization
        private string _currentLang = "en";
        private const string LangConfigFileName = "language.config";
        private ResourceManager _resManager => Properties.Resources.ResourceManager;

        public LoginForm()
        {
            _context = new AutoDbContext();

            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = true;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            BuildLayout();

            LoadSavedLanguage();
            ApplyTranslations();

            EnsureDefaultAdminUser();
        }

        private void BuildLayout()
        {
            var main = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                AutoSize = true
            };

            main.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var lblUser = new Label { Text = "Username", AutoSize = true, Margin = new Padding(3, 6, 3, 3) };
            _txtUsername = new TextBox { Width = 200 };

            var lblPass = new Label { Text = "Password", AutoSize = true, Margin = new Padding(3, 6, 3, 3) };
            _txtPassword = new TextBox { Width = 200, UseSystemPasswordChar = true };

            main.Controls.Add(lblUser, 0, 0);
            main.Controls.Add(_txtUsername, 1, 0);
            main.Controls.Add(lblPass, 0, 1);
            main.Controls.Add(_txtPassword, 1, 1);

            var btnPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            _btnLogin = new Button { Text = "Login", AutoSize = true };
            _btnCancel = new Button { Text = "Cancel", AutoSize = true };

            _btnLogin.Click += BtnLogin_Click;
            _btnCancel.Click += (_, __) => DialogResult = DialogResult.Cancel;

            btnPanel.Controls.Add(_btnLogin);
            btnPanel.Controls.Add(_btnCancel);

            main.Controls.Add(btnPanel, 0, 2);
            main.SetColumnSpan(btnPanel, 2);

            Controls.Add(main);

            AcceptButton = _btnLogin;
            CancelButton = _btnCancel;
        }

        private void EnsureDefaultAdminUser()
        {
            // Ensure there is at least one admin. Default credentials: admin / admin
            if (!_context.Users.Any())
            {
                var admin = new User
                {
                    Username = "admin",
                    PasswordHash = User.HashPassword("admin"),
                    IsSuperAdmin = true,
                    CanManageOwners = true,
                    CanManageCars = true,
                    CanManageServices = true,
                    CanManageMaintenance = true,
                    CanManageUsers = true
                };

                _context.Users.Add(admin);
                _context.SaveChanges();
            }
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            var username = _txtUsername.Text.Trim();
            var password = _txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
            {
                var ci = GetCulture();
                var msg = _resManager.GetString("LoginForm_Error_Empty", ci)
                          ?? "Enter username and password.";
                MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Easter egg <3
            if (string.Equals(username, "minesweeper", StringComparison.OrdinalIgnoreCase)
                && string.Equals(password, "minesweeper", StringComparison.Ordinal))
            {
                LaunchMinesweeper = true;
                AuthenticatedUser = null;
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            var hash = User.HashPassword(password);
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);
            if (user == null)
            {
                MessageBox.Show("Invalid username or password.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AuthenticatedUser = user;
            DialogResult = DialogResult.OK;
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private void LoadSavedLanguage()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LangConfigFileName);
                if (File.Exists(path))
                {
                    var text = File.ReadAllText(path).Trim();
                    if (!string.IsNullOrEmpty(text))
                        _currentLang = text;
                }
            }
            catch { }
        }

        private CultureInfo GetCulture()
        {
            try { return new CultureInfo(_currentLang); }
            catch { return CultureInfo.InvariantCulture; }
        }

        private void ApplyTranslations()
        {
            var ci = GetCulture();
            var rm = _resManager;

            var s = rm.GetString("LoginForm_Title", ci);
            if (!string.IsNullOrEmpty(s)) Text = s;

            s = rm.GetString("LoginForm_Username", ci);
            if (!string.IsNullOrEmpty(s))
                _txtUsername.PlaceholderText = s;

            s = rm.GetString("LoginForm_Password", ci);
            if (!string.IsNullOrEmpty(s))
                _txtPassword.PlaceholderText = s;

            s = rm.GetString("LoginForm_BtnLogin", ci);
            if (!string.IsNullOrEmpty(s))
                _btnLogin.Text = s;

            s = rm.GetString("LoginForm_BtnCancel", ci);
            if (!string.IsNullOrEmpty(s))
                _btnCancel.Text = s;
        }
    }
}
