using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Autod_ja_Omanikud.Forms
{
    public enum FieldType
    {
        Text,
        Number,
        Combo,
        Date
    }

    public class FieldSpec
    {
        // Unique field key (used in returned Values dictionary)
        public string Name { get; set; } = string.Empty;
        // Label shown to user
        public string Label { get; set; } = string.Empty;
        // Initial value (string for text/number, key for Combo, ISO date for Date)
        public string? Value { get; set; }
        // For Combo: list of pairs (key, display)
        public List<KeyValuePair<string, string>>? Options { get; set; }
        public FieldType Type { get; set; } = FieldType.Text;
    }

    public class ModalForm : Form
    {
        private readonly List<FieldSpec> _fields;
        private readonly Dictionary<string, Control> _controls = new();

        public Dictionary<string, string> Values { get; private set; } = new();

        public ModalForm(string title, IEnumerable<FieldSpec> fields)
        {
            _fields = fields.ToList();
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BuildLayout();
        }

        private void BuildLayout()
        {
            var main = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = _fields.Count + 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                AutoSize = true
            };
            main.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int row = 0;
            foreach (var f in _fields)
            {
                var lbl = new Label
                {
                    Text = f.Label,
                    Anchor = AnchorStyles.Left,
                    AutoSize = true,
                    Margin = new Padding(3, 8, 3, 3)
                };
                main.Controls.Add(lbl, 0, row);

                Control input;
                switch (f.Type)
                {
                    case FieldType.Combo:
                        var cb = new ComboBox
                        {
                            DropDownStyle = ComboBoxStyle.DropDownList,
                            Anchor = AnchorStyles.Left | AnchorStyles.Right,
                            Width = 250
                        };
                        if (f.Options != null)
                        {
                            foreach (var kv in f.Options)
                                cb.Items.Add(new KeyValuePair<string, string>(kv.Key, kv.Value));
                            cb.DisplayMember = "Value";
                            cb.ValueMember = "Key";
                        }
                        // select initial
                        if (!string.IsNullOrEmpty(f.Value) && cb.Items.Count > 0)
                        {
                            for (int i = 0; i < cb.Items.Count; i++)
                            {
                                var kv = (KeyValuePair<string, string>)cb.Items[i];
                                if (kv.Key == f.Value)
                                {
                                    cb.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        input = cb;
                        break;

                    case FieldType.Date:
                        var dt = new DateTimePicker
                        {
                            Anchor = AnchorStyles.Left,
                            Format = DateTimePickerFormat.Short,
                        };
                        if (!string.IsNullOrEmpty(f.Value) && DateTime.TryParse(f.Value, out var d))
                            dt.Value = d;
                        input = dt;
                        break;

                    case FieldType.Number:
                    case FieldType.Text:
                    default:
                        var tb = new TextBox
                        {
                            Anchor = AnchorStyles.Left | AnchorStyles.Right,
                            Width = 250,
                            Text = f.Value ?? string.Empty
                        };
                        input = tb;
                        break;
                }

                input.Name = "fld_" + f.Name;
                main.Controls.Add(input, 1, row);
                _controls[f.Name] = input;
                row++;
            }

            // Buttons
            var btnPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, AutoSize = true };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, AutoSize = true };

            ok.Click += Ok_Click;

            btnPanel.Controls.Add(ok);
            btnPanel.Controls.Add(cancel);

            main.Controls.Add(btnPanel, 0, row);
            main.SetColumnSpan(btnPanel, 2);

            Controls.Add(main);
            AcceptButton = ok;
            CancelButton = cancel;
        }

        private void Ok_Click(object? sender, EventArgs e)
        {
            Values = new Dictionary<string, string>();
            foreach (var f in _fields)
            {
                if (!_controls.TryGetValue(f.Name, out var c)) continue;

                string val = string.Empty;
                switch (f.Type)
                {
                    case FieldType.Combo:
                        var cb = (ComboBox)c;
                        if (cb.SelectedItem != null)
                            val = ((KeyValuePair<string, string>)cb.SelectedItem).Key;
                        break;
                    case FieldType.Date:
                        var dt = (DateTimePicker)c;
                        val = dt.Value.ToString("o"); // ISO
                        break;
                    default:
                        val = c.Text ?? string.Empty;
                        break;
                }
                Values[f.Name] = val;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}