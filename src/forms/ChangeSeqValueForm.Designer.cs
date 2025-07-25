﻿using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace ARM9Editor
{
    partial class ChangeSeqValueForm
    {
        private IContainer? components = null;
        private NumericUpDown inputNumericUpDown = null!;
        private Button okButton = null!;
        private Button cancelButton = null!;
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            var resources = new ComponentResourceManager(typeof(ChangeSeqValueForm));
            inputNumericUpDown = new NumericUpDown();
            okButton = new Button();
            cancelButton = new Button();
            ((ISupportInitialize)inputNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // inputNumericUpDown
            // 
            inputNumericUpDown.Location = new Point(12, 12);
            inputNumericUpDown.Maximum = 75;
            inputNumericUpDown.Minimum = 0;
            inputNumericUpDown.Name = "inputNumericUpDown";
            inputNumericUpDown.Size = new Size(156, 27);
            inputNumericUpDown.TabIndex = 0;
            inputNumericUpDown.Value = 1;
            // 
            // okButton
            // 
            okButton.Location = new Point(12, 38);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 1;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
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
            ClientSize = new Size(196, 112);
            ControlBox = false;
            Controls.Add(inputNumericUpDown);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            Icon = (Icon)resources.GetObject("$Icon")!;
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