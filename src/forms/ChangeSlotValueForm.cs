using System.ComponentModel;
namespace ARM9Editor
{
    public partial class ChangeSlotValueForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NewSlotValue { get; private set; }
        public ChangeSlotValueForm(int currentValue)
        {
            InitializeComponent();
            inputNumericUpDown.Value = Math.Clamp(currentValue, inputNumericUpDown.Minimum, inputNumericUpDown.Maximum);
        }
        private void okButton_Click(object? sender, EventArgs e)
        {
            int newValue = (int)inputNumericUpDown.Value;
            if (newValue is >= 1 and <= 54)
            {
                NewSlotValue = newValue;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                _ = MessageBox.Show(
                    "Invalid slot value. Value must be between 1 and 54.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}