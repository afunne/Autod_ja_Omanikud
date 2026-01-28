using Autod_ja_Omanikud.Data;
using Autod_ja_Omanikud.Models;
using Autod_ja_Omanikud.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;
using System.Collections.Generic;

namespace Autod_ja_Omanikud
{
    public partial class Form1 : Form
    {
        private AutoDbContext _context;
        private bool isDarkMode = false;
        private readonly User _currentUser;

        private const string LangConfigFileName = "language.config";
        private ResourceManager _resManager => Properties.Resources.ResourceManager;

        public Form1(User currentUser)
        {
            _currentUser = currentUser;

            InitializeComponent();

            // Initialize localization (must run after InitializeComponent so controls exist)
            InitLocalization();

            _context = new AutoDbContext();
            LoadAll();

            // Wire up buttons (handlers for owners/cars are in partial files too)
            btnAddOwner.Click += BtnAddOwner_Click;
            btnDeleteOwner.Click += BtnDeleteOwner_Click;
            btnUpdateOwner.Click += BtnUpdateOwner_Click;

            btnAddCar.Click += BtnAddCar_Click;
            btnDeleteCar.Click += BtnDeleteCar_Click;
            btnUpdateCar.Click += BtnUpdateCar_Click;

            btnAddService.Click += BtnAddService_Click;
            btnDeleteService.Click += BtnDeleteService_Click;
            btnUpdateService.Click += BtnUpdateService_Click;

            btnAddCarService.Click += BtnAddCarService_Click;
            btnDeleteCarService.Click += BtnDeleteCarService_Click;
            btnUpdateCarService.Click += BtnUpdateCarService_Click;

            btnRefresh.Click += BtnRefresh_Click; // Refresh button

            btnManageUsers.Click += BtnManageUsers_Click;

            ApplyPermissions();
        }

        // Parameterless constructor for designer support only
        public Form1() : this(new User
        {
            Username = "Designer",
            IsSuperAdmin = true,
            CanManageOwners = true,
            CanManageCars = true,
            CanManageServices = true,
            CanManageMaintenance = true,
            CanManageUsers = true
        })
        {
        }

        private void ApplyPermissions()
        {
            bool isSuper = _currentUser.IsSuperAdmin;

            bool canOwners = isSuper || _currentUser.CanManageOwners;
            btnAddOwner.Enabled = canOwners;
            btnDeleteOwner.Enabled = canOwners;
            btnUpdateOwner.Enabled = canOwners;

            bool canCars = isSuper || _currentUser.CanManageCars;
            btnAddCar.Enabled = canCars;
            btnDeleteCar.Enabled = canCars;
            btnUpdateCar.Enabled = canCars;

            bool canServices = isSuper || _currentUser.CanManageServices;
            btnAddService.Enabled = canServices;
            btnDeleteService.Enabled = canServices;
            btnUpdateService.Enabled = canServices;

            bool canMaintenance = isSuper || _currentUser.CanManageMaintenance;
            btnAddCarService.Enabled = canMaintenance;
            btnDeleteCarService.Enabled = canMaintenance;
            btnUpdateCarService.Enabled = canMaintenance;

            bool canUsers = isSuper || _currentUser.CanManageUsers;
            btnManageUsers.Visible = canUsers;
        }

        private void LoadAll()
        {
            LoadOwners();
            LoadCars();
            LoadServices();
            LoadCarServices();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadAll();
        }

        // ===== OWNERS =====
        private void LoadOwners()
        {
            dgvOwners.DataSource = _context.Owners.ToList();
        }

        // ===== CARS =====
        private void LoadCars()
        {
            dgvCars.DataSource = _context.Cars
                .Select(c => new
                {
                    c.Id,
                    c.Brand,
                    c.Model,
                    c.RegistrationNumber,
                    c.OwnerId
                })
                .ToList();
        }

        // ===== SERVICES (MAINTENANCE TOP GRID) =====
        private void LoadServices()
        {
            dgvServices.DataSource = _context.Services.ToList();
        }

        private void BtnAddService_Click(object sender, EventArgs e)
        {
            var fields = new List<FieldSpec>
            {
                new FieldSpec { Name = "Name",  Label = "Service name", Type = FieldType.Text,   Value = "" },
                new FieldSpec { Name = "Price", Label = "Price",        Type = FieldType.Number, Value = "" }
            };

            using var dlg = new ModalForm("Add service", fields);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            if (!dlg.Values.TryGetValue("Name", out var name) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Service name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dlg.Values.TryGetValue("Price", out var priceStr);
            var service = new Service
            {
                Name = name,
                Price = decimal.TryParse(priceStr, out var price) ? price : 0
            };

            _context.Services.Add(service);
            _context.SaveChanges();
            LoadServices();
        }

        private void BtnDeleteService_Click(object sender, EventArgs e)
        {
            if (dgvServices.CurrentRow == null) return;
            var service = _context.Services.Find((int)dgvServices.CurrentRow.Cells["Id"].Value);
            if (service == null) return;
            _context.Services.Remove(service);
            _context.SaveChanges();
            LoadServices();
        }

        private void BtnUpdateService_Click(object sender, EventArgs e)
        {
            if (dgvServices.CurrentRow == null) return;
            var service = _context.Services.Find((int)dgvServices.CurrentRow.Cells["Id"].Value);
            if (service == null) return;

            var fields = new List<FieldSpec>
            {
                new FieldSpec { Name = "Name",  Label = "Service name", Type = FieldType.Text,   Value = service.Name },
                new FieldSpec { Name = "Price", Label = "Price",        Type = FieldType.Number, Value = service.Price.ToString("0.##") }
            };

            using var dlg = new ModalForm("Update service", fields);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            if (dlg.Values.TryGetValue("Name", out var name) && !string.IsNullOrWhiteSpace(name))
                service.Name = name;

            if (dlg.Values.TryGetValue("Price", out var priceStr) &&
                decimal.TryParse(priceStr, out var price))
                service.Price = price;

            _context.SaveChanges();
            LoadServices();
        }

        // ===== CAR SERVICES (MAINTENANCE BOTTOM GRID) =====
        private void LoadCarServices()
        {
            dgvCarServices.DataSource = _context.CarServices
                .Select(cs => new
                {
                    cs.CarId,
                    cs.ServiceId,
                    cs.DateOfService,
                    cs.Mileage
                })
                .ToList();
        }

        private void BtnAddCarService_Click(object sender, EventArgs e)
        {
            OpenCarServiceModalForAdd();
        }

        private void BtnManageUsers_Click(object sender, EventArgs e)
        {
            using var dlg = new Forms.UserManagementForm();
            dlg.ShowDialog(this);
        }

        private void BtnDeleteCarService_Click(object sender, EventArgs e)
        {
            if (dgvCarServices.CurrentRow == null) return;

            var carId = (int)dgvCarServices.CurrentRow.Cells["CarId"].Value;
            var serviceId = (int)dgvCarServices.CurrentRow.Cells["ServiceId"].Value;
            var dateOfService = (DateTime)dgvCarServices.CurrentRow.Cells["DateOfService"].Value;

            var cs = _context.CarServices.Find(carId, serviceId, dateOfService);
            if (cs == null) return;

            _context.CarServices.Remove(cs);
            _context.SaveChanges();
            LoadCarServices();
        }

        private void OpenCarServiceModalForAdd()
        {
            var carOptions = _context.Cars
                .Select(c => new KeyValuePair<string, string>(
                    c.Id.ToString(),
                    $"{c.Id}: {c.Brand} {c.Model} ({c.RegistrationNumber})"
                ))
                .ToList();

            var serviceOptions = _context.Services
                .Select(s => new KeyValuePair<string, string>(
                    s.Id.ToString(),
                    $"{s.Id}: {s.Name}"
                ))
                .ToList();

            if (carOptions.Count == 0 || serviceOptions.Count == 0)
            {
                MessageBox.Show(
                    "Please add at least one car and one service before adding maintenance entries.",
                    "No data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var fields = new List<FieldSpec>
            {
                new FieldSpec { Name = "CarId",         Label = "Car",             Type = FieldType.Combo,  Options = carOptions },
                new FieldSpec { Name = "ServiceId",     Label = "Service",         Type = FieldType.Combo,  Options = serviceOptions },
                new FieldSpec { Name = "Mileage",       Label = "Mileage",         Type = FieldType.Number, Value = "" },
                new FieldSpec { Name = "DateOfService", Label = "Date of service", Type = FieldType.Date,   Value = DateTime.Today.ToShortDateString() }
            };

            using var dlg = new ModalForm("Add maintenance entry", fields);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            if (!dlg.Values.TryGetValue("CarId", out var carIdStr) ||
                !dlg.Values.TryGetValue("ServiceId", out var serviceIdStr) ||
                !dlg.Values.TryGetValue("Mileage", out var mileageStr) ||
                !dlg.Values.TryGetValue("DateOfService", out var dateStr))
            {
                MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(carIdStr, out var carId) ||
                !int.TryParse(serviceIdStr, out var serviceId) ||
                !int.TryParse(mileageStr, out var mileage) ||
                !DateTime.TryParse(dateStr, out var date))
            {
                MessageBox.Show("Invalid values. Check inputs.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cs = new CarService
            {
                CarId = carId,
                ServiceId = serviceId,
                DateOfService = date,
                Mileage = mileage
            };

            _context.CarServices.Add(cs);
            _context.SaveChanges();
            LoadCarServices();
        }

        private void OpenCarServiceModalForUpdate(CarService cs)
        {
            var carOptions = _context.Cars
                .Select(c => new KeyValuePair<string, string>(
                    c.Id.ToString(),
                    $"{c.Id}: {c.Brand} {c.Model} ({c.RegistrationNumber})"
                ))
                .ToList();

            var serviceOptions = _context.Services
                .Select(s => new KeyValuePair<string, string>(
                    s.Id.ToString(),
                    $"{s.Id}: {s.Name}"
                ))
                .ToList();

            var fields = new List<FieldSpec>
            {
                new FieldSpec { Name = "CarId",         Label = "Car",             Type = FieldType.Combo,  Options = carOptions,     Value = cs.CarId.ToString() },
                new FieldSpec { Name = "ServiceId",     Label = "Service",         Type = FieldType.Combo,  Options = serviceOptions, Value = cs.ServiceId.ToString() },
                new FieldSpec { Name = "Mileage",       Label = "Mileage",         Type = FieldType.Number, Value = cs.Mileage.ToString() },
                new FieldSpec { Name = "DateOfService", Label = "Date of service", Type = FieldType.Date,   Value = cs.DateOfService.ToShortDateString() }
            };

            using var dlg = new ModalForm("Update maintenance entry", fields);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            if (dlg.Values.TryGetValue("Mileage", out var mileageStr) &&
                int.TryParse(mileageStr, out var mileage))
                cs.Mileage = mileage;

            if (dlg.Values.TryGetValue("DateOfService", out var dateStr) &&
                DateTime.TryParse(dateStr, out var date))
                cs.DateOfService = date;

            if (dlg.Values.TryGetValue("CarId", out var carIdStr) &&
                int.TryParse(carIdStr, out var newCarId))
                cs.CarId = newCarId;

            if (dlg.Values.TryGetValue("ServiceId", out var serviceIdStr) &&
                int.TryParse(serviceIdStr, out var newServiceId))
                cs.ServiceId = newServiceId;

            _context.SaveChanges();
            LoadCarServices();
        }

        private void BtnUpdateCarService_Click(object sender, EventArgs e)
        {
            if (dgvCarServices.CurrentRow == null) return;

            var carId = (int)dgvCarServices.CurrentRow.Cells["CarId"].Value;
            var serviceId = (int)dgvCarServices.CurrentRow.Cells["ServiceId"].Value;
            var dateOfService = (DateTime)dgvCarServices.CurrentRow.Cells["DateOfService"].Value;

            var cs = _context.CarServices.Find(carId, serviceId, dateOfService);
            if (cs == null) return;

            OpenCarServiceModalForUpdate(cs);
        }

        // ===== DARK MODE =====
        private void BtnToggleDarkMode_Click(object sender, EventArgs e)
        {
            if (!isDarkMode)
            {
                this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
                tabOwners.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
                tabCars.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
                tabMaintenance.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);

                System.Drawing.Color dgvBg = System.Drawing.Color.FromArgb(50, 50, 50);
                System.Drawing.Color dgvText = System.Drawing.Color.White;
                System.Drawing.Color dgvHeader = System.Drawing.Color.FromArgb(70, 70, 70);

                foreach (var dgv in new[] { dgvOwners, dgvCars, dgvServices, dgvCarServices })
                {
                    dgv.BackgroundColor = dgvBg;
                    dgv.DefaultCellStyle.BackColor = dgvBg;
                    dgv.DefaultCellStyle.ForeColor = dgvText;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = dgvHeader;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = dgvText;
                }

                System.Drawing.Color tbBack = System.Drawing.Color.FromArgb(60, 60, 60);
                System.Drawing.Color tbFore = System.Drawing.Color.White;

                foreach (Control c in this.Controls)
                    ApplyDarkMode(c, tbBack, tbFore);

                System.Drawing.Color btnBack = System.Drawing.Color.FromArgb(70, 70, 70);
                System.Drawing.Color btnFore = System.Drawing.Color.White;

                btnRefresh.BackColor = btnBack;
                btnRefresh.ForeColor = btnFore;
                btnToggleDarkMode.BackColor = btnBack;
                btnToggleDarkMode.ForeColor = btnFore;

                isDarkMode = true;
            }
            else
            {
                this.BackColor = System.Drawing.Color.LightBlue;
                tabOwners.BackColor = System.Drawing.Color.AliceBlue;
                tabCars.BackColor = System.Drawing.Color.AliceBlue;
                tabMaintenance.BackColor = System.Drawing.Color.AliceBlue;

                System.Drawing.Color dgvHeader = System.Drawing.Color.SteelBlue;

                foreach (var dgv in new[] { dgvOwners, dgvCars, dgvServices, dgvCarServices })
                {
                    dgv.BackgroundColor = System.Drawing.Color.White;
                    dgv.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = dgvHeader;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }

                foreach (Control c in this.Controls)
                    ApplyLightMode(c);

                isDarkMode = false;
            }
        }

        private void ApplyDarkMode(Control c, System.Drawing.Color back, System.Drawing.Color fore)
        {
            if (c is TextBox || c is ComboBox || c is DateTimePicker)
            {
                c.BackColor = back;
                c.ForeColor = fore;
            }
            else if (c is Button)
            {
                c.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
                c.ForeColor = System.Drawing.Color.White;
            }
            if (c.HasChildren)
            {
                foreach (Control child in c.Controls)
                    ApplyDarkMode(child, back, fore);
            }
        }

        private void ApplyLightMode(Control c)
        {
            if (c is TextBox || c is ComboBox || c is DateTimePicker)
            {
                c.BackColor = System.Drawing.Color.White;
                c.ForeColor = System.Drawing.Color.Black;
            }
            else if (c is Button)
            {
                c.BackColor = System.Drawing.Color.SteelBlue;
                c.ForeColor = System.Drawing.Color.White;
            }
            if (c.HasChildren)
            {
                foreach (Control child in c.Controls)
                    ApplyLightMode(child);
            }
        }

        // ===== LOCALIZATION =====
        private void InitLocalization()
        {
            cmbLanguage.Items.Clear();
            cmbLanguage.Items.Add(new KeyValuePair<string, string>("en", "English"));
            cmbLanguage.Items.Add(new KeyValuePair<string, string>("et", "Eesti"));

            cmbLanguage.DisplayMember = "Value";
            cmbLanguage.ValueMember = "Key";

            var lang = LoadSavedLanguage() ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (string.IsNullOrWhiteSpace(lang)) lang = "en";

            for (int i = 0; i < cmbLanguage.Items.Count; i++)
            {
                var kv = (KeyValuePair<string, string>)cmbLanguage.Items[i];
                if (kv.Key == lang)
                {
                    cmbLanguage.SelectedIndex = i;
                    break;
                }
            }

            cmbLanguage.SelectedIndexChanged += CmbLanguage_SelectedIndexChanged;
            ApplyTranslations(lang);
        }

        private void CmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLanguage.SelectedItem is KeyValuePair<string, string> kv)
            {
                var lang = kv.Key;
                ApplyTranslations(lang);
                SaveSelectedLanguage(lang);
            }
        }

        private string? LoadSavedLanguage()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LangConfigFileName);
                if (File.Exists(path))
                {
                    var text = File.ReadAllText(path).Trim();
                    if (!string.IsNullOrEmpty(text)) return text;
                }
            }
            catch { /* ignore */ }
            return null;
        }

        private void SaveSelectedLanguage(string lang)
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LangConfigFileName);
                File.WriteAllText(path, lang);
            }
            catch { /* ignore */ }
        }

        private void ApplyTranslations(string lang)
        {
            CultureInfo ci;
            try { ci = new CultureInfo(lang); }
            catch { ci = CultureInfo.InvariantCulture; }

            var rm = _resManager;

            // Form title
            var title = rm.GetString("Form1_Title", ci);
            if (!string.IsNullOrEmpty(title)) this.Text = title;

            // Buttons
            var s = rm.GetString("btnRefresh", ci);
            if (!string.IsNullOrEmpty(s)) btnRefresh.Text = s;

            s = rm.GetString("btnToggleDarkMode", ci);
            if (!string.IsNullOrEmpty(s)) btnToggleDarkMode.Text = s;

            // Tabs
            s = rm.GetString("tabOwners", ci);
            if (!string.IsNullOrEmpty(s)) tabOwners.Text = s;
            s = rm.GetString("tabCars", ci);
            if (!string.IsNullOrEmpty(s)) tabCars.Text = s;
            s = rm.GetString("tabMaintenance", ci);
            if (!string.IsNullOrEmpty(s)) tabMaintenance.Text = s;

            // Owners buttons (owner textboxes were removed)
            s = rm.GetString("btnAddOwner", ci);
            if (!string.IsNullOrEmpty(s)) btnAddOwner.Text = s;
            s = rm.GetString("btnDeleteOwner", ci);
            if (!string.IsNullOrEmpty(s)) btnDeleteOwner.Text = s;
            s = rm.GetString("btnUpdateOwner", ci);
            if (!string.IsNullOrEmpty(s)) btnUpdateOwner.Text = s;

            // Cars controls (text fields removed, only buttons remain)
            s = rm.GetString("btnAddCar", ci);
            if (!string.IsNullOrEmpty(s)) btnAddCar.Text = s;
            s = rm.GetString("btnDeleteCar", ci);
            if (!string.IsNullOrEmpty(s)) btnDeleteCar.Text = s;
            s = rm.GetString("btnUpdateCar", ci);
            if (!string.IsNullOrEmpty(s)) btnUpdateCar.Text = s;

            // Services controls (text fields removed, only buttons remain)
            s = rm.GetString("btnAddService", ci);
            if (!string.IsNullOrEmpty(s)) btnAddService.Text = s;
            s = rm.GetString("btnDeleteService", ci);
            if (!string.IsNullOrEmpty(s)) btnDeleteService.Text = s;
            s = rm.GetString("btnUpdateService", ci);
            if (!string.IsNullOrEmpty(s)) btnUpdateService.Text = s;

            // Car service entries (mileage text field removed)
            s = rm.GetString("btnAddCarService", ci);
            if (!string.IsNullOrEmpty(s)) btnAddCarService.Text = s;
            s = rm.GetString("btnDeleteCarService", ci);
            if (!string.IsNullOrEmpty(s)) btnDeleteCarService.Text = s;
            s = rm.GetString("btnUpdateCarService", ci);
            if (!string.IsNullOrEmpty(s)) btnUpdateCarService.Text = s;
        }
    }
}