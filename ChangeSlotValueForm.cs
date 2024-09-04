using System;
using System.Windows.Forms;

namespace ARM9Editor
{
    public partial class ChangeSlotValueForm : Form
    {
        public int NewSlotValue { get; private set; }

        public ChangeSlotValueForm(int currentValue)
        {
            InitializeComponent();
            inputNumericUpDown.Value = currentValue;
        }

        private void changeSlotValueButton_Click(object sender, EventArgs e)
        {
            int newValue = (int)inputNumericUpDown.Value;
            if (newValue >= 1 && newValue <= 54)
            {
                NewSlotValue = newValue;
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid course value. Value must be between 1 and 54.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
