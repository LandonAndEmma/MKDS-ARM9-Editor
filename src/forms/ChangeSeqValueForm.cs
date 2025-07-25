﻿using System.ComponentModel;
namespace ARM9Editor
{
    public partial class ChangeSeqValueForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NewSeqValue { get; private set; }
        public ChangeSeqValueForm(int currentValue)
        {
            InitializeComponent();
            inputNumericUpDown.Value = Math.Clamp(currentValue, inputNumericUpDown.Minimum, inputNumericUpDown.Maximum);
        }
        private void okButton_Click(object? sender, EventArgs e)
        {
            int value = (int)inputNumericUpDown.Value;
            if (value is >= 0 and <= 75)
            {
                NewSeqValue = value;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                _ = MessageBox.Show(
                    "Invalid SEQ value. Value must be between 0 and 75.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}