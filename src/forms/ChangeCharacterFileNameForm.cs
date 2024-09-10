using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace ARM9Editor
{
    public partial class ChangeCharacterFileNameForm : Form
    {
        public string NewCharacterName { get; private set; }
        private readonly int characterOffset;
        private readonly byte[] armValues;
        private string GetCurrentCharacterPrefix()
        {
            byte[] characterBytes = new byte[4];
            Array.Copy(armValues, characterOffset, characterBytes, 0, characterBytes.Length);
            return Encoding.ASCII.GetString(characterBytes).TrimEnd('\0');
        }
        public ChangeCharacterFileNameForm(byte[] armValues, int characterOffset)
        {
            InitializeComponent();
            this.armValues = armValues;
            this.characterOffset = characterOffset;
            currentCharacterPrefixTextBox.Text = GetCurrentCharacterPrefix();
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            string newName = newCharacterPrefixTextBox.Text;
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
            Array.Copy(newNameBytes, 0, armValues, characterOffset, newNameBytes.Length);
            NewCharacterName = newName;
            DialogResult = DialogResult.OK;
        }
    }
}