using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
namespace ARM9Editor
{
    partial class MainForm
    {
        private IContainer components;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            tabControl1 = new TabControl();
            openToolStripMenuItem = CreateMenuItem("Open");
            saveToolStripMenuItem = CreateMenuItem("Save");
            saveAsToolStripMenuItem = CreateMenuItem("Save As");
            var fileMenu = CreateMenuItem("File");
            fileMenu.DropDownItems.AddRange(new[]
            {
        openToolStripMenuItem,
        saveToolStripMenuItem,
        saveAsToolStripMenuItem
    });
            editableOnlineTrackSelectionToolStripMenuItem = CreateMenuItem("Editable Online Track Selection");
            var patchesMenu = CreateMenuItem("Patches");
            patchesMenu.DropDownItems.Add(editableOnlineTrackSelectionToolStripMenuItem);
            infoToolStripMenuItem = CreateMenuItem("Info");
            repositoryToolStripMenuItem = CreateMenuItem("Repository");
            var helpMenu = CreateMenuItem("Help");
            helpMenu.DropDownItems.AddRange(new[]
            {
        infoToolStripMenuItem,
        repositoryToolStripMenuItem
    });
            menuStrip1.Items.AddRange(new[] { fileMenu, patchesMenu, helpMenu });
            menuStrip1.Dock = DockStyle.Top;
            tabPageMusic = CreateTabPage("Music IDs", out musicTablePanel);
            tabPageCourses = CreateTabPage("Course Filenames", out courseTablePanel);
            tabPageWeather = CreateTabPage("Weather Slots", out weatherTablePanel);
            tabPageEmblems = CreateTabPage("Emblem Filenames", out emblemTablePanel);
            tabPageKarts = CreateTabPage("Kart Filenames", out kartTablePanel);
            tabPageCharacters = CreateTabPage("Character Filenames", out characterTablePanel);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Controls.AddRange(new Control[]
            {
        tabPageMusic, tabPageCourses, tabPageWeather,
        tabPageEmblems, tabPageKarts, tabPageCharacters
            });
            ClientSize = new Size(1000, 640);
            MinimumSize = new Size(820, 480);
            Text = "Mario Kart DS ARM9 Editor";
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            using var iconStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("ARM9Editor.Icon.ico");
            if (iconStream != null)
                Icon = new Icon(iconStream);
        }
        private static ToolStripMenuItem CreateMenuItem(string text) => new(text);
        private TabPage CreateTabPage(string title, out TableLayoutPanel panel)
        {
            panel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Top,
                Margin = Padding.Empty,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(8, 8, 12, 8)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 372F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            var wrapper = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            wrapper.Controls.Add(panel);
            var page = new TabPage(title) { Padding = new Padding(6) };
            page.Controls.Add(wrapper);
            panel.Tag = wrapper;
            return page;
        }
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem infoToolStripMenuItem;
        private ToolStripMenuItem repositoryToolStripMenuItem;
        private ToolStripMenuItem patchesToolStripMenuItem;
        private ToolStripMenuItem editableOnlineTrackSelectionToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabPageMusic;
        private TabPage tabPageCourses;
        private TabPage tabPageWeather;
        private TabPage tabPageEmblems;
        private TabPage tabPageKarts;
        private TabPage tabPageCharacters;
        private TableLayoutPanel musicTablePanel;
        private TableLayoutPanel courseTablePanel;
        private TableLayoutPanel weatherTablePanel;
        private TableLayoutPanel emblemTablePanel;
        private TableLayoutPanel kartTablePanel;
        private TableLayoutPanel characterTablePanel;
    }
}