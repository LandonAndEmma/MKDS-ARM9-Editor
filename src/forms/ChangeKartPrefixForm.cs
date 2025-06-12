using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
namespace ARM9Editor
{
    public partial class ChangeKartFileNameForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NewKartName { get; private set; } = string.Empty;
        private readonly int _kartOffset;
        private readonly byte[] _armValues;
        public ChangeKartFileNameForm(byte[] armValues, int kartOffset)
        {
            InitializeComponent();
            _armValues = armValues ?? throw new ArgumentNullException(nameof(armValues));
            _kartOffset = kartOffset;
            currentKartPrefixTextBox.Text = GetCurrentKartPrefix();
        }
        private string GetCurrentKartPrefix()
        {
            byte[] kartBytes = new byte[4];
            Array.Copy(_armValues, _kartOffset, kartBytes, 0, kartBytes.Length);
            return Encoding.ASCII.GetString(kartBytes).TrimEnd('\0');
        }
        private void okButton_Click(object? sender, EventArgs e)
        {
            string newName = newKartPrefixTextBox.Text;
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
            Array.Copy(newNameBytes, 0, _armValues, _kartOffset, 4);
            NewKartName = newName;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}