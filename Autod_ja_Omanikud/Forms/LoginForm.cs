using Autod_ja_Omanikud.Data;
using Autod_ja_Omanikud.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

        public LoginForm()
        {
            _context = new AutoDbContext();

            Text = "Login";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = true;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            BuildLayout();
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
                MessageBox.Show("Enter username and password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}
