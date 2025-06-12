using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text;
namespace ARM9Editor
{
    public partial class App : Form
    {
        private string? _arm9BinPath;
        private byte[]? _armValues;
        private Dictionary<string, int>? _musicOffsets;
        private Dictionary<string, (int Start, int End)>? _courseOffsets;
        private Dictionary<string, int>? _slotOffsets;
        private Dictionary<string, int>? _emblemOffsets;
        private Dictionary<string, int>? _kartOffsets;
        private Dictionary<string, int>? _characterOffsets;
        private ListBox? _onlineCourseListBox;
        private static readonly byte[] OnlinePatchBlock =
        [
            0x2C, 0x00, 0x9F, 0xE5, 0x2C, 0x10, 0x9F, 0xE5, 0x00, 0x10, 0x80, 0xE5,
            0x28, 0x00, 0x9F, 0xE5, 0x2C, 0x10, 0x8F, 0xE2, 0x14, 0x20, 0xA0, 0xE3,
            0x01, 0x30, 0xD1, 0xE4, 0x04, 0x30, 0x80, 0xE4, 0x01, 0x20, 0x52, 0xE2,
            0xFB, 0xFF, 0xFF, 0xCA, 0x10, 0x00, 0x9F, 0xE5, 0x10, 0xFF, 0x2F, 0xE1,
            0xFE, 0xFF, 0xFF, 0xEA, 0x50, 0xFE, 0x3F, 0x02, 0x00, 0xB6, 0x0E, 0x00,
            0x4C, 0x3A, 0x15, 0x02, 0x00, 0x30, 0x00, 0x02
        ];
        private static readonly byte[] OnlineCourseIds =
        [
            0x14, 0x16, 0x1F, 0x12, 0x1B, 0x1C, 0x21, 0x18, 0x1E, 0x11,
            0x19, 0x13, 0x22, 0x1A, 0x20, 0x1D, 0x0A, 0x0B, 0x0D, 0x0E
        ];
        public App()
        {
            InitializeComponent();
            WireUpEvents();
            LoadAllOffsets();
        }
        private void WireUpEvents()
        {
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            saveAsToolStripMenuItem.Click += SaveAsToolStripMenuItem_Click;
            infoToolStripMenuItem.Click += InfoToolStripMenuItem_Click;
            repositoryToolStripMenuItem.Click += RepositoryToolStripMenuItem_Click;
            checkForUpdatesToolStripMenuItem.Click += checkForUpdatesToolStripMenuItem_Click;
            editableOnlineTrackSelectionToolStripMenuItem.Click += EditableOnlineTrackSelectionToolStripMenuItem_Click;
            musiclistBox.DoubleClick += MusicListBox_DoubleClick;
            courselistBox.DoubleClick += CourseListBox_DoubleClick;
            weatherlistBox.DoubleClick += WeatherListBox_DoubleClick;
            emblemlistBox.DoubleClick += EmblemListBox_DoubleClick;
            kartlistBox.DoubleClick += KartListBox_DoubleClick;
            characterlistBox.DoubleClick += CharacterListBox_DoubleClick;
        }
        private void LoadAllOffsets()
        {
            _musicOffsets = LoadJsonResource<Dictionary<string, int>>("ARM9Editor.assets.json.music_offsets.json", "music offsets");
            _courseOffsets = LoadCourseOffsets();
            _slotOffsets = LoadJsonResource<Dictionary<string, int>>("ARM9Editor.assets.json.slot_offsets.json", "slot offsets");
            _emblemOffsets = LoadJsonResource<Dictionary<string, int>>("ARM9Editor.assets.json.emblem_offsets.json", "emblem offsets");
            _kartOffsets = LoadJsonResource<Dictionary<string, int>>("ARM9Editor.assets.json.kart_offsets.json", "kart offsets");
            _characterOffsets = LoadJsonResource<Dictionary<string, int>>("ARM9Editor.assets.json.character_offsets.json", "character offsets");
        }
        private static T? LoadJsonResource<T>(string resourceName, string errorLabel)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            try
            {
                using Stream stream = assembly.GetManifestResourceStream(resourceName)
                    ?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");
                using StreamReader reader = new(stream);
                string jsonData = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load {errorLabel}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default;
            }
        }
        private static Dictionary<string, (int Start, int End)>? LoadCourseOffsets()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "ARM9Editor.assets.json.course_offsets.json";
            try
            {
                using Stream stream = assembly.GetManifestResourceStream(resourceName)
                    ?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");
                using StreamReader reader = new(stream);
                string jsonData = reader.ReadToEnd();
                Dictionary<string, int[]> tempOffsets = JsonConvert.DeserializeObject<Dictionary<string, int[]>>(jsonData);
                Dictionary<string, (int, int)> result = [];
                foreach ((string key, int[] value) in tempOffsets)
                {
                    if (value.Length == 2)
                    {
                        result[key] = (value[0], value[1]);
                    }
                    else
                    {
                        MessageBox.Show($"Invalid offset data for course {key}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load course offsets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private string GetFileName(int startOffset, int endOffset)
        {
            return _armValues is null || startOffset < 0 || endOffset > _armValues.Length || startOffset >= endOffset
                ? string.Empty
                : Encoding.UTF8.GetString(_armValues, startOffset, endOffset - startOffset).TrimEnd('\0');
        }
        private void OpenToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new()
            {
                Filter = "Binary files (*.bin)|*.bin",
                Title = "Open a Binary File"
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            _arm9BinPath = openFileDialog.FileName;
            if (new FileInfo(_arm9BinPath).Length == 0)
            {
                MessageBox.Show("This is not a valid arm9.bin file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _armValues = File.ReadAllBytes(_arm9BinPath);
            RefreshAllListBoxes();
            RefreshOnlineTabAfterLoad();
        }
        private void SaveToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            SaveFile(_arm9BinPath);
        }
        private void SaveAsToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using SaveFileDialog saveFileDialog = new()
            {
                DefaultExt = "bin",
                Filter = "Binary files (*.bin)|*.bin"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFile(saveFileDialog.FileName);
            }
        }
        private void SaveFile(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath) || _armValues is null)
            {
                MessageBox.Show("No valid file is opened for saving.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_armValues.Length > 0xC77 && _armValues[0x918] == 0x20 && _armValues[0x919] == 0x0C)
            {
                int start = 0xC64, count = 0;
                bool foundZero = false;
                for (int i = 0; i < 20; i++)
                {
                    byte val = _armValues[start + i];
                    if (!foundZero)
                    {
                        if (val == 0x00)
                        {
                            foundZero = true;
                        }
                        else
                        {
                            count++;
                        }
                    }
                    else if (val != 0x00)
                    {
                        MessageBox.Show(
                            "Invalid online course list: All course IDs after the first course ID of 0 must also be 0.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                _armValues[0xC34] = (byte)count;
            }
            try
            {
                File.WriteAllBytes(filePath, _armValues);
                MessageBox.Show("Modified file saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RefreshAllListBoxes()
        {
            RefreshMusicListBox();
            RefreshCourseListBox();
            RefreshWeatherListBox();
            RefreshEmblemListBox();
            RefreshKartListBox();
            RefreshCharacterListBox();
        }
        private void RefreshMusicListBox()
        {
            if (_musicOffsets is null || _armValues is null)
            {
                MessageBox.Show("Music offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            musiclistBox.Items.Clear();
            foreach ((string key, int offset) in _musicOffsets)
            {
                musiclistBox.Items.Add($"{key} [{_armValues[offset]}]");
            }
        }
        private void RefreshCourseListBox()
        {
            if (_courseOffsets is null || _armValues is null)
            {
                MessageBox.Show("Course offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            courselistBox.Items.Clear();
            foreach ((string key, (int start, int end)) in _courseOffsets)
            {
                courselistBox.Items.Add($"{key} [{GetFileName(start, end)}]");
            }
        }
        private void RefreshWeatherListBox()
        {
            if (_slotOffsets is null || _armValues is null)
            {
                MessageBox.Show("Slot offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            weatherlistBox.Items.Clear();
            foreach ((string key, int offset) in _slotOffsets)
            {
                weatherlistBox.Items.Add($"{key} [{_armValues[offset]}]");
            }
        }
        private void RefreshEmblemListBox()
        {
            if (_emblemOffsets is null || _armValues is null)
            {
                MessageBox.Show("Emblem offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            emblemlistBox.Items.Clear();
            foreach ((string key, int offset) in _emblemOffsets)
            {
                string emblemValue = Encoding.ASCII.GetString(_armValues, offset, 2).TrimEnd('\0');
                emblemlistBox.Items.Add($"{key} [{emblemValue}]");
            }
        }
        private void RefreshKartListBox()
        {
            if (_kartOffsets is null || _armValues is null)
            {
                MessageBox.Show("Kart offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            kartlistBox.Items.Clear();
            foreach ((string key, int offset) in _kartOffsets)
            {
                string kartValue = Encoding.ASCII.GetString(_armValues, offset, 4).TrimEnd('\0');
                kartlistBox.Items.Add($"{key} [{kartValue}]");
            }
        }
        private void RefreshCharacterListBox()
        {
            if (_characterOffsets is null || _armValues is null)
            {
                MessageBox.Show("Character offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            characterlistBox.Items.Clear();
            foreach ((string key, int offset) in _characterOffsets)
            {
                string characterValue = Encoding.ASCII.GetString(_armValues, offset, 4).TrimEnd('\0');
                characterlistBox.Items.Add($"{key} [{characterValue}]");
            }
        }
        private void RefreshOnlineCourseListBox()
        {
            if (_armValues is null || _armValues.Length < 0xC64 + 20)
            {
                MessageBox.Show("ARM9 file not loaded or too small.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_onlineCourseListBox is null)
            {
                return;
            }
            _onlineCourseListBox.Items.Clear();
            for (int i = 0; i < 20; i++)
            {
                _onlineCourseListBox.Items.Add($"Course {i + 1} [{_armValues[0xC64 + i]}]");
            }
        }
        private void MusicListBox_DoubleClick(object? sender, EventArgs e)
        {
            if (_musicOffsets is null || _armValues is null)
            {
                MessageBox.Show("Music offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (musiclistBox.SelectedItem is not string selectedItem)
            {
                return;
            }
            string musicName = selectedItem.Split('[')[0].Trim();
            int offset = _musicOffsets[musicName];
            using ChangeSeqValueForm form = new(_armValues[offset]);
            if (form.ShowDialog() == DialogResult.OK)
            {
                int newSeqValue = form.NewSeqValue;
                if (newSeqValue is >= 0 and <= 75)
                {
                    _armValues[offset] = (byte)newSeqValue;
                    RefreshMusicListBox();
                    MessageBox.Show($"SEQ value for {musicName} changed to {newSeqValue}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Invalid SEQ value. Value must be between 0 and 75.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void CourseListBox_DoubleClick(object? sender, EventArgs e)
        {
            if (_courseOffsets is null || _armValues is null)
            {
                MessageBox.Show("Course offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (courselistBox.SelectedItem is not string selectedItem)
            {
                return;
            }
            string courseName = selectedItem.Split('[')[0].Trim();
            (int start, int end) = _courseOffsets[courseName];
            using ChangeCourseFileNameForm form = new(_armValues, start, end);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Controls["newFileNameTextBox"] is not TextBox tb || string.IsNullOrEmpty(tb.Text))
                {
                    RefreshCourseListBox();
                    return;
                }
                RefreshCourseListBox();
                MessageBox.Show($"File name for {courseName} changed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void WeatherListBox_DoubleClick(object? sender, EventArgs e)
        {
            if (_slotOffsets is null || _armValues is null)
            {
                MessageBox.Show("Slot offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (weatherlistBox.SelectedItem is not string selectedItem)
            {
                return;
            }
            string slotName = selectedItem.Split('[')[0].Trim();
            int offset = _slotOffsets[slotName];
            using ChangeSlotValueForm form = new(_armValues[offset]);
            if (form.ShowDialog() == DialogResult.OK)
            {
                int newSlotValue = form.NewSlotValue;
                if (newSlotValue is >= 1 and <= 54)
                {
                    _armValues[offset] = (byte)newSlotValue;
                    RefreshWeatherListBox();
                    MessageBox.Show($"Slot value for {slotName} changed to {newSlotValue}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Invalid slot value. Value must be between 1 and 54.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void EmblemListBox_DoubleClick(object? sender, EventArgs e)
        {
            if (_emblemOffsets is null || _armValues is null)
            {
                MessageBox.Show("Emblem offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (emblemlistBox.SelectedItem is not string selectedItem)
            {
                return;
            }
            string emblemName = selectedItem.Split('[')[0].Trim();
            int offset = _emblemOffsets[emblemName];
            using ChangeEmblemPrefixForm form = new(_armValues, offset);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Controls["newEmblemPrefixTextBox"] is not TextBox tb || string.IsNullOrEmpty(tb.Text))
                {
                    RefreshEmblemListBox();
                    return;
                }
                RefreshEmblemListBox();
                MessageBox.Show($"Emblem name for {emblemName} changed to {form.NewEmblemName}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void KartListBox_DoubleClick(object? sender, EventArgs e)
        {
            if (_kartOffsets is null || _armValues is null)
            {
                MessageBox.Show("Kart offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (kartlistBox.SelectedItem is not string selectedItem)
            {
                return;
            }
            string kartName = selectedItem.Split('[')[0].Trim();
            int offset = _kartOffsets[kartName];
            using ChangeKartFileNameForm form = new(_armValues, offset);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Controls["newKartPrefixTextBox"] is not TextBox tb || string.IsNullOrEmpty(tb.Text))
                {
                    RefreshKartListBox();
                    return;
                }
                RefreshKartListBox();
                MessageBox.Show($"Kart name for {kartName} changed to {form.NewKartName}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void CharacterListBox_DoubleClick(object? sender, EventArgs e)
        {
            if (_characterOffsets is null || _armValues is null)
            {
                MessageBox.Show("Character offsets or ARM values are not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (characterlistBox.SelectedItem is not string selectedItem)
            {
                return;
            }
            string characterName = selectedItem.Split('[')[0].Trim();
            int offset = _characterOffsets[characterName];
            using ChangeCharacterFileNameForm form = new(_armValues, offset);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Controls["newCharacterPrefixTextBox"] is not TextBox tb || string.IsNullOrEmpty(tb.Text))
                {
                    RefreshCharacterListBox();
                    return;
                }
                RefreshCharacterListBox();
                MessageBox.Show($"Character name for {characterName} changed to {form.NewCharacterName}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void OnlineCourseListBox_DoubleClick(object? sender, EventArgs e)
        {
            if (_onlineCourseListBox is null || _onlineCourseListBox.SelectedIndex < 0 || _armValues is null)
            {
                return;
            }

            int index = _onlineCourseListBox.SelectedIndex;
            int currentId = _armValues[0xC64 + index];
            using ChangeOnlineCourseIdForm form = new(currentId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                int newId = form.NewCourseID;
                if (newId is >= 0 and <= 54)
                {
                    _armValues[0xC64 + index] = (byte)newId;
                    RefreshOnlineCourseListBox();
                    MessageBox.Show($"Course ID {index + 1} changed to ({newId}).", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Invalid course ID. Value must be between 0 and 54.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void InfoToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
            string versionShort = string.Join('.', version.Split('.').Take(3));
            MessageBox.Show(
                $"Mario Kart DS ARM9 Editor\nVersion: {versionShort}\n\n" +
                "This program allows you to edit many values in the arm9.bin file of Mario Kart DS.\n\n" +
                "Code: Landon & Emma\nSpecial Thanks: Ermelber, Yami, MkDasher",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void RepositoryToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            const string githubUrl = "https://github.com/LandonAndEmma/MKDS-ARM9-Editor";
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
        private async void checkForUpdatesToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            const string exeName = "ARM9Editor.exe";
            const string newExeName = "ARM9Editor_new.exe";
            const string batchName = "update.bat";
            string apiUrl = $"https://api.github.com/repos/LandonAndEmma/MKDS-ARM9-Editor/releases/latest";
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
            string downloadPath = Path.Combine(exeDir, newExeName);
            string batchPath = Path.Combine(exeDir, batchName);
            string currentVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";
            string currentVersionShort = string.Join('.', currentVersion.Split('.').Take(3));

            try
            {
                using HttpClient client = new();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("ARM9Editor-Updater");
                string json = await client.GetStringAsync(apiUrl);
                dynamic release = JsonConvert.DeserializeObject(json);
                string latestTag = release.tag_name;
                string latestVersion = latestTag.Trim();
                string? downloadUrl = null;
                foreach (var asset in release.assets)
                {
                    if ((string)asset.name == exeName)
                    {
                        downloadUrl = asset.browser_download_url;
                        break;
                    }
                }
                if (downloadUrl is null)
                {
                    MessageBox.Show($"No {exeName} asset found in the latest release.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (Version.TryParse(currentVersionShort, out var currentVer) && Version.TryParse(latestVersion, out var latestVer))
                {
                    if (currentVer >= latestVer)
                    {
                        MessageBox.Show($"You are already up to date.\n\nCurrent version: {currentVersionShort}", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    if (currentVersionShort == latestVersion)
                    {
                        MessageBox.Show($"You are already up to date.\n\nCurrent version: {currentVersionShort}", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (MessageBox.Show($"A new version ({latestVersion}) is available. Download and update now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                using (var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    using (var fs = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
                string batchContent = $@"
@echo off
setlocal
cd /d ""{exeDir}""
:wait
tasklist | find /i ""{exeName}"" >nul 2>&1
if not errorlevel 1 (
    timeout /t 1 >nul
    goto wait
)
:deltry
del ""{exeName}"" >nul 2>&1
if exist ""{exeName}"" (
    timeout /t 1 >nul
    goto deltry
)
move /y ""{newExeName}"" ""{exeName}""
start """" ""{exeName}""
del ""%~f0""
";
                File.WriteAllText(batchPath, batchContent);

                Process.Start(new ProcessStartInfo
                {
                    FileName = batchPath,
                    UseShellExecute = true,
                    WorkingDirectory = exeDir
                });
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update check failed: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void EditableOnlineTrackSelectionToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (_armValues is null)
            {
                MessageBox.Show("No ARM9 file loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _armValues[0x918] = 0x20;
            _armValues[0x919] = 0x0C;
            Array.Copy(OnlinePatchBlock, 0, _armValues, 0xC20, OnlinePatchBlock.Length);
            Array.Copy(OnlineCourseIds, 0, _armValues, 0xC64, OnlineCourseIds.Length);
            MessageBox.Show("Editable Online Track Selection patch applied.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowOrHideOnlineCoursesTab();
        }
        private void ShowOrHideOnlineCoursesTab()
        {
            bool isPatched = _armValues is { Length: > 0x919 } && _armValues[0x918] == 0x20 && _armValues[0x919] == 0x0C;
            TabPage? onlineTab = null;
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == "Online Courses")
                {
                    onlineTab = tab;
                    break;
                }
            }
            if (isPatched)
            {
                if (onlineTab is null)
                {
                    onlineTab = new TabPage("Online Courses");
                    _onlineCourseListBox = new ListBox
                    {
                        Dock = DockStyle.Fill,
                        FormattingEnabled = true
                    };
                    _onlineCourseListBox.DoubleClick += OnlineCourseListBox_DoubleClick;
                    onlineTab.Controls.Add(_onlineCourseListBox);
                    tabControl1.TabPages.Add(onlineTab);
                }
                else if (_onlineCourseListBox is null)
                {
                    _onlineCourseListBox = new ListBox
                    {
                        Dock = DockStyle.Fill,
                        FormattingEnabled = true
                    };
                    _onlineCourseListBox.DoubleClick += OnlineCourseListBox_DoubleClick;
                    onlineTab.Controls.Add(_onlineCourseListBox);
                }
                RefreshOnlineCourseListBox();
            }
            else if (onlineTab is not null)
            {
                tabControl1.TabPages.Remove(onlineTab);
                _onlineCourseListBox = null;
            }
        }
        private void RefreshOnlineTabAfterLoad()
        {
            ShowOrHideOnlineCoursesTab();
        }
    }
}