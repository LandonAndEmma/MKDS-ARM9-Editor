using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ARM9Editor
{
    public partial class App : Form
    {
        private string arm9BinPath = null;
        private byte[] armValues = null;
        private Dictionary<string, int> musicOffsets;

        public App()
        {
            InitializeComponent();
            // Attach the Click event handlers
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            this.repositoryToolStripMenuItem.Click += new System.EventHandler(this.repositoryToolStripMenuItem_Click);
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            this.musiclistBox.DoubleClick += new System.EventHandler(this.musicListBox_DoubleClick);

            LoadMusicOffsets(); // Initialize music offsets on form load
        }

        private void LoadMusicOffsets()
        {
            // Load the music_offsets.json file embedded as a resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ARM9Editor.music_offsets.json"; // Replace with your correct namespace and file name

            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonData = reader.ReadToEnd();
                    musicOffsets = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load music offsets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Binary files (*.bin)|*.bin",
                Title = "Open a Binary File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                arm9BinPath = openFileDialog.FileName;

                if (new FileInfo(arm9BinPath).Length == 0)
                {
                    MessageBox.Show("This is not a valid arm9.bin file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                armValues = File.ReadAllBytes(arm9BinPath);
                RefreshMusicListBox();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(arm9BinPath);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "bin",
                Filter = "Binary files (*.bin)|*.bin"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFile(saveFileDialog.FileName);
            }
        }

        private void SaveFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || armValues == null)
            {
                MessageBox.Show("No valid file is opened for saving.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                File.WriteAllBytes(filePath, armValues);
                MessageBox.Show("Modified file saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshMusicListBox()
        {
            if (musicOffsets == null || armValues == null)
            {
                MessageBox.Show("Music offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            musiclistBox.Items.Clear();
            foreach (var kvp in musicOffsets)
            {
                string displayText = $"{kvp.Key} [{armValues[kvp.Value]}]";
                musiclistBox.Items.Add(displayText);
            }
        }

        private void musicListBox_DoubleClick(object sender, EventArgs e)
        {
            if (musicOffsets == null || armValues == null)
            {
                MessageBox.Show("Music offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (musiclistBox.SelectedItem != null)
            {
                string selectedItem = musiclistBox.SelectedItem.ToString();
                string musicName = selectedItem.Split('[')[0].Trim();
                int offset = musicOffsets[musicName];

                using (var form = new ChangeSeqValueForm(armValues[offset]))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int newSeqValue = form.NewSeqValue;
                        if (newSeqValue >= 0 && newSeqValue <= 75)
                        {
                            armValues[offset] = (byte)newSeqValue;
                            RefreshMusicListBox();
                            MessageBox.Show($"SEQ value for {musicName} changed to {newSeqValue}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Invalid SEQ value. Value must be between 0 and 75.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
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

        // Event handler for "Info" menu item
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show a MessageBox with custom text
            MessageBox.Show("This program allows you to edit many values in the arm9.bin file of Mario Kart DS.\n\n Code: Landon & Emma\n Special Thanks: Ermelber, Yami, MkDasher", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
