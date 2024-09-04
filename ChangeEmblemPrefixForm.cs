using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace ARM9Editor
{
	public partial class ChangeEmblemPrefixForm : Form
	{
		public string NewEmblemName { get; private set; }
		private int emblemOffset;
		private byte[] armValues;

		public ChangeEmblemPrefixForm(byte[] armValues, int emblemOffset)
		{
			InitializeComponent();
			this.armValues = armValues;
			this.emblemOffset = emblemOffset;
		}

		private void changeButton_Click(object sender, EventArgs e)
		{
			string newName = emblemNameTextBox.Text;
			if (string.IsNullOrEmpty(newName))
			{
				if (MessageBox.Show("You have made the new prefix blank. Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return;
				newName = new string('\0', 2); // Blank name
			}
			else if (newName.Length != 2 || !Regex.IsMatch(newName, "^[A-Za-z0-9_]{2}$"))
			{
				MessageBox.Show("Invalid name. Please use exactly 2 characters with letters, numbers, or underscores.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			byte[] newNameBytes = Encoding.ASCII.GetBytes(newName.PadRight(2, '\0'));
			Array.Copy(newNameBytes, 0, armValues, emblemOffset, newNameBytes.Length);
			NewEmblemName = newName;
			DialogResult = DialogResult.OK;
		}
	}
}