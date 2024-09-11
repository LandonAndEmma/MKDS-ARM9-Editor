using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
namespace ARM9Editor
{
    public partial class App : Form
    {
        private string arm9BinPath = null;
        private byte[] armValues = null;
        private Dictionary<string, int> musicOffsets;
        private Dictionary<string, Tuple<int, int>> courseOffsets;
        private Dictionary<string, int> slotOffsets;
        private Dictionary<string, int> emblemOffsets;
        private Dictionary<string, int> kartOffsets;
        private Dictionary<string, int> characterOffsets;
        public App()
        {
            InitializeComponent();
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            repositoryToolStripMenuItem.Click += repositoryToolStripMenuItem_Click;
            infoToolStripMenuItem.Click += infoToolStripMenuItem_Click;
            musiclistBox.DoubleClick += musicListBox_DoubleClick;
            courselistBox.DoubleClick += courseListBox_DoubleClick;
            weatherlistBox.DoubleClick += weatherListBox_DoubleClick;
            emblemlistBox.DoubleClick += emblemListBox_DoubleClick;
            kartlistBox.DoubleClick += kartListBox_DoubleClick;
            characterlistBox.DoubleClick += characterListBox_DoubleClick;
            LoadMusicOffsets();
            LoadCourseOffsets();
            LoadSlotOffsets();
            LoadEmblemOffsets();
            LoadKartOffsets();
            LoadCharacterOffsets();
        }
        private void LoadMusicOffsets()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ARM9Editor.assets.json.music_offsets.json";
            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream);
                string jsonData = reader.ReadToEnd();
                musicOffsets = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load music offsets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadCourseOffsets()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ARM9Editor.assets.json.course_offsets.json";
            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream);
                string jsonData = reader.ReadToEnd();
                {
                    var tempOffsets = JsonConvert.DeserializeObject<Dictionary<string, int[]>>(jsonData);

                    courseOffsets = new Dictionary<string, Tuple<int, int>>();

                    foreach (var kvp in tempOffsets)
                    {
                        if (kvp.Value.Length == 2)
                        {
                            courseOffsets.Add(kvp.Key, new Tuple<int, int>(kvp.Value[0], kvp.Value[1]));
                        }
                        else
                        {
                            MessageBox.Show($"Invalid offset data for course {kvp.Key}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load course offsets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadSlotOffsets()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ARM9Editor.assets.json.slot_offsets.json";
            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream);
                string jsonData = reader.ReadToEnd();
                slotOffsets = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load slot offsets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadEmblemOffsets()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ARM9Editor.assets.json.emblem_offsets.json";
            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream);
                string jsonData = reader.ReadToEnd();
                emblemOffsets = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load emblem offsets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadKartOffsets()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ARM9Editor.assets.json.kart_offsets.json";
            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream);
                string jsonData = reader.ReadToEnd();
                kartOffsets = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load kart offsets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadCharacterOffsets()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ARM9Editor.assets.json.character_offsets.json";
            try
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream);
                string jsonData = reader.ReadToEnd();
                characterOffsets = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load character offsets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GetFileName(int startOffset, int endOffset)
        {
            if (armValues == null || startOffset < 0 || endOffset > armValues.Length || startOffset >= endOffset)
            {
                return string.Empty;
            }
            return Encoding.UTF8.GetString(armValues, startOffset, endOffset - startOffset).TrimEnd('\0');
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
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
                RefreshCourseListBox();
                RefreshWeatherListBox();
                RefreshEmblemListBox();
                RefreshKartListBox();
                RefreshCharacterListBox();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(arm9BinPath);
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
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
        private void RefreshCourseListBox()
        {
            if (courseOffsets == null || armValues == null)
            {
                MessageBox.Show("Course offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            courselistBox.Items.Clear();
            foreach (var kvp in courseOffsets)
            {
                string displayText = $"{kvp.Key} [{GetFileName(kvp.Value.Item1, kvp.Value.Item2)}]";
                courselistBox.Items.Add(displayText);
            }
        }
        private void RefreshWeatherListBox()
        {
            if (slotOffsets == null || armValues == null)
            {
                MessageBox.Show("Slot offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            weatherlistBox.Items.Clear();
            foreach (var kvp in slotOffsets)
            {
                string displayText = $"{kvp.Key} [{armValues[kvp.Value]}]";
                weatherlistBox.Items.Add(displayText);
            }
        }
        private void RefreshEmblemListBox()
        {
            if (emblemOffsets == null || armValues == null)
            {
                MessageBox.Show("Emblem offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            emblemlistBox.Items.Clear();
            foreach (var kvp in emblemOffsets)
            {
                string emblemName = kvp.Key;
                int offset = kvp.Value;
                string emblemValue = Encoding.ASCII.GetString(armValues, offset, 2).TrimEnd('\0');
                emblemlistBox.Items.Add($"{emblemName} [{emblemValue}]");
            }
        }
        private void RefreshKartListBox()
        {
            if (emblemOffsets == null || armValues == null)
            {
                MessageBox.Show("Kart offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            kartlistBox.Items.Clear();
            foreach (var kvp in kartOffsets)
            {
                string kartName = kvp.Key;
                int offset = kvp.Value;
                string kartValue = Encoding.ASCII.GetString(armValues, offset, 4).TrimEnd('\0');
                kartlistBox.Items.Add($"{kartName} [{kartValue}]");
            }
        }
        private void RefreshCharacterListBox()
        {
            if (characterOffsets == null || armValues == null)
            {
                MessageBox.Show("Character offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            characterlistBox.Items.Clear();
            foreach (var kvp in characterOffsets)
            {
                string characterName = kvp.Key;
                int offset = kvp.Value;
                string characterValue = Encoding.ASCII.GetString(armValues, offset, 4).TrimEnd('\0');
                characterlistBox.Items.Add($"{characterName} [{characterValue}]");
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
                using var form = new ChangeSeqValueForm(armValues[offset]);
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
        private void courseListBox_DoubleClick(object sender, EventArgs e)
        {
            if (courseOffsets == null || armValues == null)
            {
                MessageBox.Show("Course offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (courselistBox.SelectedItem != null)
            {
                string selectedItem = courselistBox.SelectedItem.ToString();
                string courseName = selectedItem.Split('[')[0].Trim();
                var offsets = courseOffsets[courseName];

                using var form = new ChangeCourseFileNameForm(armValues, offsets.Item1, offsets.Item2);
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshCourseListBox();
                        MessageBox.Show($"File name for {courseName} changed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void weatherListBox_DoubleClick(object sender, EventArgs e)
        {
            if (slotOffsets == null || armValues == null)
            {
                MessageBox.Show("Slot offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (weatherlistBox.SelectedItem != null)
            {
                string selectedItem = weatherlistBox.SelectedItem.ToString();
                string slotName = selectedItem.Split('[')[0].Trim();
                int offset = slotOffsets[slotName];

                using var form = new ChangeSlotValueForm(armValues[offset]);
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int newSlotValue = form.NewSlotValue;
                        if (newSlotValue >= 1 && newSlotValue <= 54)
                        {
                            armValues[offset] = (byte)newSlotValue;
                            RefreshWeatherListBox();
                            MessageBox.Show($"Slot value for {slotName} changed to {newSlotValue}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Invalid slot value. Value must be between 1 and 54.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void emblemListBox_DoubleClick(object sender, EventArgs e)
        {
            if (emblemOffsets == null || armValues == null)
            {
                MessageBox.Show("Emblem offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (emblemlistBox.SelectedItem != null)
            {
                string selectedItem = emblemlistBox.SelectedItem.ToString();
                string emblemName = selectedItem.Split('[')[0].Trim();
                int offset = emblemOffsets[emblemName];

                using var form = new ChangeEmblemPrefixForm(armValues, offset);
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshEmblemListBox();
                        MessageBox.Show($"Emblem name for {emblemName} changed to {form.NewEmblemName}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void kartListBox_DoubleClick(object sender, EventArgs e)
        {
            if (kartOffsets == null || armValues == null)
            {
                MessageBox.Show("Kart offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (kartlistBox.SelectedItem != null)
            {
                string selectedItem = kartlistBox.SelectedItem.ToString();
                string kartName = selectedItem.Split('[')[0].Trim();
                int offset = kartOffsets[kartName];

                using var form = new ChangeKartFileNameForm(armValues, offset);
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshKartListBox();
                        MessageBox.Show($"Kart name for {kartName} changed to {form.NewKartName}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void characterListBox_DoubleClick(object sender, EventArgs e)
        {
            if (characterOffsets == null || armValues == null)
            {
                MessageBox.Show("Character offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (characterlistBox.SelectedItem != null)
            {
                string selectedItem = characterlistBox.SelectedItem.ToString();
                string characterName = selectedItem.Split('[')[0].Trim();
                int offset = characterOffsets[characterName];

                using var form = new ChangeCharacterFileNameForm(armValues, offset);
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshCharacterListBox();
                        MessageBox.Show($"Character name for {characterName} changed to {form.NewCharacterName}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void repositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string githubUrl = "https://github.com/LandonAndEmma/MKDS-ARM9-Editor";
            try
            {
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
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This program allows you to edit many values in the arm9.bin file of Mario Kart DS.\n\n Code: Landon & Emma\n Special Thanks: Ermelber, Yami, MkDasher", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}