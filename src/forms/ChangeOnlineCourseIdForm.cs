using System.ComponentModel;
namespace ARM9Editor
{
    public partial class ChangeOnlineCourseIdForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NewCourseID { get; private set; }
        public ChangeOnlineCourseIdForm(int currentValue)
        {
            InitializeComponent();
            inputNumericUpDown.Value = Math.Clamp(currentValue, inputNumericUpDown.Minimum, inputNumericUpDown.Maximum);
        }
        private void okButton_Click(object? sender, EventArgs e)
        {
            int newValue = (int)inputNumericUpDown.Value;
            if (newValue is >= 0 and <= 54)
            {
                NewCourseID = newValue;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                _ = MessageBox.Show(
                    "Invalid course value. Value must be between 0 and 54.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}