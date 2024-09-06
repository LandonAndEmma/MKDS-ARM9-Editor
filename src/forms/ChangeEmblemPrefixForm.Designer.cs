using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace ARM9Editor
{
    partial class ChangeEmblemPrefixForm
    {
        private IContainer components = null;
        private TextBox currentEmblemPrefixTextBox;
        private TextBox newEmblemPrefixTextBox;
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ChangeEmblemPrefixForm));
            currentEmblemPrefixTextBox = new TextBox();
            newEmblemPrefixTextBox = new TextBox();
            okButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // currentEmblemPrefixTextBox
            // 
            currentEmblemPrefixTextBox.Location = new Point(12, 12);
            currentEmblemPrefixTextBox.Name = "currentEmblemPrefixTextBox";
            currentEmblemPrefixTextBox.ReadOnly = true;
            currentEmblemPrefixTextBox.Size = new Size(156, 27);
            currentEmblemPrefixTextBox.TabIndex = 0;
            // 
            // newEmblemPrefixTextBox
            // 
            newEmblemPrefixTextBox.Location = new Point(12, 45);
            newEmblemPrefixTextBox.Name = "newFileNameTextBox";
            newEmblemPrefixTextBox.Size = new Size(156, 27);
            newEmblemPrefixTextBox.TabIndex = 1;
            // 
            // okButton
            // 
            okButton.Location = new Point(12, 82);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 2;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += new(okButton_Click);
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(93, 82);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // ChangeEmblemPrefixForm
            // 
            AcceptButton = okButton;
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = cancelButton;
            ClientSize = new Size(196, 156);
            ControlBox = false;
            Controls.Add(newEmblemPrefixTextBox);
            Controls.Add(currentEmblemPrefixTextBox);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Icon = (Icon)resources.GetObject("$Icon");
            MaximizeBox = false;
            MaximumSize = new Size(196, 156);
            MinimizeBox = false;
            MinimumSize = new Size(196, 156);
            Name = "ChangeEmblemPrefixForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Change Emblem Prefix";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}