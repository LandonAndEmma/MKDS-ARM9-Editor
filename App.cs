using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class App : Form
    {
        public App()
        {
            InitializeComponent();
            // Attach the Click event handlers
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            this.repositoryToolStripMenuItem.Click += new System.EventHandler(this.repositoryToolStripMenuItem_Click);
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
        }

        // Event handler for "Open" menu item
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set filter for .bin files
                openFileDialog.Filter = "Binary files (*.bin)|*.bin";
                openFileDialog.Title = "Open a Binary File";

                // Show the dialog and check if a file was selected
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file path
                    string filePath = openFileDialog.FileName;

                    // Perform actions with the selected file path
                    MessageBox.Show($"Selected file: {filePath}", "File Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // TODO: Add code to handle the selected file (e.g., load and process the file)
                }
            }
        }

        // Event handler for "Repository" menu item
        private void repositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Define the URL you want to open
            string githubUrl = "https://github.com/LandonAndEmma/MKDS-ARM9-EDITOR-VS";

            try
            {
                // Open the URL in the default web browser
                Process.Start(new ProcessStartInfo
                {
                    FileName = githubUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the URL. " + ex.Message);
            }
        }

        // Event handler for "Help" menu item
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Show a MessageBox with custom text
            MessageBox.Show("This program allows you to edit many values in the arm9.bin file of Mario Kart DS.\n\n Code: Landon & Emma\n Special Thanks: Ermelber, Yami, MkDasher", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
