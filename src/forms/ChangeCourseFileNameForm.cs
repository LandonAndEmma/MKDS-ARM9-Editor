using System.Text;
using System.Text.RegularExpressions;
namespace ARM9Editor
{
    public partial class ChangeCourseFileNameForm : Form
    {
        private readonly int _nameOffset;
        private readonly int _sizeOffset;
        private readonly byte[] _armValues;
        public ChangeCourseFileNameForm(byte[] armValues, int nameOffset, int sizeOffset)
        {
            InitializeComponent();
            _armValues = armValues ?? throw new ArgumentNullException(nameof(armValues));
            _nameOffset = nameOffset;
            _sizeOffset = sizeOffset;
            currentFileNameTextBox.Text = GetCurrentFileName();
        }
        private string GetCurrentFileName()
        {
            int length = _sizeOffset - _nameOffset;
            if (length <= 0 || _nameOffset < 0 || _sizeOffset > _armValues.Length)
            {
                return string.Empty;
            }
            byte[] nameBytes = new byte[length];
            Array.Copy(_armValues, _nameOffset, nameBytes, 0, length);
            return Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');
        }
        private void okButton_Click(object? sender, EventArgs e)
        {
            string newName = newFileNameTextBox.Text;
            int maxLength = _sizeOffset - _nameOffset;

            if (maxLength <= 0)
            {
                _ = MessageBox.Show("Invalid name block size.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(newName))
            {
                DialogResult result = MessageBox.Show(
                    "You have made the new name blank. Do you want to continue?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            else if (!Regex.IsMatch(newName, @"^[A-Za-z0-9_]+$"))
            {
                _ = MessageBox.Show(
                    "Invalid characters detected. Please use only English letters, numbers, and underscores.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            else if (Encoding.UTF8.GetByteCount(newName) > maxLength)
            {
                _ = MessageBox.Show(
                    $"New file name exceeds block size ({maxLength} bytes).",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            byte[] newNameBytes = Encoding.UTF8.GetBytes(newName);
            Array.Copy(newNameBytes, 0, _armValues, _nameOffset, newNameBytes.Length);
            Array.Clear(_armValues, _nameOffset + newNameBytes.Length, maxLength - newNameBytes.Length);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}