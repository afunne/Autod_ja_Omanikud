using Autod_ja_Omanikud.Forms;
using Autod_ja_Omanikud.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Autod_ja_Omanikud
{
    // Partial extension of Form1 - place vehicle handlers here if you prefer separation.
    public partial class Form1
    {
        // ===== CARS =====
        private void BtnAddCar_Click(object sender, EventArgs e)
        {
            // prepare owner options
            var owners = _context.Owners.ToList();
            var opts = new List<KeyValuePair<string, string>>();
            foreach (var o in owners) opts.Add(new KeyValuePair<string, string>(o.Id.ToString(), o.FullName));

            var fields = new List<FieldSpec>
            {
                new FieldSpec { Name = "Brand", Label = "Brand", Type = FieldType.Text, Value = "" },
                new FieldSpec { Name = "Model", Label = "Model", Type = FieldType.Text, Value = "" },
                new FieldSpec { Name = "RegistrationNumber", Label = "Registration nr", Type = FieldType.Text, Value = "" },
                new FieldSpec { Name = "OwnerId", Label = "Owner", Type = FieldType.Combo, Options = opts }
            };

            using var dlg = new ModalForm("Add Car", fields);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            if (!dlg.Values.TryGetValue("Brand", out var brand) || string.IsNullOrWhiteSpace(brand))
            {
                MessageBox.Show("Brand required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dlg.Values.TryGetValue("Model", out var model);
            dlg.Values.TryGetValue("RegistrationNumber", out var reg);
            dlg.Values.TryGetValue("OwnerId", out var ownerIdStr);

            if (!int.TryParse(ownerIdStr, out var ownerId))
            {
                MessageBox.Show("Please select owner.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var car = new Car
            {
                Brand = brand,
                Model = model ?? string.Empty,
                RegistrationNumber = reg ?? string.Empty,
                OwnerId = ownerId
            };

            _context.Cars.Add(car);
            _context.SaveChanges();
            LoadCars();
        }

        private void BtnUpdateCar_Click(object sender, EventArgs e)
        {
            if (dgvCars.CurrentRow == null) return;
            var id = (int)dgvCars.CurrentRow.Cells["Id"].Value;
            var car = _context.Cars.Find(id);
            if (car == null) return;

            var owners = _context.Owners.ToList();
            var opts = new List<KeyValuePair<string, string>>();
            foreach (var o in owners) opts.Add(new KeyValuePair<string, string>(o.Id.ToString(), o.FullName));

            var fields = new List<FieldSpec>
            {
                new FieldSpec { Name = "Brand", Label = "Brand", Type = FieldType.Text, Value = car.Brand },
                new FieldSpec { Name = "Model", Label = "Model", Type = FieldType.Text, Value = car.Model },
                new FieldSpec { Name = "RegistrationNumber", Label = "Registration nr", Type = FieldType.Text, Value = car.RegistrationNumber },
                new FieldSpec { Name = "OwnerId", Label = "Owner", Type = FieldType.Combo, Options = opts, Value = car.OwnerId.ToString() }
            };

            using var dlg = new ModalForm("Update Car", fields);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            if (dlg.Values.TryGetValue("Brand", out var brand) && !string.IsNullOrWhiteSpace(brand)) car.Brand = brand;
            if (dlg.Values.TryGetValue("Model", out var model)) car.Model = model;
            if (dlg.Values.TryGetValue("RegistrationNumber", out var reg)) car.RegistrationNumber = reg;
            if (dlg.Values.TryGetValue("OwnerId", out var ownerIdStr) && int.TryParse(ownerIdStr, out var ownerId)) car.OwnerId = ownerId;

            _context.SaveChanges();
            LoadCars();
        }

        private void BtnDeleteCar_Click(object sender, EventArgs e)
        {
            if (dgvCars.CurrentRow == null) return;
            var id = (int)dgvCars.CurrentRow.Cells["Id"].Value;
            var car = _context.Cars.Find(id);
            if (car == null) return;

            var res = MessageBox.Show($"Delete car '{car.Brand} {car.Model}'?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes) return;

            _context.Cars.Remove(car);
            _context.SaveChanges();
            LoadCars();
        }
    }
}