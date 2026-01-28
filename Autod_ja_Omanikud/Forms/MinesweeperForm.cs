using System;
using System.Drawing;
using System.Windows.Forms;

namespace Autod_ja_Omanikud.Forms
{
    public class MinesweeperForm : Form
    {
        private int _rows;
        private int _cols;
        private int _mines;

        private Button[,] _buttons = null!;
        private bool[,] _hasMine = null!;
        private bool[,] _revealed = null!;
        private bool _gameOver;

        private TableLayoutPanel _grid;
        private ComboBox _cmbDifficulty = null!;
        private Button _btnRestart = null!;

        public bool BackToLoginRequested { get; private set; }

        private enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        public MinesweeperForm()
        {
            Text = "Minesweeper";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            _grid = new TableLayoutPanel { Dock = DockStyle.Fill };

            InitializeLayout();
            SetDifficulty(Difficulty.Easy);
            NewGame();
        }

        private void InitializeLayout()
        {
            var topPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(5),
                FlowDirection = FlowDirection.LeftToRight
            };

            var lbl = new Label
            {
                Text = "Difficulty:",
                AutoSize = true,
                Margin = new Padding(3, 10, 3, 3)
            };

            _cmbDifficulty = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 120
            };
            _cmbDifficulty.Items.AddRange(new object[] { "Easy", "Medium", "Hard" });
            _cmbDifficulty.SelectedIndexChanged += (_, __) =>
            {
                SetDifficulty((Difficulty)_cmbDifficulty.SelectedIndex);
                NewGame();
            };
            _cmbDifficulty.SelectedIndex = 0;

            _btnRestart = new Button
            {
                Text = "Restart",
                AutoSize = true,
                Margin = new Padding(10, 5, 3, 3)
            };
            _btnRestart.Click += (_, __) => NewGame();

            topPanel.Controls.Add(lbl);
            topPanel.Controls.Add(_cmbDifficulty);
            topPanel.Controls.Add(_btnRestart);

            Controls.Add(_grid);
            Controls.Add(topPanel);

            Width = 500;
            Height = 550;
        }

        private void SetDifficulty(Difficulty diff)
        {
            switch (diff)
            {
                case Difficulty.Easy:
                    _rows = 9;
                    _cols = 9;
                    _mines = 10;
                    break;
                case Difficulty.Medium:
                    _rows = 16;
                    _cols = 16;
                    _mines = 40;
                    break;
                case Difficulty.Hard:
                    _rows = 16;
                    _cols = 30;
                    _mines = 99;
                    break;
            }
        }

        private void NewGame()
        {
            _gameOver = false;
            BackToLoginRequested = false;

            ResizeForCurrentGrid();

            _grid.Controls.Clear();
            _grid.RowStyles.Clear();
            _grid.ColumnStyles.Clear();

            _grid.RowCount = _rows;
            _grid.ColumnCount = _cols;

            _buttons = new Button[_rows, _cols];
            _hasMine = new bool[_rows, _cols];
            _revealed = new bool[_rows, _cols];

            for (int r = 0; r < _rows; r++)
            {
                _grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / _rows));
            }
            for (int c = 0; c < _cols; c++)
            {
                _grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / _cols));
            }

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    var btn = new Button
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(1),
                        Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold),
                        Tag = (r, c)
                    };
                    btn.MouseUp += Cell_MouseUp;
                    _buttons[r, c] = btn;
                    _grid.Controls.Add(btn, c, r);
                }
            }

            PlaceMines();
        }

        private void ResizeForCurrentGrid()
        {
            // Rough sizing so that each cell is clearly visible for all difficulties.
            const int cellSize = 26; // pixels per cell
            const int topPanelHeight = 80; // difficulty + margins
            int width = Math.Max(350, _cols * cellSize + 40);
            int height = topPanelHeight + _rows * cellSize + 60;

            ClientSize = new Size(width, height);
        }

        private void PlaceMines()
        {
            var rnd = new Random();
            int placed = 0;
            while (placed < _mines)
            {
                int r = rnd.Next(_rows);
                int c = rnd.Next(_cols);
                if (_hasMine[r, c]) continue;
                _hasMine[r, c] = true;
                placed++;
            }
        }

        private void Cell_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_gameOver) return;
            if (sender is not Button btn || btn.Tag is not ValueTuple<int, int> pos) return;
            var (r, c) = pos;

            if (e.Button == MouseButtons.Right)
            {
                if (!_revealed[r, c])
                {
                    btn.Text = btn.Text == "F" ? string.Empty : "F";
                }
                return;
            }

            RevealCell(r, c);
        }

        private void RevealCell(int r, int c)
        {
            if (r < 0 || r >= _rows || c < 0 || c >= _cols) return;
            if (_revealed[r, c]) return;

            _revealed[r, c] = true;
            var btn = _buttons[r, c];
            btn.Enabled = false;

            if (_hasMine[r, c])
            {
                btn.BackColor = Color.Red;
                btn.Text = "*";
                GameOver(false);
                return;
            }

            int count = CountAdjacentMines(r, c);
            if (count > 0)
            {
                btn.Text = count.ToString();
            }
            else
            {
                // empty cell: flood fill
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        if (dr == 0 && dc == 0) continue;
                        RevealCell(r + dr, c + dc);
                    }
                }
            }

            if (CheckWin())
            {
                GameOver(true);
            }
        }

        private int CountAdjacentMines(int r, int c)
        {
            int count = 0;
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;
                    int nr = r + dr;
                    int nc = c + dc;
                    if (nr < 0 || nr >= _rows || nc < 0 || nc >= _cols) continue;
                    if (_hasMine[nr, nc]) count++;
                }
            }
            return count;
        }

        private bool CheckWin()
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if (!_hasMine[r, c] && !_revealed[r, c])
                        return false;
                }
            }
            return true;
        }

        private void GameOver(bool win)
        {
            if (_gameOver) return;
            _gameOver = true;

            // reveal all mines
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if (_hasMine[r, c])
                    {
                        var b = _buttons[r, c];
                        b.Text = "*";
                        b.BackColor = win ? Color.LightGreen : Color.Red;
                    }
                }
            }

            if (win)
            {
                var res = MessageBox.Show("You cleared the field! Go back to login?", "Minesweeper", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (res == DialogResult.Yes)
                {
                    BackToLoginRequested = true;
                    Close();
                }
                else
                {
                    NewGame();
                }
            }
            else
            {
                var res = MessageBox.Show("Boom! New game?", "Minesweeper", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    NewGame();
                }
            }
        }
    }
}
