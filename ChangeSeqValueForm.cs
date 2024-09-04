using System;
using System.Windows.Forms;
namespace ARM9Editor
{
    public partial class ChangeSeqValueForm : Form
    {
        public int NewSeqValue { get; private set; }
        public ChangeSeqValueForm(int currentValue)
        {
            InitializeComponent();
            inputNumericUpDown.Text = currentValue.ToString();
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(inputNumericUpDown.Text, out int result))
            {
                NewSeqValue = result;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}