using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace ARM9Editor
{
    partial class ChangeKartFileNameForm
    {
        private IContainer components = null;
        private TextBox currentKartPrefixTextBox;
        private TextBox newKartPrefixTextBox;
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ChangeKartFileNameForm));
            currentKartPrefixTextBox = new TextBox();
            newKartPrefixTextBox = new TextBox();
            okButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // currentKartPrefixTextBox
            // 
            currentKartPrefixTextBox.Location = new Point(12, 12);
            currentKartPrefixTextBox.Name = "currentKartPrefixTextBox";
            currentKartPrefixTextBox.ReadOnly = true;
            currentKartPrefixTextBox.Size = new Size(156, 27);
            currentKartPrefixTextBox.TabIndex = 0;
            // 
            // newKartPrefixTextBox
            // 
            newKartPrefixTextBox.Location = new Point(12, 45);
            newKartPrefixTextBox.Name = "newFileNameTextBox";
            newKartPrefixTextBox.Size = new Size(156, 27);
            newKartPrefixTextBox.TabIndex = 1;
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
            // ChangeKartPrefixForm
            // 
            AcceptButton = okButton;
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = cancelButton;
            ClientSize = new Size(196, 156);
            ControlBox = false;
            Controls.Add(newKartPrefixTextBox);
            Controls.Add(currentKartPrefixTextBox);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Icon = (Icon)resources.GetObject("$Icon");
            MaximizeBox = false;
            MaximumSize = new Size(196, 156);
            MinimizeBox = false;
            MinimumSize = new Size(196, 156);
            Name = "ChangeKartFileNameForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Change Referenced Kart Filename";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}