namespace ITAssetInventory.Forms
{
    partial class InstructionsForm
    {
        private System.Windows.Forms.TextBox txtInstructions;

        private void InitializeComponent()
        {
            this.txtInstructions = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtInstructions
            // 
            this.txtInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInstructions.Multiline = true;
            this.txtInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInstructions.ReadOnly = true;
            this.txtInstructions.Font = new System.Drawing.Font("Segoe UI", 10F);
            // 
            // InstructionsForm
            // 
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.txtInstructions);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instructions";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
