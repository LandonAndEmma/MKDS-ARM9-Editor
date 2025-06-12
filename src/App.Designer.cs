using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
namespace ARM9Editor
{
    partial class App
    {
        private IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(App));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            patchesToolStripMenuItem = new ToolStripMenuItem();
            editableOnlineTrackSelectionToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            infoToolStripMenuItem = new ToolStripMenuItem();
            repositoryToolStripMenuItem = new ToolStripMenuItem();
            checkForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            tabPage6 = new TabPage();
            characterlistBox = new ListBox();
            tabPage5 = new TabPage();
            kartlistBox = new ListBox();
            tabPage4 = new TabPage();
            emblemlistBox = new ListBox();
            tabPage3 = new TabPage();
            weatherlistBox = new ListBox();
            tabPage2 = new TabPage();
            courselistBox = new ListBox();
            tabPage1 = new TabPage();
            musiclistBox = new ListBox();
            tabControl1 = new TabControl();
            menuStrip1.SuspendLayout();
            tabPage6.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, patchesToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(0);
            menuStrip1.Size = new Size(933, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(114, 22);
            openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(114, 22);
            saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(114, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { infoToolStripMenuItem, repositoryToolStripMenuItem, checkForUpdatesToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // infoToolStripMenuItem
            // 
            infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            infoToolStripMenuItem.Size = new Size(180, 22);
            infoToolStripMenuItem.Text = "Info";
            // 
            // repositoryToolStripMenuItem
            // 
            repositoryToolStripMenuItem.Name = "repositoryToolStripMenuItem";
            repositoryToolStripMenuItem.Size = new Size(180, 22);
            repositoryToolStripMenuItem.Text = "Repository";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            checkForUpdatesToolStripMenuItem.Size = new Size(180, 22);
            checkForUpdatesToolStripMenuItem.Text = "Check For Updates";
            // 
            // patchesToolStripMenuItem
            // 
            patchesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { editableOnlineTrackSelectionToolStripMenuItem });
            patchesToolStripMenuItem.Name = "patchesToolStripMenuItem";
            patchesToolStripMenuItem.Size = new Size(60, 24);
            patchesToolStripMenuItem.Text = "Patches";
            // 
            // editableOnlineTrackSelectionToolStripMenuItem
            // 
            editableOnlineTrackSelectionToolStripMenuItem.Name = "editableOnlineTrackSelectionToolStripMenuItem";
            editableOnlineTrackSelectionToolStripMenuItem.Size = new Size(236, 22);
            editableOnlineTrackSelectionToolStripMenuItem.Text = "Editable Online Track Selection";
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(characterlistBox);
            tabPage6.Location = new Point(4, 24);
            tabPage6.Name = "tabPage6";
            tabPage6.Size = new Size(925, 467);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Character Filenames";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // characterlistBox
            // 
            characterlistBox.Dock = DockStyle.Fill;
            characterlistBox.FormattingEnabled = true;
            characterlistBox.Location = new Point(0, 0);
            characterlistBox.Margin = new Padding(0);
            characterlistBox.Name = "characterlistBox";
            characterlistBox.Size = new Size(925, 467);
            characterlistBox.TabIndex = 0;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(kartlistBox);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Margin = new Padding(0);
            tabPage5.Name = "tabPage5";
            tabPage5.Size = new Size(925, 467);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Kart Filenames";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // kartlistBox
            // 
            kartlistBox.Dock = DockStyle.Fill;
            kartlistBox.FormattingEnabled = true;
            kartlistBox.Location = new Point(0, 0);
            kartlistBox.Margin = new Padding(0);
            kartlistBox.Name = "kartlistBox";
            kartlistBox.Size = new Size(925, 467);
            kartlistBox.TabIndex = 0;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(emblemlistBox);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Margin = new Padding(0);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(925, 467);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Emblem Filenames";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // emblemlistBox
            // 
            emblemlistBox.Dock = DockStyle.Fill;
            emblemlistBox.FormattingEnabled = true;
            emblemlistBox.Location = new Point(0, 0);
            emblemlistBox.Margin = new Padding(0);
            emblemlistBox.Name = "emblemlistBox";
            emblemlistBox.Size = new Size(925, 467);
            emblemlistBox.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(weatherlistBox);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Margin = new Padding(0);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(925, 467);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Weather Slots";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // weatherlistBox
            // 
            weatherlistBox.Dock = DockStyle.Fill;
            weatherlistBox.FormattingEnabled = true;
            weatherlistBox.Location = new Point(0, 0);
            weatherlistBox.Margin = new Padding(0);
            weatherlistBox.Name = "weatherlistBox";
            weatherlistBox.Size = new Size(925, 467);
            weatherlistBox.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(courselistBox);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(0);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(925, 467);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Course Filenames";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // courselistBox
            // 
            courselistBox.Dock = DockStyle.Fill;
            courselistBox.FormattingEnabled = true;
            courselistBox.Location = new Point(0, 0);
            courselistBox.Margin = new Padding(0);
            courselistBox.Name = "courselistBox";
            courselistBox.Size = new Size(925, 467);
            courselistBox.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(musiclistBox);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(0);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(925, 467);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Music IDs";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // musiclistBox
            // 
            musiclistBox.Dock = DockStyle.Fill;
            musiclistBox.FormattingEnabled = true;
            musiclistBox.Location = new Point(0, 0);
            musiclistBox.Margin = new Padding(0);
            musiclistBox.Name = "musiclistBox";
            musiclistBox.Size = new Size(925, 467);
            musiclistBox.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Location = new Point(0, 24);
            tabControl1.Margin = new Padding(0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(933, 495);
            tabControl1.TabIndex = 0;
            // 
            // App
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 519);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(0);
            Name = "App";
            Text = "Mario Kart DS ARM9 Editor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabPage6.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem infoToolStripMenuItem;
        private ToolStripMenuItem repositoryToolStripMenuItem;
        private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private ToolStripMenuItem patchesToolStripMenuItem;
        private ToolStripMenuItem editableOnlineTrackSelectionToolStripMenuItem;
        private TabPage tabPage6;
        private ListBox characterlistBox;
        private TabPage tabPage5;
        private ListBox kartlistBox;
        private TabPage tabPage4;
        private ListBox emblemlistBox;
        private TabPage tabPage3;
        private ListBox weatherlistBox;
        private TabPage tabPage2;
        private ListBox courselistBox;
        private TabPage tabPage1;
        private ListBox musiclistBox;
        private TabControl tabControl1;
    }
}