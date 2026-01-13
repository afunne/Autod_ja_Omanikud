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
using System.Reflection;
using System.Collections.Generic;

namespace Autod_ja_Omanikud
{
    public partial class Form1 : Form
    {
        private AutoDbContext _context;

        public Form1()
        {
            InitializeComponent();

            // Initialize localization (must run after InitializeComponent so controls exist)
            InitLocalization();

            _context = new AutoDbContext();
            LoadAll();

            // Wire up buttons (handlers moved to partial files: OwnerHandlers.cs and CarHandlers.cs)
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
            btnUpdateCarService.Click += BtnUpdateCarService_Click;

            btnRefresh.Click += BtnRefresh_Click; // Refresh button
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

            cmbCarOwner.DataSource = _context.Owners.ToList();
            cmbCarOwner.DisplayMember = "FullName";
            cmbCarOwner.ValueMember = "Id";
        }

        // ===== SERVICES =====
        private void LoadServices()
        {
            dgvServices.DataSource = _context.Services.ToList();

            cmbServiceForCar.DataSource = _context.Services.ToList();
            cmbServiceForCar.DisplayMember = "Id";
            cmbServiceForCar.ValueMember = "Id";
        }

        private void BtnAddService_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServiceName.Text)) return;
            var service = new Service
            {
                Name = txtServiceName.Text,
                Price = decimal.TryParse(txtServicePrice.Text, out var price) ? price : 0
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
            service.Name = txtServiceName.Text;
            service.Price = decimal.TryParse(txtServicePrice.Text, out var price) ? price : service.Price;
            _context.SaveChanges();
            LoadServices();
        }

        // ===== CAR SERVICES =====
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

            cmbCarForService.DataSource = _context.Cars.ToList();
            cmbCarForService.DisplayMember = "Id";
            cmbCarForService.ValueMember = "Id";

            cmbServiceForCar.DataSource = _context.Services.ToList();
            cmbServiceForCar.DisplayMember = "Id";
            cmbServiceForCar.ValueMember = "Id";
        }

        private void BtnAddCarService_Click(object sender, EventArgs e)
        {
            if (cmbCarForService.SelectedValue == null || cmbServiceForCar.SelectedValue == null) return;
            var cs = new CarService
            {
                CarId = (int)cmbCarForService.SelectedValue,
                ServiceId = (int)cmbServiceForCar.SelectedValue,
                DateOfService = dtpServiceDate.Value,
                Mileage = int.TryParse(txtMileage.Text, out var m) ? m : 0
            };
            _context.CarServices.Add(cs);
            _context.SaveChanges();
            LoadCarServices();
        }

        private void BtnUpdateCarService_Click(object sender, EventArgs e)
        {
            if (dgvCarServices.CurrentRow == null) return;
            var cs = _context.CarServices.Find(
                (int)dgvCarServices.CurrentRow.Cells["CarId"].Value,
                (int)dgvCarServices.CurrentRow.Cells["ServiceId"].Value,
                (DateTime)dgvCarServices.CurrentRow.Cells["DateOfService"].Value
            );
            if (cs == null) return;
            cs.Mileage = int.TryParse(txtMileage.Text, out var m) ? m : cs.Mileage;
            _context.SaveChanges();
            LoadCarServices();
        }

        private bool isDarkMode = false;

        private void BtnToggleDarkMode_Click(object sender, EventArgs e)
        {
            if (!isDarkMode)
            {
                // Dark Mode
                this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
                tabOwners.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
                tabCars.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
                tabMaintenance.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);

                // DataGridViews
                System.Drawing.Color dgvBg = System.Drawing.Color.FromArgb(50, 50, 50);
                System.Drawing.Color dgvText = System.Drawing.Color.White;
                System.Drawing.Color dgvHeader = System.Drawing.Color.FromArgb(70, 70, 70);

                dgvOwners.BackgroundColor = dgvBg;
                dgvOwners.DefaultCellStyle.BackColor = dgvBg;
                dgvOwners.DefaultCellStyle.ForeColor = dgvText;
                dgvOwners.ColumnHeadersDefaultCellStyle.BackColor = dgvHeader;
                dgvOwners.ColumnHeadersDefaultCellStyle.ForeColor = dgvText;

                dgvCars.BackgroundColor = dgvBg;
                dgvCars.DefaultCellStyle.BackColor = dgvBg;
                dgvCars.DefaultCellStyle.ForeColor = dgvText;
                dgvCars.ColumnHeadersDefaultCellStyle.BackColor = dgvHeader;
                dgvCars.ColumnHeadersDefaultCellStyle.ForeColor = dgvText;

                dgvServices.BackgroundColor = dgvBg;
                dgvServices.DefaultCellStyle.BackColor = dgvBg;
                dgvServices.DefaultCellStyle.ForeColor = dgvText;
                dgvServices.ColumnHeadersDefaultCellStyle.BackColor = dgvHeader;
                dgvServices.ColumnHeadersDefaultCellStyle.ForeColor = dgvText;

                dgvCarServices.BackgroundColor = dgvBg;
                dgvCarServices.DefaultCellStyle.BackColor = dgvBg;
                dgvCarServices.DefaultCellStyle.ForeColor = dgvText;
                dgvCarServices.ColumnHeadersDefaultCellStyle.BackColor = dgvHeader;
                dgvCarServices.ColumnHeadersDefaultCellStyle.ForeColor = dgvText;

                // TextBoxes and ComboBoxes
                System.Drawing.Color tbBack = System.Drawing.Color.FromArgb(60, 60, 60);
                System.Drawing.Color tbFore = System.Drawing.Color.White;

                foreach (Control c in this.Controls)
                {
                    ApplyDarkMode(c, tbBack, tbFore);
                }

                // Buttons
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
                // Light/Blue Mode
                this.BackColor = System.Drawing.Color.LightBlue;
                tabOwners.BackColor = System.Drawing.Color.AliceBlue;
                tabCars.BackColor = System.Drawing.Color.AliceBlue;
                tabMaintenance.BackColor = System.Drawing.Color.AliceBlue;

                System.Drawing.Color dgvHeader = System.Drawing.Color.SteelBlue;

                foreach (DataGridView dgv in new[] { dgvOwners, dgvCars, dgvServices, dgvCarServices })
                {
                    dgv.BackgroundColor = System.Drawing.Color.White;
                    dgv.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = dgvHeader;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }

                foreach (Control c in this.Controls)
                {
                    ApplyLightMode(c);
                }

                isDarkMode = false;
            }
        }

        // Recursive helper for dark mode on all controls
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

        // Recursive helper for light mode
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

        // --- Localization helpers ---
        private const string LangConfigFileName = "language.config";

        private ResourceManager _resManager => Properties.Resources.ResourceManager;

        // Call this from the constructor after InitializeComponent()
        private void InitLocalization()
        {
            // populate language combo (value = culture code)
            cmbLanguage.Items.Clear();
            cmbLanguage.Items.Add(new KeyValuePair<string, string>("en", "English"));
            cmbLanguage.Items.Add(new KeyValuePair<string, string>("et", "Eesti"));

            cmbLanguage.DisplayMember = "Value";
            cmbLanguage.ValueMember = "Key";

            // load saved language or default to system language
            var lang = LoadSavedLanguage() ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (string.IsNullOrWhiteSpace(lang)) lang = "en";

            // select in combo and apply
            for (int i = 0; i < cmbLanguage.Items.Count; i++)
            {
                var kv = (KeyValuePair<string, string>)cmbLanguage.Items[i];
                if (kv.Key == lang)
                {
                    cmbLanguage.SelectedIndex = i;
                    break;
                }
            }

            // event
            cmbLanguage.SelectedIndexChanged += CmbLanguage_SelectedIndexChanged;

            // apply initial translations
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

        /// <summary>
        /// Apply translations for the given two-letter language code (e.g., "en", "et").
        /// Add/extend keys in the Resources files as needed.
        /// </summary>
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

            // Owners controls
            s = rm.GetString("txtOwnerName_Placeholder", ci);
            if (!string.IsNullOrEmpty(s)) txtOwnerName.PlaceholderText = s;
            s = rm.GetString("txtOwnerPhone_Placeholder", ci);
            if (!string.IsNullOrEmpty(s)) txtOwnerPhone.PlaceholderText = s;

            s = rm.GetString("btnAddOwner", ci);
            if (!string.IsNullOrEmpty(s)) btnAddOwner.Text = s;
            s = rm.GetString("btnDeleteOwner", ci);
            if (!string.IsNullOrEmpty(s)) btnDeleteOwner.Text = s;
            s = rm.GetString("btnUpdateOwner", ci);
            if (!string.IsNullOrEmpty(s)) btnUpdateOwner.Text = s;

            // Cars controls
            s = rm.GetString("txtCarBrand_Placeholder", ci);
            if (!string.IsNullOrEmpty(s)) txtCarBrand.PlaceholderText = s;
            s = rm.GetString("txtCarModel_Placeholder", ci);
            if (!string.IsNullOrEmpty(s)) txtCarModel.PlaceholderText = s;
            s = rm.GetString("txtCarReg_Placeholder", ci);
            if (!string.IsNullOrEmpty(s)) txtCarReg.PlaceholderText = s;
            s = rm.GetString("btnAddCar", ci);
            if (!string.IsNullOrEmpty(s)) btnAddCar.Text = s;
            s = rm.GetString("btnDeleteCar", ci);
            if (!string.IsNullOrEmpty(s)) btnDeleteCar.Text = s;
            s = rm.GetString("btnUpdateCar", ci);
            if (!string.IsNullOrEmpty(s)) btnUpdateCar.Text = s;

            // Services controls
            s = rm.GetString("txtServiceName_Placeholder", ci);
            if (!string.IsNullOrEmpty(s)) txtServiceName.PlaceholderText = s;
            s = rm.GetString("txtServicePrice_Placeholder", ci);
            if (!string.IsNullOrEmpty(s)) txtServicePrice.PlaceholderText = s;
            s = rm.GetString("btnAddService", ci);
            if (!string.IsNullOrEmpty(s)) btnAddService.Text = s;
            s = rm.GetString("btnDeleteService", ci);
            if (!string.IsNullOrEmpty(s)) btnDeleteService.Text = s;
            s = rm.GetString("btnUpdateService", ci);
            if (!string.IsNullOrEmpty(s)) btnUpdateService.Text = s;

            // Car service entries
            s = rm.GetString("txtMileage_Placeholder", ci);
            if (!string.IsNullOrEmpty(s)) txtMileage.PlaceholderText = s;
            s = rm.GetString("btnAddCarService", ci);
            if (!string.IsNullOrEmpty(s)) btnAddCarService.Text = s;
            s = rm.GetString("btnUpdateCarService", ci);
            if (!string.IsNullOrEmpty(s)) btnUpdateCarService.Text = s;

            // Note: DataGridView column headers / dynamic content must be updated where you set them (e.g., after setting DataSource).
        }
    }
}