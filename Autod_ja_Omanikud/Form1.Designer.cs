namespace Autod_ja_Omanikud
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private RoundedButton btnRefresh;
        private RoundedButton btnToggleDarkMode;

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabOwners;
        private System.Windows.Forms.TabPage tabCars;
        private System.Windows.Forms.TabPage tabMaintenance;

        // Owners
        private System.Windows.Forms.DataGridView dgvOwners;
        private System.Windows.Forms.TextBox txtOwnerName;
        private System.Windows.Forms.TextBox txtOwnerPhone;
        private RoundedButton btnAddOwner;
        private RoundedButton btnDeleteOwner;
        private RoundedButton btnUpdateOwner;

        // Cars
        private System.Windows.Forms.DataGridView dgvCars;
        private System.Windows.Forms.TextBox txtCarBrand;
        private System.Windows.Forms.TextBox txtCarModel;
        private System.Windows.Forms.TextBox txtCarReg;
        private System.Windows.Forms.ComboBox cmbCarOwner;
        private RoundedButton btnAddCar;
        private RoundedButton btnDeleteCar;
        private RoundedButton btnUpdateCar;

        // Services
        private System.Windows.Forms.DataGridView dgvServices;
        private System.Windows.Forms.TextBox txtServiceName;
        private System.Windows.Forms.TextBox txtServicePrice;
        private RoundedButton btnAddService;
        private RoundedButton btnDeleteService;
        private RoundedButton btnUpdateService;

        // Car Services / Maintenance Entries
        private System.Windows.Forms.DataGridView dgvCarServices;
        private System.Windows.Forms.ComboBox cmbCarForService;
        private System.Windows.Forms.ComboBox cmbServiceForCar;
        private System.Windows.Forms.DateTimePicker dtpServiceDate;
        private System.Windows.Forms.TextBox txtMileage;
        private RoundedButton btnAddCarService;
        private RoundedButton btnUpdateCarService;

        // Language selector
        private System.Windows.Forms.ComboBox cmbLanguage;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnRefresh = new RoundedButton();
            this.btnToggleDarkMode = new RoundedButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabOwners = new System.Windows.Forms.TabPage();
            this.tabCars = new System.Windows.Forms.TabPage();
            this.tabMaintenance = new System.Windows.Forms.TabPage();

            // Language combo
            this.cmbLanguage = new System.Windows.Forms.ComboBox();

            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Text = "Auto Manager";
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.BackColor = System.Drawing.Color.LightBlue;

            // ===== Refresh Button =====
            this.btnRefresh.Location = new System.Drawing.Point(850, 10);
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;

            // ===== Dark Mode Toggle Button =====
            this.btnToggleDarkMode.Location = new System.Drawing.Point(730, 10);
            this.btnToggleDarkMode.Size = new System.Drawing.Size(100, 30);
            this.btnToggleDarkMode.Text = "Dark Mode";
            this.btnToggleDarkMode.BackColor = System.Drawing.Color.Gray;
            this.btnToggleDarkMode.ForeColor = System.Drawing.Color.White;
            this.btnToggleDarkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleDarkMode.FlatAppearance.BorderSize = 0;
            this.btnToggleDarkMode.Click += BtnToggleDarkMode_Click;

            // ===== Language ComboBox =====
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.Location = new System.Drawing.Point(610, 10);
            this.cmbLanguage.Size = new System.Drawing.Size(110, 30);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.TabIndex = 3;
            // Items populated at runtime in InitLocalization()

            // ===== TabControl =====
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Controls.Add(this.tabOwners);
            this.tabControl.Controls.Add(this.tabCars);
            this.tabControl.Controls.Add(this.tabMaintenance);

            this.tabOwners.BackColor = System.Drawing.Color.AliceBlue;
            this.tabCars.BackColor = System.Drawing.Color.AliceBlue;
            this.tabMaintenance.BackColor = System.Drawing.Color.AliceBlue;

            this.Controls.AddRange(new System.Windows.Forms.Control[] { btnRefresh, btnToggleDarkMode, cmbLanguage, tabControl });

            // ===== OWNERS TAB =====
            this.tabOwners.Text = "Owners";
            this.dgvOwners = new System.Windows.Forms.DataGridView() { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(960, 300) };
            this.dgvOwners.EnableHeadersVisualStyles = false;
            this.dgvOwners.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
            this.dgvOwners.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvOwners.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 230, 250);
            this.dgvOwners.RowHeadersVisible = false;
            this.dgvOwners.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.txtOwnerName = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(10, 320), Width = 200, PlaceholderText = "Full Name", BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };
            this.txtOwnerPhone = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(220, 320), Width = 150, PlaceholderText = "Phone", BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };

            this.btnAddOwner = new RoundedButton() { Location = new System.Drawing.Point(380, 320), Text = "Add Owner", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };
            this.btnDeleteOwner = new RoundedButton() { Location = new System.Drawing.Point(500, 320), Text = "Delete", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };
            this.btnUpdateOwner = new RoundedButton() { Location = new System.Drawing.Point(620, 320), Text = "Update", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };

            this.tabOwners.Controls.AddRange(new System.Windows.Forms.Control[] { dgvOwners, txtOwnerName, txtOwnerPhone, btnAddOwner, btnDeleteOwner, btnUpdateOwner });

            // ===== CARS TAB =====
            this.tabCars.Text = "Cars";
            this.dgvCars = new System.Windows.Forms.DataGridView() { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(960, 300) };
            this.dgvCars.EnableHeadersVisualStyles = false;
            this.dgvCars.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
            this.dgvCars.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvCars.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 230, 250);
            this.dgvCars.RowHeadersVisible = false;
            this.dgvCars.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.txtCarBrand = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(10, 320), Width = 150, PlaceholderText = "Brand", BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };
            this.txtCarModel = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(170, 320), Width = 150, PlaceholderText = "Model", BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };
            this.txtCarReg = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(330, 320), Width = 150, PlaceholderText = "Registration", BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };
            this.cmbCarOwner = new System.Windows.Forms.ComboBox() { Location = new System.Drawing.Point(490, 320), Width = 200, BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };

            this.btnAddCar = new RoundedButton() { Location = new System.Drawing.Point(700, 320), Text = "Add Car", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };
            this.btnDeleteCar = new RoundedButton() { Location = new System.Drawing.Point(820, 320), Text = "Delete", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };
            this.btnUpdateCar = new RoundedButton() { Location = new System.Drawing.Point(700, 360), Text = "Update", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };

            this.tabCars.Controls.AddRange(new System.Windows.Forms.Control[] { dgvCars, txtCarBrand, txtCarModel, txtCarReg, cmbCarOwner, btnAddCar, btnDeleteCar, btnUpdateCar });

            // ===== MAINTENANCE TAB =====
            this.tabMaintenance.Text = "Maintenance";
            this.dgvServices = new System.Windows.Forms.DataGridView() { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(960, 200) };
            this.dgvServices.EnableHeadersVisualStyles = false;
            this.dgvServices.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
            this.dgvServices.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvServices.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 230, 250);
            this.dgvServices.RowHeadersVisible = false;
            this.dgvServices.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.txtServiceName = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(10, 220), Width = 200, PlaceholderText = "Service Name", BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };
            this.txtServicePrice = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(220, 220), Width = 100, PlaceholderText = "Price", BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };

            this.btnAddService = new RoundedButton() { Location = new System.Drawing.Point(340, 220), Text = "Add", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };
            this.btnDeleteService = new RoundedButton() { Location = new System.Drawing.Point(440, 220), Text = "Delete", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };
            this.btnUpdateService = new RoundedButton() { Location = new System.Drawing.Point(540, 220), Text = "Update", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };

            this.dgvCarServices = new System.Windows.Forms.DataGridView() { Location = new System.Drawing.Point(10, 260), Size = new System.Drawing.Size(960, 200) };
            this.dgvCarServices.EnableHeadersVisualStyles = false;
            this.dgvCarServices.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
            this.dgvCarServices.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvCarServices.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 230, 250);
            this.dgvCarServices.RowHeadersVisible = false;
            this.dgvCarServices.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.cmbCarForService = new System.Windows.Forms.ComboBox() { Location = new System.Drawing.Point(10, 470), Width = 200, BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };
            this.cmbServiceForCar = new System.Windows.Forms.ComboBox() { Location = new System.Drawing.Point(220, 470), Width = 200, BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };
            this.dtpServiceDate = new System.Windows.Forms.DateTimePicker() { Location = new System.Drawing.Point(430, 470), Width = 150, CalendarForeColor = System.Drawing.Color.Black, CalendarMonthBackground = System.Drawing.Color.White };
            this.txtMileage = new System.Windows.Forms.TextBox() { Location = new System.Drawing.Point(590, 470), Width = 100, PlaceholderText = "Mileage", BackColor = System.Drawing.Color.White, ForeColor = System.Drawing.Color.Black };
            this.btnAddCarService = new RoundedButton() { Location = new System.Drawing.Point(700, 470), Text = "Add Entry", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };
            this.btnUpdateCarService = new RoundedButton() { Location = new System.Drawing.Point(820, 470), Text = "Update Entry", BackColor = System.Drawing.Color.SteelBlue, ForeColor = System.Drawing.Color.White };

            this.tabMaintenance.Controls.AddRange(new System.Windows.Forms.Control[] {
                dgvServices, txtServiceName, txtServicePrice, btnAddService, btnDeleteService, btnUpdateService,
                dgvCarServices, cmbCarForService, cmbServiceForCar, dtpServiceDate, txtMileage, btnAddCarService, btnUpdateCarService
            });

            // Add tabs to tab control (already done above via Controls.Add)
            // The RoundedButton class is defined after.

        }

        // Rounded Button Class
        public class RoundedButton : System.Windows.Forms.Button
        {
            protected override void OnPaint(System.Windows.Forms.PaintEventArgs pevent)
            {
                using (var g = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    g.AddArc(0, 0, 20, 20, 180, 90);
                    g.AddArc(this.Width - 20, 0, 20, 20, 270, 90);
                    g.AddArc(this.Width - 20, this.Height - 20, 20, 20, 0, 90);
                    g.AddArc(0, this.Height - 20, 20, 20, 90, 90);
                    g.CloseFigure();
                    this.Region = new System.Drawing.Region(g);
                }
                base.OnPaint(pevent);
            }
        }
    }
}