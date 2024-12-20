using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ITAssetInventory.Models;
using ITAssetInventory.Data;

namespace ITAssetInventory.Forms
{
    public partial class InventoryForm : Form
    {
        // Holds the in-memory list of assets currently tracked.
        private List<Asset> _assets = new List<Asset>();
        
        // A timer used to toggle the flashing red icon for low-stock items.
        private System.Windows.Forms.Timer _flashTimer;
        private bool _flashState = false; // Used to toggle red indicators
        
        // Images representing the status icons:
        // Green icon for sufficient stock, red icon (flashing) for low stock.
        private Image _greenIcon;
        private Image _redIcon;

        // Path to the data file storing the inventory. 
        // JSON file is used here for persistent storage of asset data.
        private string dataFilePath = "inventory_data.json";

        // The repository handles loading and saving assets to the data file.
        private AssetRepository _repository;

        private int _dragIndex = -1;
        private int _dropIndex = -1;


        public InventoryForm()
        {
            InitializeComponent(); // Initialize UI components from the designer.
            _repository = new AssetRepository(dataFilePath);
            LoadData(); // Load assets from file if exists, else initialize defaults
            LoadIcons();
            SetupDataGridView();
            SetupTimer();
        }

        private void LoadData()
        {
            // Load assets from the JSON file via the repository.
            _assets = _repository.LoadAssets();
            if (_assets.Count == 0)
            {
                // If no data file or empty list, initialize with defaults
                InitializeAssets();
                SaveData(); // Save defaults
            }
        }

        private void SaveData()
        {
            // Save current _assets list to the file.
            _repository.SaveAssets(_assets);
            ShowStatusMessage("Changes saved successfully!");
        }

        private void LoadIcons()
        {
            // Create a small green circle icon for indicating sufficient stock.
            Bitmap greenBmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(greenBmp))
            {
                g.FillEllipse(Brushes.Green, 0, 0, 16, 16);
            }
            _greenIcon = greenBmp;

            // Create a small red circle icon for when the stock is 3 or below
            Bitmap redBmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(redBmp))
            {
                g.FillEllipse(Brushes.Red, 0, 0, 16, 16);
            }
            _redIcon = redBmp;
        }

        private void InitializeAssets()
        {
            // If the current list is null or empty, add some default assets so the list doesn't look empty
            // User will start with these assets when first running the program
            if (_assets == null || !_assets.Any())
            {
                _assets.Add(new Asset("Monitors", 10));
                _assets.Add(new Asset("Computers (Tiny Clients)", 10));
                _assets.Add(new Asset("Laptops - 16GB flashdrive", 10));
                
            }
        }

        // No auto generated columns as I've noticed it causes problems, manually added. (well, technically automatically, but they get added when adding the asset LOL)
        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            // TO DO: maybe make the width auto generated based on the character name with a buffer on each side? hmm

            var snCol = new DataGridViewTextBoxColumn
            {
                HeaderText = "S/N",
                Width = 50,
                ReadOnly = false
            };
            dataGridView1.Columns.Add(snCol);

            // asset name, pretty self explanatory
            var nameCol = new DataGridViewTextBoxColumn
            {
                HeaderText = "Asset Name",
                DataPropertyName = "Name",
                ReadOnly = false,
                Width = 200
            };

            // quantity...
            var qtyCol = new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity",
                ReadOnly = false,
                Width = 80
            };

            // status - red or green depending on stock
            var statusCol = new DataGridViewImageColumn
            {
                HeaderText = "Status",
                Width = 50,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };

            dataGridView1.Columns.Add(nameCol);
            dataGridView1.Columns.Add(qtyCol);
            dataGridView1.Columns.Add(statusCol);

            //using ToList() to make sure the DataGridView doesn't operate on the original list (the 3 mentioned)
            dataGridView1.DataSource = _assets.ToList();

            dataGridView1.DataBindingComplete += (s, e) => AssignSerialNumbers();
            dataGridView1.CellValidating += DataGridView1_CellValidating;
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            
            dataGridView1.AllowDrop = true;
            dataGridView1.MouseDown += DataGridView1_MouseDown;
            dataGridView1.MouseMove += DataGridView1_MouseMove;
            dataGridView1.DragOver += DataGridView1_DragOver;
            dataGridView1.DragDrop += DataGridView1_DragDrop;

            UpdateIndicators();
        }


            private void DataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
            {
                // Assuming columns: S/N=0, Name=1, Quantity=2, Status=3
                if (e.ColumnIndex == 2) // Quantity column index
                {
                    if (!int.TryParse(e.FormattedValue.ToString(), out int _))
                    {
                        MessageBox.Show("Invalid quantity. Please enter a valid number.");
                        e.Cancel = true;
                    }
                }
            }

            private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
            {
                // Ensure we don't run this on header rows or invalid rows
                if (e.RowIndex >= 0 && e.ColumnIndex > 0)
                {
                    var editedRow = dataGridView1.Rows[e.RowIndex];
                    var asset = editedRow.DataBoundItem as ITAssetInventory.Models.Asset;
                    if (asset != null)
                    {
                        // Update the corresponding asset in the _assets list
                        string updatedName = editedRow.Cells[1].Value.ToString();
                        int updatedQty = int.Parse(editedRow.Cells[2].Value.ToString());

                        // Find the original asset by old name (assuming unique names)
                        var originalAsset = _assets.FirstOrDefault(a => a.Name == asset.Name);
                        if (originalAsset != null)
                        {
                            originalAsset.Name = updatedName;
                            originalAsset.Quantity = updatedQty;
                            UpdateIndicators();
                            SaveData();
                        }
                    }
                }
            }


            private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
            {
                var hitTest = dataGridView1.HitTest(e.X, e.Y);
                if (hitTest.RowIndex >= 0)
                {
                    _dragIndex = hitTest.RowIndex;
                }
            }

            private void DataGridView1_MouseMove(object sender, MouseEventArgs e)
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left && _dragIndex >= 0)
                {
                    dataGridView1.DoDragDrop(dataGridView1.Rows[_dragIndex], DragDropEffects.Move);
                }
            }

            private void DataGridView1_DragOver(object sender, DragEventArgs e)
            {
                e.Effect = DragDropEffects.Move;
                Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));
                var hitTest = dataGridView1.HitTest(clientPoint.X, clientPoint.Y);
                if (hitTest.RowIndex >= 0)
                {
                    _dropIndex = hitTest.RowIndex;
                }
            }

            private void DataGridView1_DragDrop(object sender, DragEventArgs e)
            {
                if (_dragIndex >= 0 && _dropIndex >= 0 && _dropIndex != _dragIndex)
                {
                    var draggedAsset = _assets[_dragIndex];
                    _assets.RemoveAt(_dragIndex);
                    _assets.Insert(_dropIndex, draggedAsset);

                    dataGridView1.DataSource = _assets.ToList();
                    AssignSerialNumbers();
                    SaveData();
                }

                _dragIndex = -1;
                _dropIndex = -1;
            }



        private void AssignSerialNumbers()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
            }
        }


        private void SetupTimer()
        {
            //timer for status color - only for red when low stock
            _flashTimer = new System.Windows.Forms.Timer();
            _flashTimer.Interval = 500; // milliseconds, so half a second
            _flashTimer.Tick += (s, e) =>
            {
                _flashState = !_flashState;
                UpdateIndicators();
            };
            _flashTimer.Start();
        }

        // function to make sure that when the stock is 3 or less, the red circle pops up and starts flashing
        private void UpdateIndicators()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.DataBoundItem is Asset asset)
                {
                    var cell = row.Cells[3] as DataGridViewImageCell; // Status column is now index 3
                    if (asset.Quantity <= 3)
                    {
                        cell.Value = _flashState ? _redIcon : null;
                    }
                    else
                    {
                        cell.Value = _greenIcon;
                    }
                }
            }
        }

        // status at the bottom left of the app, stays and changes every 3 seconds dpending on the action
        private void ShowStatusMessage(string message)
        {
            statusLabel.Text = message;
            var timer = new System.Windows.Forms.Timer { Interval = 3000 }; // Reset after 3 seconds
            timer.Tick += (s, e) =>
            {
                statusLabel.Text = "Ready";
                timer.Stop();
            };
            timer.Start();
        }

        // search feature for the asset finding
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //this whole thing says that when the user types in the asset name, it filters the assets, and if there is nothing to display, it shows all assets
            string query = txtSearch.Text.Trim().ToLower();
            var filteredAssets = string.IsNullOrEmpty(query)
                ? _assets
                : _assets.Where(a => a.Name.ToLower().Contains(query)).ToList();

            dataGridView1.DataSource = filteredAssets;
            UpdateIndicators();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && int.TryParse(txtQuantity.Text, out int newQty))
            {
                var asset = dataGridView1.SelectedRows[0].DataBoundItem as Asset;
                if (asset != null)
                {
                    asset.Quantity = newQty;
                    dataGridView1.Refresh();
                    UpdateIndicators();
                    SaveData(); // Save changes
                }
            }
            else
            {
                MessageBox.Show("Select an asset and enter a valid quantity.");
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files|*.csv|Text files|*.txt|All Files|*.*";
                sfd.Title = "Export Inventory";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        //parsing into the .txt or .csv
                        sw.WriteLine("Asset Name,Quantity");
                        foreach (var asset in _assets)
                        {
                            sw.WriteLine($"{asset.Name},{asset.Quantity}");
                        }
                    }
                    ShowStatusMessage("Export successful!");
                }
            }
        }

        //function to add an asset, has name and quantity
        private void btnAddAsset_Click(object sender, EventArgs e)
        {
            using (Form addForm = new Form())
            {
                addForm.Text = "Add New Asset";
                addForm.Size = new System.Drawing.Size(300, 200);

                Label lblName = new Label { Text = "Name:", Location = new Point(20, 20) };
                TextBox txtName = new TextBox { Location = new Point(120, 20), Width = 150, BorderStyle = BorderStyle.FixedSingle };

                Label lblQuantity = new Label { Text = "Quantity:", Location = new Point(20, 60) };
                TextBox txtQuantity = new TextBox { Location = new Point(120, 60), Width = 150 };

                Button btnAdd = new Button { Text = "Add", Location = new Point(100, 100) };
                btnAdd.Click += (snd, evt) =>
                {
                    if (int.TryParse(txtQuantity.Text, out int quantity) && !string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        _assets.Add(new Asset(txtName.Text.Trim(), quantity));
                        dataGridView1.DataSource = _assets.ToList();
                        UpdateIndicators();
                        SaveData(); // Save changes
                        addForm.Close();
                    }
                    else
                    {
                        // i am NOT testing for all the use cases that the user might try to break this app...ugh
                        MessageBox.Show("Invalid input! Make sure you are using numbers for quantity.");
                    }
                };

                addForm.Controls.Add(lblName);
                addForm.Controls.Add(txtName);
                addForm.Controls.Add(lblQuantity);
                addForm.Controls.Add(txtQuantity);
                addForm.Controls.Add(btnAdd);

                addForm.ShowDialog();
            }
        }

        //basically adding the asset, but opposite, cool feature to add a confirmation that you actually want to remove said asset
        private void btnRemoveAsset_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var asset = dataGridView1.SelectedRows[0].DataBoundItem as Asset;
                if (asset != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to remove {asset.Name}?", 
                                                "Confirm Remove", 
                                                MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        _assets.Remove(asset);
                        dataGridView1.DataSource = _assets.ToList();
                        UpdateIndicators();
                        SaveData(); // Save changes
                    }
                }
            }
            else
            {
                MessageBox.Show("No asset selected.");
            }
        }

        private void instructionsMenuItem_Click(object sender, EventArgs e)
        {
            // Show the instructions form as a dialog.
            using (var instructionsForm = new InstructionsForm())
            {
                instructionsForm.ShowDialog();
            }
        }


        // this function is pretty cool, when you select an asset from the grid, there is this small text box where the asset will appear, so you can see which one you selected
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var asset = dataGridView1.SelectedRows[0].DataBoundItem as Asset;
                if (asset != null)
                {
                    txtSelectedAsset.Text = asset.Name; // Display selected asset name
                }
                }
                else
                {
                txtSelectedAsset.Text = string.Empty; // Clear when no selection
                }
}


        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            SaveData(); // Ensure data is saved on close
        }
    }
}
