using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ARM9Editor
{
    partial class App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(App));
            tabPage2 = new TabPage();
            courselistBox = new ListBox();
            tabPage1 = new TabPage();
            musiclistBox = new ListBox();
            tabControl1 = new TabControl();
            tabPage3 = new TabPage();
            weatherlistBox = new ListBox();
            tabPage4 = new TabPage();
            emblemlistBox = new ListBox();
            tabPage5 = new TabPage();
            kartlistBox = new ListBox();
            tabPage6 = new TabPage();
            characterlistBox = new ListBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            infoToolStripMenuItem = new ToolStripMenuItem();
            repositoryToolStripMenuItem = new ToolStripMenuItem();
            tabPage2.SuspendLayout();
            tabPage1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage6.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(courselistBox);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4, 3, 4, 3);
            tabPage2.Size = new Size(925, 467);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Course Filenames";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // courselistBox
            // 
            courselistBox.Dock = DockStyle.Fill;
            courselistBox.FormattingEnabled = true;
            courselistBox.ItemHeight = 15;
            courselistBox.Location = new Point(4, 3);
            courselistBox.Margin = new Padding(4, 3, 4, 3);
            courselistBox.Name = "courselistBox";
            courselistBox.Size = new Size(917, 461);
            courselistBox.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(musiclistBox);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new Size(925, 467);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Music IDs";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // musiclistBox
            // 
            musiclistBox.Dock = DockStyle.Fill;
            musiclistBox.FormattingEnabled = true;
            musiclistBox.ItemHeight = 15;
            musiclistBox.Location = new Point(4, 3);
            musiclistBox.Margin = new Padding(4, 3, 4, 3);
            musiclistBox.Name = "musiclistBox";
            musiclistBox.Size = new Size(917, 461);
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
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(933, 495);
            tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(weatherlistBox);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Margin = new Padding(4, 3, 4, 3);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(4, 3, 4, 3);
            tabPage3.Size = new Size(925, 467);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Weather Slots";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // weatherlistBox
            // 
            weatherlistBox.Dock = DockStyle.Fill;
            weatherlistBox.FormattingEnabled = true;
            weatherlistBox.ItemHeight = 15;
            weatherlistBox.Location = new Point(4, 3);
            weatherlistBox.Margin = new Padding(4, 3, 4, 3);
            weatherlistBox.Name = "weatherlistBox";
            weatherlistBox.Size = new Size(917, 461);
            weatherlistBox.TabIndex = 0;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(emblemlistBox);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Margin = new Padding(4, 3, 4, 3);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(4, 3, 4, 3);
            tabPage4.Size = new Size(925, 467);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Emblem Filenames";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // emblemlistBox
            // 
            emblemlistBox.Dock = DockStyle.Fill;
            emblemlistBox.FormattingEnabled = true;
            emblemlistBox.ItemHeight = 15;
            emblemlistBox.Location = new Point(4, 3);
            emblemlistBox.Margin = new Padding(4, 3, 4, 3);
            emblemlistBox.Name = "emblemlistBox";
            emblemlistBox.Size = new Size(917, 461);
            emblemlistBox.TabIndex = 0;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(kartlistBox);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Margin = new Padding(3, 2, 3, 2);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(4, 3, 4, 3);
            tabPage5.Size = new Size(925, 467);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Kart Filenames";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // kartlistBox
            // 
            kartlistBox.Dock = DockStyle.Fill;
            kartlistBox.FormattingEnabled = true;
            kartlistBox.ItemHeight = 15;
            kartlistBox.Location = new Point(3, 2);
            kartlistBox.Margin = new Padding(4, 3, 4, 3);
            kartlistBox.Name = "kartlistBox";
            kartlistBox.Size = new Size(919, 463);
            kartlistBox.TabIndex = 0;
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(characterlistBox);
            tabPage6.Location = new Point(4, 24);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(4, 3, 4, 3);
            tabPage6.Size = new Size(925, 467);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Character Filenames";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // characterlistBox
            // 
            characterlistBox.Dock = DockStyle.Fill;
            characterlistBox.FormattingEnabled = true;
            characterlistBox.ItemHeight = 15;
            characterlistBox.Location = new Point(3, 3);
            characterlistBox.Margin = new Padding(4, 3, 4, 3);
            characterlistBox.Name = "characterlistBox";
            characterlistBox.Size = new Size(919, 461);
            characterlistBox.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(933, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
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
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { infoToolStripMenuItem, repositoryToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // infoToolStripMenuItem
            // 
            infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            infoToolStripMenuItem.Size = new Size(130, 22);
            infoToolStripMenuItem.Text = "Info";
            // 
            // repositoryToolStripMenuItem
            // 
            repositoryToolStripMenuItem.Name = "repositoryToolStripMenuItem";
            repositoryToolStripMenuItem.Size = new Size(130, 22);
            repositoryToolStripMenuItem.Text = "Repository";
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
            Margin = new Padding(4, 3, 4, 3);
            Name = "App";
            Text = "Mario Kart DS ARM9 Editor";
            tabPage2.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            tabPage6.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem infoToolStripMenuItem;
        private ToolStripMenuItem repositoryToolStripMenuItem;
        private ListBox musiclistBox;
        private ListBox courselistBox;
        private ListBox weatherlistBox;
        private ListBox characterlistBox;
        private ListBox emblemlistBox;
        private ListBox kartlistBox;
    }
}