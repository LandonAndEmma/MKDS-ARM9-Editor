using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
namespace ARM9Editor
{
    public partial class ChangeEmblemPrefixForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NewEmblemName { get; private set; } = string.Empty;
        private readonly int _emblemOffset;
        private readonly byte[] _armValues;
        public ChangeEmblemPrefixForm(byte[] armValues, int emblemOffset)
        {
            InitializeComponent();
            _armValues = armValues ?? throw new ArgumentNullException(nameof(armValues));
            _emblemOffset = emblemOffset;
            currentEmblemPrefixTextBox.Text = GetCurrentEmblemPrefix();
        }
        private string GetCurrentEmblemPrefix()
        {
            byte[] emblemBytes = new byte[2];
            Array.Copy(_armValues, _emblemOffset, emblemBytes, 0, emblemBytes.Length);
            return Encoding.ASCII.GetString(emblemBytes).TrimEnd('\0');
        }
        private void okButton_Click(object? sender, EventArgs e)
        {
            string newName = newEmblemPrefixTextBox.Text;
            if (string.IsNullOrEmpty(newName))
            {
                DialogResult result = MessageBox.Show(
                    "You have made the new prefix blank. Do you want to continue?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }

                newName = "\0\0";
            }
            else if (newName.Length != 2 || !Regex.IsMatch(newName, @"^[A-Za-z0-9_]{2}$"))
            {
                _ = MessageBox.Show(
                    "Invalid name. Please use exactly 2 characters with letters, numbers, or underscores.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            byte[] newNameBytes = Encoding.ASCII.GetBytes(newName.PadRight(2, '\0'));
            Array.Copy(newNameBytes, 0, _armValues, _emblemOffset, newNameBytes.Length);
            NewEmblemName = newName;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}