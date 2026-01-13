using Autod_ja_Omanikud.Forms;
using Autod_ja_Omanikud.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Autod_ja_Omanikud
{
    // This is a partial extension of Form1. Do NOT duplicate these methods elsewhere.
    public partial class Form1
    {
        // ===== OWNERS =====
        private void BtnAddOwner_Click(object sender, EventArgs e)
        {
            var fields = new List<FieldSpec>
            {
                new FieldSpec { Name = "FullName", Label = "Full name", Type = FieldType.Text, Value = "" },
                new FieldSpec { Name = "Phone", Label = "Phone", Type = FieldType.Text, Value = "" }
            };

            using var dlg = new ModalForm("Add Owner", fields);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            if (!dlg.Values.TryGetValue("FullName", out var fullName) || string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Full name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dlg.Values.TryGetValue("Phone", out var phone);

            var owner = new Owner { FullName = fullName, Phone = phone ?? string.Empty };
            _context.Owners.Add(owner);
            _context.SaveChanges();
            LoadOwners();
        }

        private void BtnUpdateOwner_Click(object sender, EventArgs e)
        {
            if (dgvOwners.CurrentRow == null) return;
            var id = (int)dgvOwners.CurrentRow.Cells["Id"].Value;
            var owner = _context.Owners.Find(id);
            if (owner == null) return;

            var fields = new List<FieldSpec>
            {
                new FieldSpec { Name = "FullName", Label = "Full name", Type = FieldType.Text, Value = owner.FullName },
                new FieldSpec { Name = "Phone", Label = "Phone", Type = FieldType.Text, Value = owner.Phone }
            };

            using var dlg = new ModalForm("Update Owner", fields);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            if (dlg.Values.TryGetValue("FullName", out var fullName) && !string.IsNullOrWhiteSpace(fullName))
                owner.FullName = fullName;
            if (dlg.Values.TryGetValue("Phone", out var phone))
                owner.Phone = phone;

            _context.SaveChanges();
            LoadOwners();
        }

        private void BtnDeleteOwner_Click(object sender, EventArgs e)
        {
            if (dgvOwners.CurrentRow == null) return;
            var id = (int)dgvOwners.CurrentRow.Cells["Id"].Value;
            var owner = _context.Owners.Find(id);
            if (owner == null) return;

            var res = MessageBox.Show($"Delete owner '{owner.FullName}'?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes) return;

            _context.Owners.Remove(owner);
            _context.SaveChanges();
            LoadOwners();
        }
    }
}