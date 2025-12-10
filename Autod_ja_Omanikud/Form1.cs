using Autod_ja_Omanikud.Data;
using Autod_ja_Omanikud.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Autod_ja_Omanikud
{
    public partial class Form1 : Form
    {
        private AutoDbContext _context;

        public Form1()
        {
            InitializeComponent();
            _context = new AutoDbContext();
            LoadAll();

            // Wire up buttons
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

        private void BtnAddOwner_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOwnerName.Text)) return;
            var owner = new Owner { FullName = txtOwnerName.Text, Phone = txtOwnerPhone.Text };
            _context.Owners.Add(owner);
            _context.SaveChanges();
            LoadOwners();
        }

        private void BtnDeleteOwner_Click(object sender, EventArgs e)
        {
            if (dgvOwners.CurrentRow == null) return;
            var owner = _context.Owners.Find((int)dgvOwners.CurrentRow.Cells["Id"].Value);
            if (owner == null) return;
            _context.Owners.Remove(owner);
            _context.SaveChanges();
            LoadOwners();
        }

        private void BtnUpdateOwner_Click(object sender, EventArgs e)
        {
            if (dgvOwners.CurrentRow == null) return;
            var owner = _context.Owners.Find((int)dgvOwners.CurrentRow.Cells["Id"].Value);
            if (owner == null) return;
            owner.FullName = txtOwnerName.Text;
            owner.Phone = txtOwnerPhone.Text;
            _context.SaveChanges();
            LoadOwners();
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

        private void BtnAddCar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCarBrand.Text) || cmbCarOwner.SelectedValue == null) return;
            var car = new Car
            {
                Brand = txtCarBrand.Text,
                Model = txtCarModel.Text,
                RegistrationNumber = txtCarReg.Text,
                OwnerId = (int)cmbCarOwner.SelectedValue
            };
            _context.Cars.Add(car);
            _context.SaveChanges();
            LoadCars();
        }

        private void BtnDeleteCar_Click(object sender, EventArgs e)
        {
            if (dgvCars.CurrentRow == null) return;
            var car = _context.Cars.Find((int)dgvCars.CurrentRow.Cells["Id"].Value);
            if (car == null) return;
            _context.Cars.Remove(car);
            _context.SaveChanges();
            LoadCars();
        }

        private void BtnUpdateCar_Click(object sender, EventArgs e)
        {
            if (dgvCars.CurrentRow == null) return;
            var car = _context.Cars.Find((int)dgvCars.CurrentRow.Cells["Id"].Value);
            if (car == null) return;
            car.Brand = txtCarBrand.Text;
            car.Model = txtCarModel.Text;
            car.RegistrationNumber = txtCarReg.Text;
            car.OwnerId = (int)cmbCarOwner.SelectedValue;
            _context.SaveChanges();
            LoadCars();
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
                this.BackColor = Color.FromArgb(30, 30, 30);
                tabOwners.BackColor = Color.FromArgb(45, 45, 45);
                tabCars.BackColor = Color.FromArgb(45, 45, 45);
                tabMaintenance.BackColor = Color.FromArgb(45, 45, 45);

                // DataGridViews
                Color dgvBg = Color.FromArgb(50, 50, 50);
                Color dgvText = Color.White;
                Color dgvHeader = Color.FromArgb(70, 70, 70);

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
                Color tbBack = Color.FromArgb(60, 60, 60);
                Color tbFore = Color.White;

                foreach (Control c in this.Controls)
                {
                    ApplyDarkMode(c, tbBack, tbFore);
                }

                // Buttons
                Color btnBack = Color.FromArgb(70, 70, 70);
                Color btnFore = Color.White;

                btnRefresh.BackColor = btnBack;
                btnRefresh.ForeColor = btnFore;
                btnToggleDarkMode.BackColor = btnBack;
                btnToggleDarkMode.ForeColor = btnFore;

                isDarkMode = true;
            }
            else
            {
                // Light/Blue Mode
                this.BackColor = Color.LightBlue;
                tabOwners.BackColor = Color.AliceBlue;
                tabCars.BackColor = Color.AliceBlue;
                tabMaintenance.BackColor = Color.AliceBlue;

                Color dgvHeader = Color.SteelBlue;

                foreach (DataGridView dgv in new[] { dgvOwners, dgvCars, dgvServices, dgvCarServices })
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.DefaultCellStyle.BackColor = Color.White;
                    dgv.DefaultCellStyle.ForeColor = Color.Black;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = dgvHeader;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                }

                foreach (Control c in this.Controls)
                {
                    ApplyLightMode(c);
                }

                isDarkMode = false;
            }
        }

        // Recursive helper for dark mode on all controls
        private void ApplyDarkMode(Control c, Color back, Color fore)
        {
            if (c is TextBox || c is ComboBox || c is DateTimePicker)
            {
                c.BackColor = back;
                c.ForeColor = fore;
            }
            else if (c is Button)
            {
                c.BackColor = Color.FromArgb(70, 70, 70);
                c.ForeColor = Color.White;
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
                c.BackColor = Color.White;
                c.ForeColor = Color.Black;
            }
            else if (c is Button)
            {
                c.BackColor = Color.SteelBlue;
                c.ForeColor = Color.White;
            }
            if (c.HasChildren)
            {
                foreach (Control child in c.Controls)
                    ApplyLightMode(child);
            }
        }
    }
}
