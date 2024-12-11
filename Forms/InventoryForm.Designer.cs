namespace ITAssetInventory.Forms
{
    partial class InventoryForm
    {
        // UI controls that are used in the form
        // pretty self explanatory, using windows forms
        // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridview?view=windowsdesktop-9.0
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtSelectedAsset;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnAddAsset;
        private System.Windows.Forms.Button btnRemoveAsset;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtSearch;

        // these are different, this is the status at the bottom left of the app, where it says if everything saved, or if it's ready and so on
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;

        // adding a help menu for instructions
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem instructionsMenuItem;

        private void InitializeComponent()
        {
            // Set up the DataGridView for displaying assets.
            // this takes what I wrote a couple lines above and instantiates these controls, sets their properties and adds them to the form's control group
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtSelectedAsset = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnAddAsset = new System.Windows.Forms.Button();
            this.btnRemoveAsset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            // menu helpMenu instructionsMenu stuff
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instructionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            // SelectionChanged event on the DataGridView to update selected asset (the textbox I spoke about earlier)
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // these are pretty much self-explanatory, will just go over what DataGridView1 and adjacent.xxx do:
            // location = specifies the position relative to top-left corner of the form
            // size = specifies the width and height
            // AutoSizeColumnsMode = determines how the columns resize to fit the available space in the COLUMNS (basically when you resize the app, this resizes too)
            // Anchor = same as the AutoSizeColumnsMode, but for the whole form
            // SelectionMode = selects said row

            // dataGridView1
            this.dataGridView1.Location = new System.Drawing.Point(12, 27); // Adjusted to account for MenuStrip height
            this.dataGridView1.Size = new System.Drawing.Size(760, 280);
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(12, 320);
            this.txtSearch.Size = new System.Drawing.Size(760, 20);
            this.txtSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            // this one is self-explanatory...
            this.txtSearch.PlaceholderText = "Search assets...";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // txtSelectedAsset
            this.txtSelectedAsset.Location = new System.Drawing.Point(12, 350);
            this.txtSelectedAsset.Size = new System.Drawing.Size(300, 23);
            this.txtSelectedAsset.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            // I don't want people editing the actual text
            this.txtSelectedAsset.ReadOnly = true;

            // txtQuantity
            this.txtQuantity.Location = new System.Drawing.Point(320, 350);
            this.txtQuantity.Size = new System.Drawing.Size(100, 23);
            this.txtQuantity.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // btnUpdate
            this.btnUpdate.Location = new System.Drawing.Point(430, 350);
            this.btnUpdate.Size = new System.Drawing.Size(100, 23);
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            // this makes the update and all the other buttons actually work
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            // btnExport
            this.btnExport.Location = new System.Drawing.Point(12, 380);
            this.btnExport.Size = new System.Drawing.Size(100, 23);
            this.btnExport.Text = "Export";
            this.btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);

            // btnAddAsset
            this.btnAddAsset.Location = new System.Drawing.Point(120, 380);
            this.btnAddAsset.Size = new System.Drawing.Size(100, 23);
            this.btnAddAsset.Text = "Add Asset";
            this.btnAddAsset.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnAddAsset.Click += new System.EventHandler(this.btnAddAsset_Click);

            // btnRemoveAsset
            this.btnRemoveAsset.Location = new System.Drawing.Point(230, 380);
            this.btnRemoveAsset.Size = new System.Drawing.Size(120, 23);
            this.btnRemoveAsset.Text = "Remove Asset";
            this.btnRemoveAsset.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnRemoveAsset.Click += new System.EventHandler(this.btnRemoveAsset_Click);

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(360, 380);
            this.btnSave.Size = new System.Drawing.Size(100, 23);
            this.btnSave.Text = "Save";
            this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // statusStrip
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.statusLabel });
            this.statusStrip.Location = new System.Drawing.Point(0, 410);
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            // gives a bit of space for the text to appear in the text box - I am not explaining this properly am I, you know what I mean
            this.statusStrip.TabIndex = 10;
            this.statusStrip.Text = "statusStrip";

            // statusLabel
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";

            // Configure helpMenuItem
            this.helpMenuItem.Text = "Help";

            // Configure instructionsMenuItem
            this.instructionsMenuItem.Text = "Instructions";
            this.instructionsMenuItem.Click += new System.EventHandler(this.instructionsMenuItem_Click);

            // Add instructionsMenuItem to helpMenuItem
            this.helpMenuItem.DropDownItems.Add(this.instructionsMenuItem);

            // Add helpMenuItem to the menuStrip
            this.menuStrip.Items.Add(this.helpMenuItem);

            // Position the menuStrip at the top
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 11;
            this.menuStrip.Text = "menuStrip";
            this.menuStrip.Dock = DockStyle.Top; // Ensure it docks properly

            // Add menuStrip to the Form
            this.Controls.Add(this.menuStrip);

            // InventoryForm
            this.ClientSize = new System.Drawing.Size(784, 440);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.txtSelectedAsset);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnAddAsset);
            this.Controls.Add(this.btnRemoveAsset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.statusStrip);
            this.Text = "PMDS IT Asset Inventory";
            this.StartPosition = FormStartPosition.CenterScreen;

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
