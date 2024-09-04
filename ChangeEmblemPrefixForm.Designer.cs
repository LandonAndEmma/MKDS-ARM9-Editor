using System.Windows.Forms;
namespace ARM9Editor
{
    partial class ChangeEmblemPrefixForm : Form
    {
        private System.ComponentModel.IContainer components = null;
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
            this.emblemNameTextBox = new System.Windows.Forms.TextBox();
            this.changeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // emblemNameTextBox
            // 
            this.emblemNameTextBox.Location = new System.Drawing.Point(12, 12);
            this.emblemNameTextBox.MaxLength = 2;
            this.emblemNameTextBox.Name = "emblemNameTextBox";
            this.emblemNameTextBox.Size = new System.Drawing.Size(100, 22);
            this.emblemNameTextBox.TabIndex = 0;
            // 
            // changeButton
            // 
            this.changeButton.Location = new System.Drawing.Point(118, 10);
            this.changeButton.Name = "changeButton";
            this.changeButton.Size = new System.Drawing.Size(75, 23);
            this.changeButton.TabIndex = 1;
            this.changeButton.Text = "Change";
            this.changeButton.UseVisualStyleBackColor = true;
            this.changeButton.Click += new System.EventHandler(this.changeButton_Click);
            // 
            // ChangeEmblemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 45);
            this.Controls.Add(this.changeButton);
            this.Controls.Add(this.emblemNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ChangeEmblemForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Emblem Name";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.TextBox emblemNameTextBox;
        private System.Windows.Forms.Button changeButton;
    }
}