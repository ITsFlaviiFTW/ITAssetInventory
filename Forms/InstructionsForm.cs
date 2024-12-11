using System;
using System.Windows.Forms;

namespace ITAssetInventory.Forms
{
    public partial class InstructionsForm : Form
    {
        public InstructionsForm()
        {
            InitializeComponent();
            LoadInstructionsText();
        }

        private void LoadInstructionsText()
        {
            // This could be the same text as the user manual described above.
            // For instance:
            txtInstructions.Text = 
@"How to Use the PMDS IT Asset Inventory:

1. Launch the app; the main window shows your assets.
2. Use the search box to filter assets by name.
3. Select an asset from the list to view or update its quantity.
4. Enter a new quantity and click 'Update' to save changes.
5. Click 'Add Asset' to create a new item; 'Remove Asset' to delete one.
6. Use 'Export' to save the inventory list as a CSV or TXT file.
7. 'Save' ensures your changes are written to the data file.
8. Low stock items (quantity ≤ 3) will flash red; sufficient stock shows green.
9. Hover over status bar messages or see them change after updates and exports.
10. Closing the app saves your changes automatically.

For more detailed instructions or troubleshooting, refer to the provided user manual.";

            // Make sure txtInstructions is multiline and set to fill the form area in designer.
        }
    }
}
