using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace ARM9Editor
{
    partial class ChangeFileNameForm
    {
        private IContainer components = null;
        private TextBox currentFileNameTextBox;
        private TextBox newFileNameTextBox;
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ChangeFileNameForm));
            currentFileNameTextBox = new TextBox();
            newFileNameTextBox = new TextBox();
            okButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // currentFileNameTextBox
            // 
            currentFileNameTextBox.Location = new Point(12, 12);
            currentFileNameTextBox.Name = "currentFileNameTextBox";
            currentFileNameTextBox.ReadOnly = true;
            currentFileNameTextBox.Size = new Size(156, 27);
            currentFileNameTextBox.TabIndex = 0;
            // 
            // newFileNameTextBox
            // 
            newFileNameTextBox.Location = new Point(12, 45);
            newFileNameTextBox.Name = "newFileNameTextBox";
            newFileNameTextBox.Size = new Size(156, 27);
            newFileNameTextBox.TabIndex = 1;
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
            // ChangeFileNameForm
            // 
            AcceptButton = okButton;
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = cancelButton;
            ClientSize = new Size(196, 156);
            ControlBox = false;
            Controls.Add(newFileNameTextBox);
            Controls.Add(currentFileNameTextBox);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Icon = (Icon)resources.GetObject("$Icon");
            MaximizeBox = false;
            MaximumSize = new Size(196, 156);
            MinimizeBox = false;
            MinimumSize = new Size(196, 156);
            Name = "ChangeFileNameForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Change File Name";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}