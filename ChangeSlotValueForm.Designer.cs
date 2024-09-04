using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace ARM9Editor
{
    partial class ChangeSlotValueForm
    {
        private IContainer components = null;
        private NumericUpDown inputNumericUpDown;
        private Button changeSlotValueButton;
        private Button cancelButton;
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ChangeSlotValueForm));
            this.inputNumericUpDown = new NumericUpDown();
            this.changeSlotValueButton = new Button();
            this.cancelButton = new Button();
            ((ISupportInitialize)(this.inputNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // inputNumericUpDown
            // 
            this.inputNumericUpDown.Location = new Point(30, 30);
            this.inputNumericUpDown.Maximum = new decimal(new int[] {
            54,
            0,
            0,
            0});
            this.inputNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.inputNumericUpDown.Name = "inputNumericUpDown";
            this.inputNumericUpDown.Size = new Size(120, 20);
            this.inputNumericUpDown.TabIndex = 0;
            this.inputNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // changeSlotValueButton
            // 
            this.changeSlotValueButton.Location = new Point(30, 70);
            this.changeSlotValueButton.Name = "changeSlotValueButton";
            this.changeSlotValueButton.Size = new Size(120, 30);
            this.changeSlotValueButton.TabIndex = 1;
            this.changeSlotValueButton.Text = "Change Value";
            this.changeSlotValueButton.UseVisualStyleBackColor = true;
            this.changeSlotValueButton.Click += new (this.changeSlotValueButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new Point(30, 110);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new Size(120, 30);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new (this.cancelButton_Click);
            // 
            // ChangeSlotValueForm
            // 
            this.ClientSize = new Size(180, 160);
            this.ControlBox = false;
            this.Controls.Add(this.inputNumericUpDown);
            this.Controls.Add(this.changeSlotValueButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new Size(196, 199);
            this.MinimizeBox = false;
            this.MinimumSize = new Size(196, 199);
            this.Name = "ChangeSlotValueForm";
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Change Slot Value";
            ((ISupportInitialize)(this.inputNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }
    }
}