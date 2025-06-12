using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace ARM9Editor
{
    partial class ChangeCourseFileNameForm
    {
        private IContainer? components = null;
        private TextBox currentFileNameTextBox = null!;
        private TextBox newFileNameTextBox = null!;
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
            var resources = new ComponentResourceManager(typeof(ChangeCourseFileNameForm));
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
            okButton.Click += okButton_Click;
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
            // ChangeCourseFileNameForm
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
            Icon = (Icon)resources.GetObject("$Icon")!;
            MaximizeBox = false;
            MaximumSize = new Size(196, 156);
            MinimizeBox = false;
            MinimumSize = new Size(196, 156);
            Name = "ChangeCourseFileNameForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Change File Name";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}