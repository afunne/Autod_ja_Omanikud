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

            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Text = "Auto Manager";
            this.Font = new Font("Segoe UI", 10);
            this.BackColor = Color.LightBlue;

            // ===== Refresh Button =====
            this.btnRefresh.Location = new Point(850, 10);
            this.btnRefresh.Size = new Size(100, 30);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.BackColor = Color.SteelBlue;
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;

            // ===== Dark Mode Toggle Button =====
            this.btnToggleDarkMode.Location = new Point(730, 10);
            this.btnToggleDarkMode.Size = new Size(100, 30);
            this.btnToggleDarkMode.Text = "Dark Mode";
            this.btnToggleDarkMode.BackColor = Color.Gray;
            this.btnToggleDarkMode.ForeColor = Color.White;
            this.btnToggleDarkMode.FlatStyle = FlatStyle.Flat;
            this.btnToggleDarkMode.FlatAppearance.BorderSize = 0;
            this.btnToggleDarkMode.Click += BtnToggleDarkMode_Click;

            // ===== TabControl =====
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Controls.Add(this.tabOwners);
            this.tabControl.Controls.Add(this.tabCars);
            this.tabControl.Controls.Add(this.tabMaintenance);

            this.tabOwners.BackColor = Color.AliceBlue;
            this.tabCars.BackColor = Color.AliceBlue;
            this.tabMaintenance.BackColor = Color.AliceBlue;

            this.Controls.AddRange(new Control[] { btnRefresh, btnToggleDarkMode, tabControl });

            // ===== OWNERS TAB =====
            this.tabOwners.Text = "Owners";
            this.dgvOwners = new DataGridView() { Location = new Point(10, 10), Size = new Size(960, 300) };
            this.dgvOwners.EnableHeadersVisualStyles = false;
            this.dgvOwners.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            this.dgvOwners.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvOwners.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 250);
            this.dgvOwners.RowHeadersVisible = false;
            this.dgvOwners.BorderStyle = BorderStyle.None;

            this.txtOwnerName = new TextBox() { Location = new Point(10, 320), Width = 200, PlaceholderText = "Full Name", BackColor = Color.White, ForeColor = Color.Black };
            this.txtOwnerPhone = new TextBox() { Location = new Point(220, 320), Width = 150, PlaceholderText = "Phone", BackColor = Color.White, ForeColor = Color.Black };

            this.btnAddOwner = new RoundedButton() { Location = new Point(380, 320), Text = "Add Owner", BackColor = Color.SteelBlue, ForeColor = Color.White };
            this.btnDeleteOwner = new RoundedButton() { Location = new Point(500, 320), Text = "Delete", BackColor = Color.SteelBlue, ForeColor = Color.White };
            this.btnUpdateOwner = new RoundedButton() { Location = new Point(620, 320), Text = "Update", BackColor = Color.SteelBlue, ForeColor = Color.White };

            this.tabOwners.Controls.AddRange(new Control[] { dgvOwners, txtOwnerName, txtOwnerPhone, btnAddOwner, btnDeleteOwner, btnUpdateOwner });

            // ===== CARS TAB =====
            this.tabCars.Text = "Cars";
            this.dgvCars = new DataGridView() { Location = new Point(10, 10), Size = new Size(960, 300) };
            this.dgvCars.EnableHeadersVisualStyles = false;
            this.dgvCars.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            this.dgvCars.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvCars.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 250);
            this.dgvCars.RowHeadersVisible = false;
            this.dgvCars.BorderStyle = BorderStyle.None;

            this.txtCarBrand = new TextBox() { Location = new Point(10, 320), Width = 150, PlaceholderText = "Brand", BackColor = Color.White, ForeColor = Color.Black };
            this.txtCarModel = new TextBox() { Location = new Point(170, 320), Width = 150, PlaceholderText = "Model", BackColor = Color.White, ForeColor = Color.Black };
            this.txtCarReg = new TextBox() { Location = new Point(330, 320), Width = 150, PlaceholderText = "Registration", BackColor = Color.White, ForeColor = Color.Black };
            this.cmbCarOwner = new ComboBox() { Location = new Point(490, 320), Width = 200, BackColor = Color.White, ForeColor = Color.Black };

            this.btnAddCar = new RoundedButton() { Location = new Point(700, 320), Text = "Add Car", BackColor = Color.SteelBlue, ForeColor = Color.White };
            this.btnDeleteCar = new RoundedButton() { Location = new Point(820, 320), Text = "Delete", BackColor = Color.SteelBlue, ForeColor = Color.White };
            this.btnUpdateCar = new RoundedButton() { Location = new Point(700, 360), Text = "Update", BackColor = Color.SteelBlue, ForeColor = Color.White };

            this.tabCars.Controls.AddRange(new Control[] { dgvCars, txtCarBrand, txtCarModel, txtCarReg, cmbCarOwner, btnAddCar, btnDeleteCar, btnUpdateCar });

            // ===== MAINTENANCE TAB =====
            this.tabMaintenance.Text = "Maintenance";
            this.dgvServices = new DataGridView() { Location = new Point(10, 10), Size = new Size(960, 200) };
            this.dgvServices.EnableHeadersVisualStyles = false;
            this.dgvServices.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            this.dgvServices.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvServices.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 250);
            this.dgvServices.RowHeadersVisible = false;
            this.dgvServices.BorderStyle = BorderStyle.None;

            this.txtServiceName = new TextBox() { Location = new Point(10, 220), Width = 200, PlaceholderText = "Service Name", BackColor = Color.White, ForeColor = Color.Black };
            this.txtServicePrice = new TextBox() { Location = new Point(220, 220), Width = 100, PlaceholderText = "Price", BackColor = Color.White, ForeColor = Color.Black };

            this.btnAddService = new RoundedButton() { Location = new Point(340, 220), Text = "Add", BackColor = Color.SteelBlue, ForeColor = Color.White };
            this.btnDeleteService = new RoundedButton() { Location = new Point(440, 220), Text = "Delete", BackColor = Color.SteelBlue, ForeColor = Color.White };
            this.btnUpdateService = new RoundedButton() { Location = new Point(540, 220), Text = "Update", BackColor = Color.SteelBlue, ForeColor = Color.White };

            this.dgvCarServices = new DataGridView() { Location = new Point(10, 260), Size = new Size(960, 200) };
            this.dgvCarServices.EnableHeadersVisualStyles = false;
            this.dgvCarServices.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            this.dgvCarServices.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvCarServices.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 250);
            this.dgvCarServices.RowHeadersVisible = false;
            this.dgvCarServices.BorderStyle = BorderStyle.None;

            this.cmbCarForService = new ComboBox() { Location = new Point(10, 470), Width = 200, BackColor = Color.White, ForeColor = Color.Black };
            this.cmbServiceForCar = new ComboBox() { Location = new Point(220, 470), Width = 200, BackColor = Color.White, ForeColor = Color.Black };
            this.dtpServiceDate = new DateTimePicker() { Location = new Point(430, 470), Width = 150, CalendarForeColor = Color.Black, CalendarMonthBackground = Color.White };
            this.txtMileage = new TextBox() { Location = new Point(590, 470), Width = 100, PlaceholderText = "Mileage", BackColor = Color.White, ForeColor = Color.Black };
            this.btnAddCarService = new RoundedButton() { Location = new Point(700, 470), Text = "Add Entry", BackColor = Color.SteelBlue, ForeColor = Color.White };
            this.btnUpdateCarService = new RoundedButton() { Location = new Point(820, 470), Text = "Update Entry", BackColor = Color.SteelBlue, ForeColor = Color.White };

            this.tabMaintenance.Controls.AddRange(new Control[] { dgvServices, txtServiceName, txtServicePrice, btnAddService, btnDeleteService, btnUpdateService, dgvCarServices, cmbCarForService, cmbServiceForCar, dtpServiceDate, txtMileage, btnAddCarService, btnUpdateCarService });
        }
    }

    // Rounded Button Class
    public class RoundedButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            using (var g = new System.Drawing.Drawing2D.GraphicsPath())
            {
                g.AddArc(0, 0, 20, 20, 180, 90);
                g.AddArc(this.Width - 20, 0, 20, 20, 270, 90);
                g.AddArc(this.Width - 20, this.Height - 20, 20, 20, 0, 90);
                g.AddArc(0, this.Height - 20, 20, 20, 90, 90);
                g.CloseFigure();
                this.Region = new Region(g);
            }
            base.OnPaint(pevent);
        }
    }
}
