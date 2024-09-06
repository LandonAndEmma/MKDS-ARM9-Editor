using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace ARM9Editor
{
    public partial class ChangeFileNameForm : Form
    {
        private int nameOffset;
        private int sizeOffset;
        private byte[] armValues;
        public ChangeFileNameForm(byte[] armValues, int nameOffset, int sizeOffset)
        {
            InitializeComponent();
            this.armValues = armValues;
            this.nameOffset = nameOffset;
            this.sizeOffset = sizeOffset;
            currentFileNameTextBox.Text = GetCurrentFileName();
        }
        private string GetCurrentFileName()
        {
            byte[] nameBytes = new byte[sizeOffset - nameOffset];
            Array.Copy(armValues, nameOffset, nameBytes, 0, nameBytes.Length);
            return System.Text.Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            string newName = newFileNameTextBox.Text;

            if (string.IsNullOrEmpty(newName))
            {
                if (MessageBox.Show("You have made the new name blank. Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }
            else if (!Regex.IsMatch(newName, @"^[A-Za-z0-9_]+$"))
            {
                MessageBox.Show("Invalid characters detected. Please use only English letters, numbers, and underscores.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (newName.Length > (sizeOffset - nameOffset))
            {
                MessageBox.Show($"New file name exceeds block size ({sizeOffset - nameOffset} bytes).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] newNameBytes = System.Text.Encoding.UTF8.GetBytes(newName);
            Array.Copy(newNameBytes, 0, armValues, nameOffset, newNameBytes.Length);
            for (int i = newNameBytes.Length; i < sizeOffset - nameOffset; i++)
            {
                armValues[nameOffset + i] = 0x00;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}