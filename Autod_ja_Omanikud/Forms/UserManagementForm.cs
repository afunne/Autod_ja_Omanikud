using Autod_ja_Omanikud.Data;
using Autod_ja_Omanikud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;

namespace Autod_ja_Omanikud.Forms
{
    public class UserManagementForm : Form
    {
        private readonly AutoDbContext _context;
        private DataGridView _dgvUsers = null!;
        private Button _btnAdd = null!;
        private Button _btnEdit = null!;
        private Button _btnDelete = null!;

        // localization
        private string _currentLang = "en";
        private const string LangConfigFileName = "language.config";
        private ResourceManager _resManager => Properties.Resources.ResourceManager;

        public UserManagementForm()
        {
            _context = new AutoDbContext();

            StartPosition = FormStartPosition.CenterParent;
            Width = 700;
            Height = 400;

            BuildLayout();

            LoadSavedLanguage();
            ApplyTranslations();

            LoadUsers();
        }

        private void BuildLayout()
        {
            _dgvUsers = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 260,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoGenerateColumns = true
            };

            _btnAdd = new Button { Text = "Add", AutoSize = true };
            _btnEdit = new Button { Text = "Update", AutoSize = true };
            _btnDelete = new Button { Text = "Delete", AutoSize = true };

            _btnAdd.Click += BtnAdd_Click;
            _btnEdit.Click += BtnEdit_Click;
            _btnDelete.Click += BtnDelete_Click;

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Padding = new Padding(10)
            };

            buttonPanel.Controls.AddRange(new Control[] { _btnAdd, _btnEdit, _btnDelete });

            Controls.Add(_dgvUsers);
            Controls.Add(buttonPanel);
        }

        private void LoadUsers()
        {
            _dgvUsers.DataSource = _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.IsSuperAdmin,
                    u.CanManageOwners,
                    u.CanManageCars,
                    u.CanManageServices,
                    u.CanManageMaintenance,
                    u.CanManageUsers
                })
                .ToList();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var user = EditUser(null);
            if (user != null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                LoadUsers();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (_dgvUsers.CurrentRow == null) return;
            var id = (int)_dgvUsers.CurrentRow.Cells["Id"].Value;
            var existing = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existing == null) return;

            var updated = EditUser(existing);
            if (updated != null)
            {
                existing.Username = updated.Username;
                if (!string.IsNullOrEmpty(updated.PasswordHash))
                {
                    existing.PasswordHash = updated.PasswordHash;
                }
                existing.IsSuperAdmin = updated.IsSuperAdmin;
                existing.CanManageOwners = updated.CanManageOwners;
                existing.CanManageCars = updated.CanManageCars;
                existing.CanManageServices = updated.CanManageServices;
                existing.CanManageMaintenance = updated.CanManageMaintenance;
                existing.CanManageUsers = updated.CanManageUsers;

                _context.SaveChanges();
                LoadUsers();
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_dgvUsers.CurrentRow == null) return;
            var id = (int)_dgvUsers.CurrentRow.Cells["Id"].Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return;

            if (user.IsSuperAdmin)
            {
                MessageBox.Show("Cannot delete a super admin.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var res = MessageBox.Show($"Delete user '{user.Username}'?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes) return;

            _context.Users.Remove(user);
            _context.SaveChanges();
            LoadUsers();
        }

        private User? EditUser(User? existing)
        {
            var ci = GetCulture();
            var rm = _resManager;

            var yesLabel = rm.GetString("UserManagement_Yes", ci) ?? "Yes";
            var noLabel = rm.GetString("UserManagement_No", ci) ?? "No";

            var yesNo = new List<KeyValuePair<string, string>>
            {
                new("true", yesLabel),
                new("false", noLabel)
            };

            var fields = new List<FieldSpec>
            {
                new FieldSpec { Name = "Username", Label = rm.GetString("UserManagement_Field_Username", ci) ?? "Username", Type = FieldType.Text, Value = existing?.Username ?? string.Empty },
                new FieldSpec { Name = "Password", Label = existing == null ? (rm.GetString("UserManagement_Field_Password", ci) ?? "Password") : (rm.GetString("UserManagement_Field_PasswordKeep", ci) ?? "Password (leave blank to keep)"), Type = FieldType.Text, Value = string.Empty },
                new FieldSpec { Name = "IsSuperAdmin", Label = rm.GetString("UserManagement_Field_SuperAdmin", ci) ?? "Super admin", Type = FieldType.Combo, Options = yesNo, Value = (existing?.IsSuperAdmin ?? false) ? "true" : "false" },
                new FieldSpec { Name = "CanManageOwners", Label = rm.GetString("UserManagement_Field_CanManageOwners", ci) ?? "Can manage owners", Type = FieldType.Combo, Options = yesNo, Value = (existing?.CanManageOwners ?? false) ? "true" : "false" },
                new FieldSpec { Name = "CanManageCars", Label = rm.GetString("UserManagement_Field_CanManageCars", ci) ?? "Can manage cars", Type = FieldType.Combo, Options = yesNo, Value = (existing?.CanManageCars ?? false) ? "true" : "false" },
                new FieldSpec { Name = "CanManageServices", Label = rm.GetString("UserManagement_Field_CanManageServices", ci) ?? "Can manage services", Type = FieldType.Combo, Options = yesNo, Value = (existing?.CanManageServices ?? false) ? "true" : "false" },
                new FieldSpec { Name = "CanManageMaintenance", Label = rm.GetString("UserManagement_Field_CanManageMaintenance", ci) ?? "Can manage maintenance", Type = FieldType.Combo, Options = yesNo, Value = (existing?.CanManageMaintenance ?? false) ? "true" : "false" },
                new FieldSpec { Name = "CanManageUsers", Label = rm.GetString("UserManagement_Field_CanManageUsers", ci) ?? "Can manage users", Type = FieldType.Combo, Options = yesNo, Value = (existing?.CanManageUsers ?? false) ? "true" : "false" }
            };

            using var dlg = new ModalForm(Text, fields);

            if (dlg.ShowDialog(this) != DialogResult.OK)
                return null;

            if (!dlg.Values.TryGetValue("Username", out var username) || string.IsNullOrWhiteSpace(username))
            {
                var msg = rm.GetString("UserManagement_Error_UsernameRequired", ci) ?? "Username is required.";
                MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var otherWithSameName = _context.Users.FirstOrDefault(u => u.Username == username && (existing == null || u.Id != existing.Id));
            if (otherWithSameName != null)
            {
                var msg = rm.GetString("UserManagement_Error_UsernameExists", ci) ?? "Username already exists.";
                MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            dlg.Values.TryGetValue("Password", out var password);

            bool GetBool(string key)
            {
                return dlg.Values.TryGetValue(key, out var val) && string.Equals(val, "true", StringComparison.OrdinalIgnoreCase);
            }

            var user = existing ?? new User();
            user.Username = username;
            if (!string.IsNullOrWhiteSpace(password))
            {
                user.PasswordHash = User.HashPassword(password);
            }
            user.IsSuperAdmin = GetBool("IsSuperAdmin");
            user.CanManageOwners = GetBool("CanManageOwners");
            user.CanManageCars = GetBool("CanManageCars");
            user.CanManageServices = GetBool("CanManageServices");
            user.CanManageMaintenance = GetBool("CanManageMaintenance");
            user.CanManageUsers = GetBool("CanManageUsers");

            return user;
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

            var s = rm.GetString("UserManagement_Title", ci);
            if (!string.IsNullOrEmpty(s)) Text = s;

            if (_btnAdd != null)
            {
                s = rm.GetString("UserManagement_BtnAdd", ci);
                if (!string.IsNullOrEmpty(s)) _btnAdd.Text = s;
            }
            if (_btnEdit != null)
            {
                s = rm.GetString("UserManagement_BtnEdit", ci);
                if (!string.IsNullOrEmpty(s)) _btnEdit.Text = s;
            }
            if (_btnDelete != null)
            {
                s = rm.GetString("UserManagement_BtnDelete", ci);
                if (!string.IsNullOrEmpty(s)) _btnDelete.Text = s;
            }
        }
    }
}
