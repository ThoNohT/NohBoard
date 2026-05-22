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
    using System.Drawing;

    partial class LoadKeyboardForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblCategory = new System.Windows.Forms.Label();
            lblKeyboardDefinition = new System.Windows.Forms.Label();
            CategoryCombo = new System.Windows.Forms.ComboBox();
            DefinitionsList = new System.Windows.Forms.ListBox();
            DefinitionListMenu = new System.Windows.Forms.ContextMenuStrip(components);
            mnuDeleteDefinition = new System.Windows.Forms.ToolStripMenuItem();
            lblKeyboardStyle = new System.Windows.Forms.Label();
            StyleList = new System.Windows.Forms.ListBox();
            LoadLegacyButton = new System.Windows.Forms.Button();
            CloseButton = new System.Windows.Forms.Button();
            lblMissingFonts = new System.Windows.Forms.Label();
            fontsGridPanel = new System.Windows.Forms.Panel();
            fontsGrid = new System.Windows.Forms.DataGridView();
            lblRestart = new System.Windows.Forms.Label();
            btnRestart = new System.Windows.Forms.Button();
            DefinitionListMenu.SuspendLayout();
            fontsGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fontsGrid).BeginInit();
            SuspendLayout();
            lblCategory.Location = new Point(10, 10);
            lblCategory.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(180, 23);
            lblCategory.TabIndex = 0;
            lblCategory.Text = "Category:";
            lblCategory.TextAlign = ContentAlignment.MiddleLeft;
            lblKeyboardDefinition.Location = new Point(10, 65);
            lblKeyboardDefinition.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblKeyboardDefinition.Name = "lblKeyboardDefinition";
            lblKeyboardDefinition.Size = new Size(180, 23);
            lblKeyboardDefinition.TabIndex = 1;
            lblKeyboardDefinition.Text = "Keyboard Definition:";
            lblKeyboardDefinition.TextAlign = ContentAlignment.MiddleLeft;
            CategoryCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CategoryCombo.FormattingEnabled = true;
            CategoryCombo.Location = new Point(10, 35);
            CategoryCombo.Margin = new System.Windows.Forms.Padding(5);
            CategoryCombo.Name = "CategoryCombo";
            CategoryCombo.Size = new Size(180, 23);
            CategoryCombo.TabIndex = 2;
            CategoryCombo.SelectedIndexChanged += CategoryCombo_SelectedIndexChanged;
            DefinitionsList.ContextMenuStrip = DefinitionListMenu;
            DefinitionsList.FormattingEnabled = true;
            DefinitionsList.IntegralHeight = false;
            DefinitionsList.ItemHeight = 15;
            DefinitionsList.Location = new Point(10, 90);
            DefinitionsList.Margin = new System.Windows.Forms.Padding(5);
            DefinitionsList.Name = "DefinitionsList";
            DefinitionsList.Size = new Size(180, 200);
            DefinitionsList.TabIndex = 3;
            DefinitionsList.SelectedIndexChanged += DefinitionsList_SelectedIndexChanged;
            DefinitionListMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuDeleteDefinition });
            DefinitionListMenu.Name = "DefinitionListMenu";
            DefinitionListMenu.Size = new Size(112, 26);
            mnuDeleteDefinition.Name = "mnuDeleteDefinition";
            mnuDeleteDefinition.Size = new Size(111, 22);
            mnuDeleteDefinition.Text = "Delete";
            mnuDeleteDefinition.Click += mnuDeleteDefinition_Click;
            LoadLegacyButton.Location = new Point(195, 35);
            LoadLegacyButton.Margin = new System.Windows.Forms.Padding(5);
            LoadLegacyButton.Name = "LoadLegacyButton";
            LoadLegacyButton.Size = new Size(180, 23);
            LoadLegacyButton.TabIndex = 8;
            LoadLegacyButton.Text = "Load Legacy kb file...";
            LoadLegacyButton.UseVisualStyleBackColor = true;
            LoadLegacyButton.Click += LoadLegacyButton_Click;
            lblKeyboardStyle.Location = new Point(195, 65);
            lblKeyboardStyle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblKeyboardStyle.Name = "lblKeyboardStyle";
            lblKeyboardStyle.Size = new Size(180, 23);
            lblKeyboardStyle.TabIndex = 4;
            lblKeyboardStyle.Text = "Keyboard Style:";
            lblKeyboardStyle.TextAlign = ContentAlignment.MiddleLeft;
            StyleList.FormattingEnabled = true;
            StyleList.IntegralHeight = false;
            StyleList.ItemHeight = 15;
            StyleList.Location = new Point(195, 90);
            StyleList.Margin = new System.Windows.Forms.Padding(5);
            StyleList.Name = "StyleList";
            StyleList.Size = new Size(180, 200);
            StyleList.TabIndex = 5;
            StyleList.SelectedIndexChanged += StyleList_SelectedIndexChanged;
            CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CloseButton.Location = new Point(295, 305);
            CloseButton.Margin = new System.Windows.Forms.Padding(5);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(80, 23);
            CloseButton.TabIndex = 9;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            lblMissingFonts.Location = new Point(380, 10);
            lblMissingFonts.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblMissingFonts.Name = "lblMissingFonts";
            lblMissingFonts.Size = new Size(600, 35);
            lblMissingFonts.TabIndex = 10;
            lblMissingFonts.Text = "The following fonts are defined in the chosen style but not present on this system.\r\nIf a link is provided, you may download it by double clicking the link:";
            lblMissingFonts.TextAlign = ContentAlignment.MiddleLeft;
            fontsGridPanel.BackColor = SystemColors.Window;
            fontsGridPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            fontsGridPanel.Controls.Add(fontsGrid);
            fontsGridPanel.Location = new Point(380, 45);
            fontsGridPanel.Margin = new System.Windows.Forms.Padding(0);
            fontsGridPanel.Name = "fontsGridPanel";
            fontsGridPanel.Padding = new System.Windows.Forms.Padding(5);
            fontsGridPanel.Size = new Size(600, 245);
            fontsGridPanel.TabIndex = 14;
            fontsGrid.AllowUserToAddRows = false;
            fontsGrid.AllowUserToDeleteRows = false;
            fontsGrid.AllowUserToResizeRows = false;
            fontsGrid.BackgroundColor = SystemColors.Window;
            fontsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            fontsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            fontsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            fontsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            fontsGrid.Enabled = false;
            fontsGrid.Location = new Point(5, 5);
            fontsGrid.Margin = new System.Windows.Forms.Padding(0);
            fontsGrid.Name = "fontsGrid";
            fontsGrid.RowHeadersVisible = false;
            fontsGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            fontsGrid.ShowCellErrors = false;
            fontsGrid.ShowCellToolTips = false;
            fontsGrid.ShowEditingIcon = false;
            fontsGrid.ShowRowErrors = false;
            fontsGrid.Size = new Size(588, 233);
            fontsGrid.TabIndex = 11;
            fontsGrid.CellDoubleClick += fontsGrid_CellDoubleClick;
            lblRestart.Location = new Point(380, 305);
            lblRestart.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblRestart.Name = "lblRestart";
            lblRestart.Size = new Size(600, 23);
            lblRestart.TabIndex = 12;
            lblRestart.Text = "After a new font has been installed, NohBoard needs to be restarted to recognize it.";
            lblRestart.TextAlign = ContentAlignment.MiddleLeft;
            btnRestart.Enabled = false;
            btnRestart.Location = new Point(900, 305);
            btnRestart.Margin = new System.Windows.Forms.Padding(5);
            btnRestart.Name = "btnRestart";
            btnRestart.Size = new Size(80, 23);
            btnRestart.TabIndex = 13;
            btnRestart.Text = "Restart";
            btnRestart.UseVisualStyleBackColor = true;
            btnRestart.Click += btnRestart_Click;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(990, 335);
            Controls.Add(btnRestart);
            Controls.Add(lblRestart);
            Controls.Add(fontsGridPanel);
            Controls.Add(lblMissingFonts);
            Controls.Add(CloseButton);
            Controls.Add(LoadLegacyButton);
            Controls.Add(StyleList);
            Controls.Add(lblKeyboardStyle);
            Controls.Add(DefinitionsList);
            Controls.Add(CategoryCombo);
            Controls.Add(lblKeyboardDefinition);
            Controls.Add(lblCategory);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(5);
            Name = "LoadKeyboardForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Load Keyboard";
            Load += LoadKeyboardForm_Load;
            DefinitionListMenu.ResumeLayout(false);
            fontsGridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)fontsGrid).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblKeyboardDefinition;
        private System.Windows.Forms.ComboBox CategoryCombo;
        private System.Windows.Forms.ListBox DefinitionsList;
        private System.Windows.Forms.Label lblKeyboardStyle;
        private System.Windows.Forms.ListBox StyleList;
        private System.Windows.Forms.Button LoadLegacyButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ContextMenuStrip DefinitionListMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteDefinition;
        private System.Windows.Forms.Label lblMissingFonts;
        private System.Windows.Forms.Panel fontsGridPanel;
        private System.Windows.Forms.DataGridView fontsGrid;
        private System.Windows.Forms.Label lblRestart;
        private System.Windows.Forms.Button btnRestart;
    }
}
