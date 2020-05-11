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
            this.MainMenuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuToggleEditMode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdateTextPosition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMoveElement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMoveToTop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMoveToBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddElement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddKeyboardKeyDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddMouseKeyDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddMouseScrollDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddMouseSpeedIndicatorDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveElement = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddBoundaryPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveBoundaryPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuKeyboardProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuElementProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditKeyboardStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditElementStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSaveDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveDefinitionAsName = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveDefinitionAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveStyleToName = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveToGlobalStyleName = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveStyleAs = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGenerateLog = new System.Windows.Forms.ToolStripMenuItem();
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
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSettings,
            this.mnuKeyboards,
            this.MainMenuSep2,
            this.mnuToggleEditMode,
            this.mnuUpdateTextPosition,
            this.mnuMoveElement,
            this.mnuAddElement,
            this.mnuRemoveElement,
            this.mnuAddBoundaryPoint,
            this.mnuRemoveBoundaryPoint,
            this.MainMenuSep1,
            this.mnuKeyboardProperties,
            this.mnuElementProperties,
            this.mnuEditKeyboardStyle,
            this.mnuEditElementStyle,
            this.MainMenuSep4,
            this.mnuSaveDefinition,
            this.mnuSaveStyle,
            this.MainMenuSep3,
            this.mnuExit,
            this.mnuUpdate,
            this.mnuGenerateLog});
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(203, 424);
            this.MainMenu.Opening += new System.ComponentModel.CancelEventHandler(this.MainMenu_Opening);
            // 
            // mnuSettings
            // 
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(202, 22);
            this.mnuSettings.Text = "&Settings";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // mnuKeyboards
            // 
            this.mnuKeyboards.Name = "mnuKeyboards";
            this.mnuKeyboards.Size = new System.Drawing.Size(202, 22);
            this.mnuKeyboards.Text = "&Load Keyboard";
            this.mnuKeyboards.Click += new System.EventHandler(this.mnuLoadKeyboard_Click);
            // 
            // MainMenuSep2
            // 
            this.MainMenuSep2.Name = "MainMenuSep2";
            this.MainMenuSep2.Size = new System.Drawing.Size(199, 6);
            // 
            // mnuToggleEditMode
            // 
            this.mnuToggleEditMode.CheckOnClick = true;
            this.mnuToggleEditMode.Name = "mnuToggleEditMode";
            this.mnuToggleEditMode.Size = new System.Drawing.Size(202, 22);
            this.mnuToggleEditMode.Text = "Start &Editing";
            this.mnuToggleEditMode.Click += new System.EventHandler(this.mnuToggleEditMode_Click);
            // 
            // mnuUpdateTextPosition
            // 
            this.mnuUpdateTextPosition.Checked = true;
            this.mnuUpdateTextPosition.CheckOnClick = true;
            this.mnuUpdateTextPosition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuUpdateTextPosition.Name = "mnuUpdateTextPosition";
            this.mnuUpdateTextPosition.Size = new System.Drawing.Size(202, 22);
            this.mnuUpdateTextPosition.Text = "&Update Text Position";
            this.mnuUpdateTextPosition.Click += new System.EventHandler(this.mnuUpdateTextPosition_Click);
            // 
            // mnuMoveElement
            // 
            this.mnuMoveElement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMoveToTop,
            this.mnuMoveUp,
            this.mnuMoveDown,
            this.mnuMoveToBottom});
            this.mnuMoveElement.Name = "mnuMoveElement";
            this.mnuMoveElement.Size = new System.Drawing.Size(202, 22);
            this.mnuMoveElement.Text = "&Move";
            // 
            // mnuMoveToTop
            // 
            this.mnuMoveToTop.Name = "mnuMoveToTop";
            this.mnuMoveToTop.Size = new System.Drawing.Size(161, 22);
            this.mnuMoveToTop.Text = "Move to &top";
            this.mnuMoveToTop.Click += new System.EventHandler(this.mnuMoveToTop_Click);
            // 
            // mnuMoveUp
            // 
            this.mnuMoveUp.Name = "mnuMoveUp";
            this.mnuMoveUp.Size = new System.Drawing.Size(161, 22);
            this.mnuMoveUp.Text = "Move &up";
            this.mnuMoveUp.Click += new System.EventHandler(this.mnuMoveUp_Click);
            // 
            // mnuMoveDown
            // 
            this.mnuMoveDown.Name = "mnuMoveDown";
            this.mnuMoveDown.Size = new System.Drawing.Size(161, 22);
            this.mnuMoveDown.Text = "Move &down";
            this.mnuMoveDown.Click += new System.EventHandler(this.mnuMoveDown_Click);
            // 
            // mnuMoveToBottom
            // 
            this.mnuMoveToBottom.Name = "mnuMoveToBottom";
            this.mnuMoveToBottom.Size = new System.Drawing.Size(161, 22);
            this.mnuMoveToBottom.Text = "Move to &bottom";
            this.mnuMoveToBottom.Click += new System.EventHandler(this.mnuMoveToBottom_Click);
            // 
            // mnuAddElement
            // 
            this.mnuAddElement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddKeyboardKeyDefinition,
            this.mnuAddMouseKeyDefinition,
            this.mnuAddMouseScrollDefinition,
            this.mnuAddMouseSpeedIndicatorDefinition});
            this.mnuAddElement.Name = "mnuAddElement";
            this.mnuAddElement.Size = new System.Drawing.Size(202, 22);
            this.mnuAddElement.Text = "&Add Element";
            this.mnuAddElement.Visible = false;
            // 
            // mnuAddKeyboardKeyDefinition
            // 
            this.mnuAddKeyboardKeyDefinition.Name = "mnuAddKeyboardKeyDefinition";
            this.mnuAddKeyboardKeyDefinition.Size = new System.Drawing.Size(220, 22);
            this.mnuAddKeyboardKeyDefinition.Text = "Add &Keyboard Key";
            this.mnuAddKeyboardKeyDefinition.Click += new System.EventHandler(this.mnuAddKeyboardKeyDefinition_Click);
            // 
            // mnuAddMouseKeyDefinition
            // 
            this.mnuAddMouseKeyDefinition.Name = "mnuAddMouseKeyDefinition";
            this.mnuAddMouseKeyDefinition.Size = new System.Drawing.Size(220, 22);
            this.mnuAddMouseKeyDefinition.Text = "Add &Mouse Key";
            this.mnuAddMouseKeyDefinition.Click += new System.EventHandler(this.mnuAddMouseKeyDefinition_Click);
            // 
            // mnuAddMouseScrollDefinition
            // 
            this.mnuAddMouseScrollDefinition.Name = "mnuAddMouseScrollDefinition";
            this.mnuAddMouseScrollDefinition.Size = new System.Drawing.Size(220, 22);
            this.mnuAddMouseScrollDefinition.Text = "Add Mouse &Scroll";
            this.mnuAddMouseScrollDefinition.Click += new System.EventHandler(this.mnuAddMouseScrollDefinition_Click);
            // 
            // mnuAddMouseSpeedIndicatorDefinition
            // 
            this.mnuAddMouseSpeedIndicatorDefinition.Name = "mnuAddMouseSpeedIndicatorDefinition";
            this.mnuAddMouseSpeedIndicatorDefinition.Size = new System.Drawing.Size(220, 22);
            this.mnuAddMouseSpeedIndicatorDefinition.Text = "Add Mouse Speed &Indicator";
            this.mnuAddMouseSpeedIndicatorDefinition.Click += new System.EventHandler(this.mnuAddMouseSpeedIndicatorDefinition_Click);
            // 
            // mnuRemoveElement
            // 
            this.mnuRemoveElement.Name = "mnuRemoveElement";
            this.mnuRemoveElement.Size = new System.Drawing.Size(202, 22);
            this.mnuRemoveElement.Text = "&Remove Element";
            this.mnuRemoveElement.Visible = false;
            this.mnuRemoveElement.Click += new System.EventHandler(this.mnuRemoveElement_Click);
            // 
            // mnuAddBoundaryPoint
            // 
            this.mnuAddBoundaryPoint.Name = "mnuAddBoundaryPoint";
            this.mnuAddBoundaryPoint.Size = new System.Drawing.Size(202, 22);
            this.mnuAddBoundaryPoint.Text = "Add Boundary Point";
            this.mnuAddBoundaryPoint.Visible = false;
            this.mnuAddBoundaryPoint.Click += new System.EventHandler(this.mnuAddBoundaryPoint_Click);
            // 
            // mnuRemoveBoundaryPoint
            // 
            this.mnuRemoveBoundaryPoint.Name = "mnuRemoveBoundaryPoint";
            this.mnuRemoveBoundaryPoint.Size = new System.Drawing.Size(202, 22);
            this.mnuRemoveBoundaryPoint.Text = "Remove Boundary Point";
            this.mnuRemoveBoundaryPoint.Visible = false;
            this.mnuRemoveBoundaryPoint.Click += new System.EventHandler(this.mnuRemoveBoundaryPoint_Click);
            // 
            // MainMenuSep1
            // 
            this.MainMenuSep1.Name = "MainMenuSep1";
            this.MainMenuSep1.Size = new System.Drawing.Size(199, 6);
            // 
            // mnuKeyboardProperties
            // 
            this.mnuKeyboardProperties.Name = "mnuKeyboardProperties";
            this.mnuKeyboardProperties.Size = new System.Drawing.Size(202, 22);
            this.mnuKeyboardProperties.Text = "Keyboard &Properties";
            this.mnuKeyboardProperties.Click += new System.EventHandler(this.mnuKeyboardProperties_Click);
            // 
            // mnuElementProperties
            // 
            this.mnuElementProperties.Enabled = false;
            this.mnuElementProperties.Name = "mnuElementProperties";
            this.mnuElementProperties.Size = new System.Drawing.Size(202, 22);
            this.mnuElementProperties.Text = "Element &Properties";
            this.mnuElementProperties.Click += new System.EventHandler(this.mnuElementProperties_Click);
            // 
            // mnuEditKeyboardStyle
            // 
            this.mnuEditKeyboardStyle.Name = "mnuEditKeyboardStyle";
            this.mnuEditKeyboardStyle.Size = new System.Drawing.Size(202, 22);
            this.mnuEditKeyboardStyle.Text = "Keyboard &Style";
            this.mnuEditKeyboardStyle.Click += new System.EventHandler(this.mnuEditKeyboardStyle_Click);
            // 
            // mnuEditElementStyle
            // 
            this.mnuEditElementStyle.Enabled = false;
            this.mnuEditElementStyle.Name = "mnuEditElementStyle";
            this.mnuEditElementStyle.Size = new System.Drawing.Size(202, 22);
            this.mnuEditElementStyle.Text = "Element &Style";
            this.mnuEditElementStyle.Visible = false;
            this.mnuEditElementStyle.Click += new System.EventHandler(this.mnuEditElementStyle_Click);
            // 
            // MainMenuSep4
            // 
            this.MainMenuSep4.Name = "MainMenuSep4";
            this.MainMenuSep4.Size = new System.Drawing.Size(199, 6);
            // 
            // mnuSaveDefinition
            // 
            this.mnuSaveDefinition.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveDefinitionAsName,
            this.mnuSaveDefinitionAs});
            this.mnuSaveDefinition.Name = "mnuSaveDefinition";
            this.mnuSaveDefinition.Size = new System.Drawing.Size(202, 22);
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
            this.mnuSaveStyle.Size = new System.Drawing.Size(202, 22);
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
            this.MainMenuSep3.Size = new System.Drawing.Size(199, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(202, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuUpdate
            // 
            this.mnuUpdate.Name = "mnuUpdate";
            this.mnuUpdate.Size = new System.Drawing.Size(202, 22);
            this.mnuUpdate.Text = "Update available";
            this.mnuUpdate.Visible = false;
            // 
            // mnuGenerateLog
            // 
            this.mnuGenerateLog.Name = "mnuGenerateLog";
            this.mnuGenerateLog.Size = new System.Drawing.Size(202, 22);
            this.mnuGenerateLog.Text = "Generate crash log";
            this.mnuGenerateLog.Click += new System.EventHandler(this.mnuGenerateLog_Click);
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
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.Move += new System.EventHandler(this.MainForm_Move);
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
        private System.Windows.Forms.ToolStripMenuItem mnuUpdate;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveElement;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveToTop;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveDown;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveToBottom;
        private System.Windows.Forms.ToolStripMenuItem mnuAddBoundaryPoint;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveBoundaryPoint;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveElement;
        private System.Windows.Forms.ToolStripMenuItem mnuAddElement;
        private System.Windows.Forms.ToolStripMenuItem mnuAddKeyboardKeyDefinition;
        private System.Windows.Forms.ToolStripMenuItem mnuAddMouseKeyDefinition;
        private System.Windows.Forms.ToolStripMenuItem mnuAddMouseScrollDefinition;
        private System.Windows.Forms.ToolStripMenuItem mnuAddMouseSpeedIndicatorDefinition;
        private System.Windows.Forms.ToolStripMenuItem mnuElementProperties;
        private System.Windows.Forms.ToolStripSeparator MainMenuSep4;
        private System.Windows.Forms.ToolStripMenuItem mnuKeyboardProperties;
        private System.Windows.Forms.ToolStripMenuItem mnuUpdateTextPosition;
        private System.Windows.Forms.ToolStripMenuItem mnuGenerateLog;
    }
}

