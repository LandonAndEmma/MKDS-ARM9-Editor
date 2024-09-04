using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
namespace ARM9Editor
{
    partial class ChangeEmblemPrefixForm : Form
    {
        private IContainer components = null;
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
            emblemNameTextBox = new TextBox();
            changeButton = new Button();
            SuspendLayout();
            // 
            // emblemNameTextBox
            // 
            emblemNameTextBox.Location = new Point(12, 12);
            emblemNameTextBox.MaxLength = 2;
            emblemNameTextBox.Name = "emblemNameTextBox";
            emblemNameTextBox.Size = new Size(100, 22);
            emblemNameTextBox.TabIndex = 0;
            // 
            // changeButton
            // 
            changeButton.Location = new Point(118, 10);
            changeButton.Name = "changeButton";
            changeButton.Size = new Size(75, 23);
            changeButton.TabIndex = 1;
            changeButton.Text = "Change";
            changeButton.UseVisualStyleBackColor = true;
            changeButton.Click += new (this.changeButton_Click);
            // 
            // ChangeEmblemForm
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(205, 45);
            Controls.Add(changeButton);
            Controls.Add(emblemNameTextBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "ChangeEmblemForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Change Emblem Name";
            ResumeLayout(false);
            PerformLayout();
        }
        private TextBox emblemNameTextBox;
        private Button changeButton;
    }
}