using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
namespace ARM9Editor
{
    public enum TabType { Music, Courses, Weather, Emblems, Karts, Characters }
    public static class ResourceLoader
    {
        public static T? LoadOffsets<T>(string fileName)
        {
            try
            {
                string resourceName = $"ARM9Editor.assets.json.{fileName}";
                using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    _ = MessageBox.Show($"Resource not found: {fileName}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return default;
                }
                using var reader = new StreamReader(stream);
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Failed to load {fileName}: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default;
            }
        }
        public static Dictionary<string, (int Start, int End)>? LoadCourseOffsets()
        {
            Dictionary<string, int[]>? temp = LoadOffsets<Dictionary<string, int[]>>("course_offsets.json");
            if (temp == null)
            {
                return null;
            }

            var result = new Dictionary<string, (int, int)>(temp.Count);
            foreach ((string? key, int[]? val) in temp.Where(kv => kv.Value?.Length == 2))
            {
                result[key] = (val[0], val[1]);
            }

            return result;
        }
    }
    [SupportedOSPlatform("windows6.1")]
    public partial class MainForm : Form
    {
        #region Constants
        private const int ONLINE_PATCH_CHECK_OFFSET1 = 0x918;
        private const int ONLINE_PATCH_CHECK_OFFSET2 = 0x919;
        private const byte ONLINE_PATCH_BYTE1 = 0x20;
        private const byte ONLINE_PATCH_BYTE2 = 0x0C;
        private const int ONLINE_PATCH_START = 0xC20;
        private const int ONLINE_COURSES_START = 0xC64;
        private const int ONLINE_COURSE_COUNT_OFFSET = 0xC34;
        private const int ONLINE_COURSE_COUNT = 20;
        private static readonly byte[] OnlinePatchBlock =
        {
            0x2C,0x00,0x9F,0xE5,0x2C,0x10,0x9F,0xE5,0x00,0x10,0x80,0xE5,
            0x28,0x00,0x9F,0xE5,0x2C,0x10,0x8F,0xE2,0x14,0x20,0xA0,0xE3,
            0x01,0x30,0xD1,0xE4,0x04,0x30,0x80,0xE4,0x01,0x20,0x52,0xE2,
            0xFB,0xFF,0xFF,0xCA,0x10,0x00,0x9F,0xE5,0x10,0xFF,0x2F,0xE1,
            0xFE,0xFF,0xFF,0xEA,0x50,0xFE,0x3F,0x02,0x00,0xB6,0x0E,0x00,
            0x4C,0x3A,0x15,0x02,0x00,0x30,0x00,0x02
        };
        private static readonly byte[] OnlineCourseIds =
        {
            0x14,0x16,0x1F,0x12,0x1B,0x1C,0x21,0x18,0x1E,0x11,
            0x19,0x13,0x22,0x1A,0x20,0x1D,0x0A,0x0B,0x0D,0x0E
        };
        #endregion
        #region Fields
        private Dictionary<string, int>? _musicOffsets;
        private Dictionary<string, (int Start, int End)>? _courseOffsets;
        private Dictionary<string, int>? _slotOffsets;
        private Dictionary<string, int>? _emblemOffsets;
        private Dictionary<string, int>? _kartOffsets;
        private Dictionary<string, int>? _characterOffsets;
        private string? _filePath;
        private byte[]? _data;
        private readonly Dictionary<string, TableLayoutPanel> _tablePanels = [];
        #endregion
        public MainForm()
        {
            InitializeComponent();
            InitializeTablePanels();
            LoadAllOffsets();
            WireEvents();
        }
        #region Initialization
        private void InitializeTablePanels()
        {
            _tablePanels["Music"] = musicTablePanel;
            _tablePanels["Courses"] = courseTablePanel;
            _tablePanels["Weather"] = weatherTablePanel;
            _tablePanels["Emblems"] = emblemTablePanel;
            _tablePanels["Karts"] = kartTablePanel;
            _tablePanels["Characters"] = characterTablePanel;
        }
        private void LoadAllOffsets()
        {
            _musicOffsets = ResourceLoader.LoadOffsets<Dictionary<string, int>>("music_offsets.json");
            _courseOffsets = ResourceLoader.LoadCourseOffsets();
            _slotOffsets = ResourceLoader.LoadOffsets<Dictionary<string, int>>("slot_offsets.json");
            _emblemOffsets = ResourceLoader.LoadOffsets<Dictionary<string, int>>("emblem_offsets.json");
            _kartOffsets = ResourceLoader.LoadOffsets<Dictionary<string, int>>("kart_offsets.json");
            _characterOffsets = ResourceLoader.LoadOffsets<Dictionary<string, int>>("character_offsets.json");
        }
        private void WireEvents()
        {
            openToolStripMenuItem!.Click += (_, _) => OpenFile();
            saveToolStripMenuItem!.Click += (_, _) => SaveFile(_filePath);
            saveAsToolStripMenuItem!.Click += (_, _) => SaveFileAs();
            infoToolStripMenuItem!.Click += (_, _) => ShowInfo();
            repositoryToolStripMenuItem!.Click += (_, _) => OpenRepository();
            editableOnlineTrackSelectionToolStripMenuItem!.Click += (_, _) => ApplyOnlinePatch();
        }
        #endregion
        #region File Operations
        private void OpenFile()
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Binary files (*.bin)|*.bin",
                Title = "Open ARM9.bin File"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                _data = File.ReadAllBytes(ofd.FileName);
                if (_data.Length == 0)
                {
                    ShowError("Invalid ARM9 file: File is empty.");
                    return;
                }
                _filePath = ofd.FileName;
                ClearAllHeaders();
                RefreshAllTables();
                UpdateOnlineCoursesTab();
            }
            catch (Exception ex)
            {
                ShowError($"Failed to open file: {ex.Message}");
            }
        }
        private void SaveFileAs()
        {
            using var sfd = new SaveFileDialog
            {
                DefaultExt = "bin",
                Filter = "Binary files (*.bin)|*.bin"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveFile(sfd.FileName);
            }
        }
        private void SaveFile(string? path)
        {
            if (string.IsNullOrEmpty(path) || _data == null)
            {
                ShowError("No file loaded.");
                return;
            }
            if (!ValidateOnlineCourses())
            {
                return;
            }

            try
            {
                File.WriteAllBytes(path, _data);
                _ = MessageBox.Show("File saved successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError($"Save failed: {ex.Message}");
            }
        }
        private bool ValidateOnlineCourses()
        {
            if (!IsOnlinePatchApplied() || _data == null)
            {
                return true;
            }

            int count = 0, index = ONLINE_COURSES_START;
            bool foundZero = false;
            for (int i = 0; i < ONLINE_COURSE_COUNT; i++, index++)
            {
                byte val = _data[index];
                if (!foundZero)
                {
                    if (val == 0)
                    {
                        foundZero = true;
                    }
                    else
                    {
                        count++;
                    }
                }
                else if (val != 0)
                {
                    ShowError("Invalid online courses: All IDs after first 0 must be 0.");
                    return false;
                }
            }
            _data[ONLINE_COURSE_COUNT_OFFSET] = (byte)count;
            return true;
        }
        #endregion
        #region Table Management
        private void RefreshAllTables()
        {
            RefreshTable("Music", "Music Track", "Music ID", _musicOffsets,
                (name, offset) => CreateNumericControl(offset, 0, 75));
            RefreshTable("Courses", "Course", "Filename", _courseOffsets,
                (name, range) => CreateCourseTextBox(range.Start, range.End));
            RefreshTable("Weather", "Weather Slot", "Course ID", _slotOffsets,
                (name, offset) => CreateNumericControl(offset, 1, 54));
            RefreshTable("Emblems", "Emblem", "Filename", _emblemOffsets,
                (name, offset) => CreateFixedTextBox(offset, 2));
            RefreshTable("Karts", "Kart", "Filename", _kartOffsets,
                (name, offset) => CreateFixedTextBox(offset, 4));
            RefreshTable("Characters", "Character", "Filename", _characterOffsets,
                (name, offset) => CreateFixedTextBox(offset, 4));
        }
        private void RefreshTable<T>(string panelName, string leftHeader, string rightHeader,
            Dictionary<string, T>? items, Func<string, T, Control> controlCreator)
        {
            if (!_tablePanels.TryGetValue(panelName, out TableLayoutPanel? panel) || items == null || _data == null)
            {
                return;
            }

            ClearPanel(panel);
            AddTableHeader(panel, leftHeader, rightHeader);
            foreach (KeyValuePair<string, T> item in items)
            {
                AddTableRow(panel, item.Key, controlCreator(item.Key, item.Value));
            }
        }
        private void ClearAllHeaders()
        {
            foreach (TableLayoutPanel panel in _tablePanels.Values)
            {
                RemoveHeader(panel);
            }
        }
        #endregion
        #region Control Creation
        private NumericUpDown CreateNumericControl(int offset, int min, int max)
        {
            var nud = new NumericUpDown
            {
                Minimum = min,
                Maximum = max,
                Value = _data != null && offset < _data.Length
                    ? Math.Clamp(_data[offset], min, max) : min,
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 4, 5, 4),
                Tag = offset
            };
            nud.ValueChanged += (s, _) =>
            {
                if (_data != null && s is NumericUpDown n && n.Tag is int off)
                {
                    _data[off] = (byte)n.Value;
                }
            };
            return nud;
        }
        private TextBox CreateCourseTextBox(int start, int end)
        {
            int maxLen = end - start;
            var tb = new TextBox
            {
                Text = ReadString(start, end),
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 4, 5, 4),
                Tag = (start, end, maxLen)
            };
            tb.Leave += (s, _) => ValidateAndWriteVariableString(s as TextBox);
            return tb;
        }
        private TextBox CreateFixedTextBox(int offset, int length)
        {
            var tb = new TextBox
            {
                Text = ReadFixedString(offset, length),
                MaxLength = length,
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 4, 5, 4),
                Tag = (offset, length)
            };
            tb.Leave += (s, _) => ValidateAndWriteFixedString(s as TextBox, length);
            return tb;
        }
        #endregion
        #region String Operations
        private void ValidateAndWriteVariableString(TextBox? tb)
        {
            if (tb?.Tag is not (int start, int end, int maxLen) || _data == null)
            {
                return;
            }

            string text = tb.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                Array.Clear(_data, start, maxLen);
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^[A-Za-z0-9_]+$"))
            {
                ShowError("Only alphanumeric characters and underscores allowed.");
                tb.Text = ReadString(start, end);
                return;
            }
            if (Encoding.UTF8.GetByteCount(text) > maxLen)
            {
                ShowError($"Maximum length: {maxLen} bytes.");
                tb.Text = ReadString(start, end);
                return;
            }
            WriteString(text, start, maxLen);
        }
        private void ValidateAndWriteFixedString(TextBox? tb, int length)
        {
            if (tb?.Tag is not (int offset, int len) || _data == null)
            {
                return;
            }

            string text = tb.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                Array.Clear(_data, offset, len);
                return;
            }
            if (text.Length != length || !System.Text.RegularExpressions.Regex.IsMatch(text, @"^[A-Za-z0-9_]+$"))
            {
                ShowError($"Must be exactly {length} alphanumeric characters or underscores.");
                tb.Text = ReadFixedString(offset, len);
                return;
            }
            WriteFixedString(text, offset, len);
        }
        private string ReadString(int start, int end)
        {
            return _data != null && start >= 0 && end <= _data.Length && start < end
                ? Encoding.UTF8.GetString(_data, start, end - start).TrimEnd('\0')
                : string.Empty;
        }

        private string ReadFixedString(int offset, int length)
        {
            return _data != null && offset >= 0 && offset + length <= _data.Length
                        ? Encoding.ASCII.GetString(_data, offset, length).TrimEnd('\0')
                        : string.Empty;
        }

        private void WriteString(string text, int start, int maxLen)
        {
            if (_data == null)
            {
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            Array.Copy(bytes, 0, _data, start, bytes.Length);
            Array.Clear(_data, start + bytes.Length, maxLen - bytes.Length);
        }
        private void WriteFixedString(string text, int offset, int length)
        {
            if (_data == null)
            {
                return;
            }

            byte[] bytes = Encoding.ASCII.GetBytes(text.PadRight(length, '\0'));
            Array.Copy(bytes, 0, _data, offset, length);
        }
        #endregion
        #region Online Courses
        private void UpdateOnlineCoursesTab()
        {
            bool patched = IsOnlinePatchApplied();
            TabPage? onlineTab = tabControl1?.TabPages.Cast<TabPage>()
                .FirstOrDefault(t => t.Text == "Online Courses");
            if (patched && onlineTab == null)
            {
                CreateOnlineCoursesTab();
            }
            else if (!patched && onlineTab != null)
            {
                tabControl1!.TabPages.Remove(onlineTab);
            }
            else if (patched && onlineTab != null)
            {
                RefreshOnlineCoursesTable();
            }
        }
        private void CreateOnlineCoursesTab()
        {
            TableLayoutPanel panel = CreateTablePanel();
            var wrapper = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            wrapper.Controls.Add(panel);
            var tab = new TabPage("Online Courses") { Padding = new Padding(6) };
            tab.Controls.Add(wrapper);
            tabControl1!.TabPages.Add(tab);
            panel.Tag = wrapper;
            RefreshOnlineCoursesTable(panel);
        }
        private void RefreshOnlineCoursesTable(TableLayoutPanel? panel = null)
        {
            if (_data == null || _data.Length < ONLINE_COURSES_START + ONLINE_COURSE_COUNT)
            {
                return;
            }

            panel ??= FindOnlineCoursesPanel();
            if (panel == null)
            {
                return;
            }

            ClearPanel(panel);
            AddTableHeader(panel, "Course", "Course ID");
            for (int i = 0; i < ONLINE_COURSE_COUNT; i++)
            {
                int offset = ONLINE_COURSES_START + i;
                AddTableRow(panel, $"Course {i + 1}", CreateNumericControl(offset, 0, 54));
            }
        }
        private TableLayoutPanel? FindOnlineCoursesPanel()
        {
            TabPage? onlineTab = tabControl1?.TabPages.Cast<TabPage>()
                .FirstOrDefault(t => t.Text == "Online Courses");
            return onlineTab?.Controls.OfType<Panel>().FirstOrDefault()
                ?.Controls.OfType<TableLayoutPanel>().FirstOrDefault();
        }
        private void ApplyOnlinePatch()
        {
            if (_data == null)
            {
                ShowError("No ARM9 file loaded.");
                return;
            }
            if (_data.Length < ONLINE_COURSES_START + OnlinePatchBlock.Length)
            {
                ShowError("File too small to apply patch.");
                return;
            }
            _data[ONLINE_PATCH_CHECK_OFFSET1] = ONLINE_PATCH_BYTE1;
            _data[ONLINE_PATCH_CHECK_OFFSET2] = ONLINE_PATCH_BYTE2;
            Array.Copy(OnlinePatchBlock, 0, _data, ONLINE_PATCH_START, OnlinePatchBlock.Length);
            Array.Copy(OnlineCourseIds, 0, _data, ONLINE_COURSES_START, OnlineCourseIds.Length);
            _ = MessageBox.Show("Online track selection patch applied.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateOnlineCoursesTab();
        }
        private bool IsOnlinePatchApplied()
        {
            return _data != null && _data.Length > ONLINE_PATCH_CHECK_OFFSET2
            && _data[ONLINE_PATCH_CHECK_OFFSET1] == ONLINE_PATCH_BYTE1
            && _data[ONLINE_PATCH_CHECK_OFFSET2] == ONLINE_PATCH_BYTE2;
        }
        #endregion
        #region UI Helpers
        private static void ClearPanel(TableLayoutPanel? panel)
        {
            if (panel == null)
            {
                return;
            }

            panel.SuspendLayout();
            panel.Controls.Clear();
            panel.RowStyles.Clear();
            panel.ColumnStyles.Clear();
            panel.RowCount = 0;
            _ = panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 372F));
            _ = panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panel.ResumeLayout(true);
        }
        private void AddTableHeader(TableLayoutPanel? panel, string left, string right)
        {
            if (panel?.Parent is not Panel wrapper || wrapper.Parent is not TabPage tab)
            {
                return;
            }

            RemoveHeader(panel);
            var header = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 1,
                Height = 30,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            _ = header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 372F));
            _ = header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            header.Controls.Add(CreateHeaderLabel(left), 0, 0);
            header.Controls.Add(CreateHeaderLabel(right), 1, 0);
            tab.Controls.Clear();
            tab.Controls.Add(wrapper);
            tab.Controls.Add(header);
            panel.Tag = header;
        }
        private static Label CreateHeaderLabel(string text)
        {
            return new()
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };
        }

        private static void RemoveHeader(TableLayoutPanel? panel)
        {
            if (panel?.Tag is TableLayoutPanel header && header.Parent is TabPage tab)
            {
                tab.Controls.Remove(header);
                panel.Tag = null;
            }
        }
        private static void AddTableRow(TableLayoutPanel? panel, string label, Control control)
        {
            if (panel == null)
            {
                return;
            }

            int row = panel.RowCount++;
            _ = panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            var lbl = new Label
            {
                Text = label,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 0, 0),
                Margin = new Padding(0, 2, 0, 2),
                AutoSize = false,
                Height = 28
            };
            control.Margin = new Padding(5, 4, 5, 4);
            panel.Controls.Add(lbl, 0, row);
            panel.Controls.Add(control, 1, row);
        }
        private static TableLayoutPanel CreateTablePanel()
        {
            return new()
            {
                ColumnCount = 2,
                Dock = DockStyle.Top,
                Margin = Padding.Empty,
                RowCount = 0,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(8, 8, 12, 8)
            };
        }
        #endregion
        #region Utility Methods
        private static void ShowError(string message)
        {
            _ = MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void ShowInfo()
        {
            Version? version = Assembly.GetExecutingAssembly().GetName().Version;
            string ver = version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "Unknown";
            _ = MessageBox.Show(
                $"Mario Kart DS ARM9 Editor\nVersion: {ver}\n\n" +
                "Edit values in Mario Kart DS ARM9 files.\n\n" +
                "Code: Landon & Emma\nSpecial Thanks: Ermelber, Yami, MkDasher",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private static void OpenRepository()
        {
            try
            {
                _ = Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/LandonAndEmma/MKDS-ARM9-Editor",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ShowError($"Cannot open URL: {ex.Message}");
            }
        }
        #endregion
    }
}