using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace ARM9Editor
{
    partial class ChangeCharacterFileNameForm
    {
        private IContainer components = null;
        private TextBox currentCharacterPrefixTextBox;
        private TextBox newCharacterPrefixTextBox;
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ChangeCharacterFileNameForm));
            currentCharacterPrefixTextBox = new TextBox();
            newCharacterPrefixTextBox = new TextBox();
            okButton = new Button();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // currentCharacterPrefixTextBox
            // 
            currentCharacterPrefixTextBox.Location = new Point(12, 12);
            currentCharacterPrefixTextBox.Name = "currentCharacterPrefixTextBox";
            currentCharacterPrefixTextBox.ReadOnly = true;
            currentCharacterPrefixTextBox.Size = new Size(156, 27);
            currentCharacterPrefixTextBox.TabIndex = 0;
            // 
            // newCharacterPrefixTextBox
            // 
            newCharacterPrefixTextBox.Location = new Point(12, 45);
            newCharacterPrefixTextBox.Name = "newFileNameTextBox";
            newCharacterPrefixTextBox.Size = new Size(156, 27);
            newCharacterPrefixTextBox.TabIndex = 1;
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
            // ChangeCharacterFileNameForm
            // 
            AcceptButton = okButton;
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = cancelButton;
            ClientSize = new Size(196, 156);
            ControlBox = false;
            Controls.Add(newCharacterPrefixTextBox);
            Controls.Add(currentCharacterPrefixTextBox);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Icon = (Icon)resources.GetObject("$Icon");
            MaximizeBox = false;
            MaximumSize = new Size(196, 156);
            MinimizeBox = false;
            MinimumSize = new Size(196, 156);
            Name = "ChangeCharacterFileNameForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Change Character Filename";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}