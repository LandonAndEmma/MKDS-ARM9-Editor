using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
namespace ARM9Editor
{
    public partial class ChangeCharacterFileNameForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NewCharacterName { get; private set; } = string.Empty;
        private readonly int _characterOffset;
        private readonly byte[] _armValues;
        public ChangeCharacterFileNameForm(byte[] armValues, int characterOffset)
        {
            InitializeComponent();
            _armValues = armValues ?? throw new ArgumentNullException(nameof(armValues));
            _characterOffset = characterOffset;
            currentCharacterPrefixTextBox.Text = GetCurrentCharacterPrefix();
        }
        private string GetCurrentCharacterPrefix()
        {
            byte[] characterBytes = new byte[4];
            Array.Copy(_armValues, _characterOffset, characterBytes, 0, characterBytes.Length);
            return Encoding.ASCII.GetString(characterBytes).TrimEnd('\0');
        }
        private void okButton_Click(object? sender, EventArgs e)
        {
            string newName = newCharacterPrefixTextBox.Text;

            if (string.IsNullOrEmpty(newName))
            {
                DialogResult result = MessageBox.Show(
                    "You have made the new filename blank. Do you want to continue?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
                newName = new string('\0', 4);
            }
            else if (newName.Length != 4 || !Regex.IsMatch(newName, @"^[A-Za-z0-9_]{4}$"))
            {
                _ = MessageBox.Show(
                    "Invalid filename. Please use exactly 4 characters with letters, numbers, or underscores.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            byte[] newNameBytes = Encoding.ASCII.GetBytes(newName.PadRight(4, '\0'));
            Array.Copy(newNameBytes, 0, _armValues, _characterOffset, 4);
            NewCharacterName = newName;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}