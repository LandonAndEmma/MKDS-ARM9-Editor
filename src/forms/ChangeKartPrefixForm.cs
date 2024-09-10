using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace ARM9Editor
{
	public partial class ChangeKartFileNameForm : Form
	{
		public string NewKartName { get; private set; }
        private readonly int kartOffset;
        private readonly byte[] armValues;
        private string GetCurrentKartPrefix()
        {
            byte[] kartBytes = new byte[4];
            Array.Copy(armValues, kartOffset, kartBytes, 0, kartBytes.Length);
            return Encoding.ASCII.GetString(kartBytes).TrimEnd('\0');
        }
        public ChangeKartFileNameForm(byte[] armValues, int kartOffset)
		{
			InitializeComponent();
			this.armValues = armValues;
			this.kartOffset = kartOffset;
            currentKartPrefixTextBox.Text = GetCurrentKartPrefix();
        }
        private void okButton_Click(object sender, EventArgs e)
		{
			string newName = newKartPrefixTextBox.Text;
			if (string.IsNullOrEmpty(newName))
			{
				if (MessageBox.Show("You have made the new filename blank. Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return;
				newName = new string('\0', 4);
			}
			else if (newName.Length != 4 || !Regex.IsMatch(newName, "^[A-Za-z0-9_]{4}$"))
			{
				MessageBox.Show("Invalid filename. Please use exactly 2 characters with letters, numbers, or underscores.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			byte[] newNameBytes = Encoding.ASCII.GetBytes(newName.PadRight(4, '\0'));
			Array.Copy(newNameBytes, 0, armValues, kartOffset, newNameBytes.Length);
			NewKartName = newName;
			DialogResult = DialogResult.OK;
		}
	}
}