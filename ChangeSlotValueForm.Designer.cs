namespace ARM9Editor
{
    partial class ChangeSlotValueForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.NumericUpDown slotValueNumericUpDown;
        private System.Windows.Forms.Button changeSlotValueButton;
        private System.Windows.Forms.Button cancelButton;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeSlotValueForm));
            this.slotValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.changeSlotValueButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.slotValueNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // slotValueNumericUpDown
            // 
            this.slotValueNumericUpDown.Location = new System.Drawing.Point(30, 30);
            this.slotValueNumericUpDown.Maximum = new decimal(new int[] {
            54,
            0,
            0,
            0});
            this.slotValueNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.slotValueNumericUpDown.Name = "slotValueNumericUpDown";
            this.slotValueNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.slotValueNumericUpDown.TabIndex = 0;
            this.slotValueNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // changeSlotValueButton
            // 
            this.changeSlotValueButton.Location = new System.Drawing.Point(30, 70);
            this.changeSlotValueButton.Name = "changeSlotValueButton";
            this.changeSlotValueButton.Size = new System.Drawing.Size(120, 30);
            this.changeSlotValueButton.TabIndex = 1;
            this.changeSlotValueButton.Text = "Change Value";
            this.changeSlotValueButton.UseVisualStyleBackColor = true;
            this.changeSlotValueButton.Click += new System.EventHandler(this.changeSlotValueButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(30, 110);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(120, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ChangeSlotValueForm
            // 
            this.ClientSize = new System.Drawing.Size(180, 160);
            this.ControlBox = false;
            this.Controls.Add(this.slotValueNumericUpDown);
            this.Controls.Add(this.changeSlotValueButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(196, 199);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(196, 199);
            this.Name = "ChangeSlotValueForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Slot Value";
            ((System.ComponentModel.ISupportInitialize)(this.slotValueNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
