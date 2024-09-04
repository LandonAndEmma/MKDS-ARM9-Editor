using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace ARM9Editor
{
    partial class ChangeSeqValueForm
    {
        private IContainer components = null;
        private NumericUpDown inputNumericUpDown;
        private Button okButton;
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ChangeSeqValueForm));
            okButton = new Button();
            cancelButton = new Button();
            inputNumericUpDown = new NumericUpDown();
            ((ISupportInitialize)inputNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // inputNumericUpDown
            // 
            inputNumericUpDown.Location = new Point(12, 12);
            this.inputNumericUpDown.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.inputNumericUpDown.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            inputNumericUpDown.Name = "NumericUpDown";
            inputNumericUpDown.Size = new Size(156, 27);
            inputNumericUpDown.TabIndex = 0;
            this.inputNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // okButton
            // 
            okButton.Location = new Point(12, 38);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 1;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new(this.okButton_Click);
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(93, 38);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // ChangeSeqValueForm
            // 
            AcceptButton = okButton;
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = cancelButton;
            ClientSize = new Size(178, 65);
            ControlBox = false;
            Controls.Add(inputNumericUpDown);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(196, 112);
            MinimizeBox = false;
            MinimumSize = new Size(196, 112);
            Name = "ChangeSeqValueForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Change SEQ Value";
            ((ISupportInitialize)inputNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}