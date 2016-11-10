/*
Copyright (C) 2016 by Eric Bataille <e.c.p.bataille@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace ThoNohT.NohBoard.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.MainMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKeyboards = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuToggleEditMode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditKeyboardStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditElementStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSaveDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveDefinitionAsName = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveDefinitionAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveStyleToName = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveToGlobalStyleName = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveStyleAs = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.KeyCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Interval = 33;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSettings,
            this.mnuKeyboards,
            this.MainMenuSep1,
            this.mnuToggleEditMode,
            this.mnuEditKeyboardStyle,
            this.mnuEditElementStyle,
            this.MainMenuSep2,
            this.mnuSaveDefinition,
            this.mnuSaveStyle,
            this.MainMenuSep3,
            this.mnuExit});
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(176, 220);
            this.MainMenu.Opening += new System.ComponentModel.CancelEventHandler(this.MainMenu_Opening);
            // 
            // mnuSettings
            // 
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(175, 22);
            this.mnuSettings.Text = "&Settings";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // mnuKeyboards
            // 
            this.mnuKeyboards.Name = "mnuKeyboards";
            this.mnuKeyboards.Size = new System.Drawing.Size(175, 22);
            this.mnuKeyboards.Text = "&Load Keyboard";
            this.mnuKeyboards.Click += new System.EventHandler(this.mnuLoadKeyboard_Click);
            // 
            // MainMenuSep1
            // 
            this.MainMenuSep1.Name = "MainMenuSep1";
            this.MainMenuSep1.Size = new System.Drawing.Size(172, 6);
            // 
            // mnuToggleEditMode
            // 
            this.mnuToggleEditMode.Name = "mnuToggleEditMode";
            this.mnuToggleEditMode.Size = new System.Drawing.Size(175, 22);
            this.mnuToggleEditMode.Text = "Start &Editing";
            this.mnuToggleEditMode.Click += new System.EventHandler(this.mnuToggleEditMode_Click);
            // 
            // mnuEditKeyboardStyle
            // 
            this.mnuEditKeyboardStyle.Name = "mnuEditKeyboardStyle";
            this.mnuEditKeyboardStyle.Size = new System.Drawing.Size(175, 22);
            this.mnuEditKeyboardStyle.Text = "Edit Keyboard Style";
            this.mnuEditKeyboardStyle.Click += new System.EventHandler(this.mnuEditKeyboardStyle_Click);
            // 
            // mnuEditElementStyle
            // 
            this.mnuEditElementStyle.Enabled = false;
            this.mnuEditElementStyle.Name = "mnuEditElementStyle";
            this.mnuEditElementStyle.Size = new System.Drawing.Size(175, 22);
            this.mnuEditElementStyle.Text = "Edit Element Style";
            this.mnuEditElementStyle.Click += new System.EventHandler(this.mnuEditElementStyle_Click);
            // 
            // MainMenuSep2
            // 
            this.MainMenuSep2.Name = "MainMenuSep2";
            this.MainMenuSep2.Size = new System.Drawing.Size(172, 6);
            // 
            // mnuSaveDefinition
            // 
            this.mnuSaveDefinition.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveDefinitionAsName,
            this.mnuSaveDefinitionAs});
            this.mnuSaveDefinition.Name = "mnuSaveDefinition";
            this.mnuSaveDefinition.Size = new System.Drawing.Size(175, 22);
            this.mnuSaveDefinition.Text = "Save &Definition";
            // 
            // mnuSaveDefinitionAsName
            // 
            this.mnuSaveDefinitionAsName.Name = "mnuSaveDefinitionAsName";
            this.mnuSaveDefinitionAsName.Size = new System.Drawing.Size(217, 22);
            this.mnuSaveDefinitionAsName.Text = "&Save &To <DefinitionName>";
            this.mnuSaveDefinitionAsName.Click += new System.EventHandler(this.mnuSaveDefinitionAsName_Click);
            // 
            // mnuSaveDefinitionAs
            // 
            this.mnuSaveDefinitionAs.Name = "mnuSaveDefinitionAs";
            this.mnuSaveDefinitionAs.Size = new System.Drawing.Size(217, 22);
            this.mnuSaveDefinitionAs.Text = "Save &As";
            this.mnuSaveDefinitionAs.Click += new System.EventHandler(this.mnuSaveDefinitionAs_Click);
            // 
            // mnuSaveStyle
            // 
            this.mnuSaveStyle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveStyleToName,
            this.mnuSaveToGlobalStyleName,
            this.mnuSaveStyleAs});
            this.mnuSaveStyle.Name = "mnuSaveStyle";
            this.mnuSaveStyle.Size = new System.Drawing.Size(175, 22);
            this.mnuSaveStyle.Text = "Save St&yle";
            // 
            // mnuSaveStyleToName
            // 
            this.mnuSaveStyleToName.Name = "mnuSaveStyleToName";
            this.mnuSaveStyleToName.Size = new System.Drawing.Size(227, 22);
            this.mnuSaveStyleToName.Text = "&Save To <StyleName>";
            this.mnuSaveStyleToName.Click += new System.EventHandler(this.mnuSaveStyleToName_Click);
            // 
            // mnuSaveToGlobalStyleName
            // 
            this.mnuSaveToGlobalStyleName.Name = "mnuSaveToGlobalStyleName";
            this.mnuSaveToGlobalStyleName.Size = new System.Drawing.Size(227, 22);
            this.mnuSaveToGlobalStyleName.Text = "Save To &Global <StyleName>";
            this.mnuSaveToGlobalStyleName.Click += new System.EventHandler(this.mnuSaveToGlobalStyleName_Click);
            // 
            // mnuSaveStyleAs
            // 
            this.mnuSaveStyleAs.Name = "mnuSaveStyleAs";
            this.mnuSaveStyleAs.Size = new System.Drawing.Size(227, 22);
            this.mnuSaveStyleAs.Text = "Save &As";
            this.mnuSaveStyleAs.Click += new System.EventHandler(this.mnuSaveStyleAs_Click);
            // 
            // MainMenuSep3
            // 
            this.MainMenuSep3.Name = "MainMenuSep3";
            this.MainMenuSep3.Size = new System.Drawing.Size(172, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(175, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // KeyCheckTimer
            // 
            this.KeyCheckTimer.Interval = 1000;
            this.KeyCheckTimer.Tick += new System.EventHandler(this.KeyCheckTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ContextMenuStrip = this.MainMenu;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "NohBoard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.MainMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.ContextMenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuKeyboards;
        private System.Windows.Forms.ToolStripMenuItem mnuSettings;
        private System.Windows.Forms.ToolStripSeparator MainMenuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuToggleEditMode;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveStyle;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveStyleToName;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveStyleAs;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveDefinition;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveDefinitionAsName;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveDefinitionAs;
        private System.Windows.Forms.ToolStripSeparator MainMenuSep2;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuEditKeyboardStyle;
        private System.Windows.Forms.ToolStripMenuItem mnuEditElementStyle;
        private System.Windows.Forms.ToolStripSeparator MainMenuSep3;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveToGlobalStyleName;
        private System.Windows.Forms.Timer KeyCheckTimer;
    }
}

