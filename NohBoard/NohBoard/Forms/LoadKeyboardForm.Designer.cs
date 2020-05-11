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
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblKeyboardDefinition = new System.Windows.Forms.Label();
            this.CategoryCombo = new System.Windows.Forms.ComboBox();
            this.DefinitionsList = new System.Windows.Forms.ListBox();
            this.DefinitionListMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDeleteDefinition = new System.Windows.Forms.ToolStripMenuItem();
            this.lblKeyboardStyle = new System.Windows.Forms.Label();
            this.StyleList = new System.Windows.Forms.ListBox();
            this.LoadLegacyButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.lblMissingFonts = new System.Windows.Forms.Label();
            this.fontsGrid = new System.Windows.Forms.DataGridView();
            this.lblRestart = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.DefinitionListMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(14, 10);
            this.lblCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(58, 15);
            this.lblCategory.TabIndex = 0;
            this.lblCategory.Text = "Category:";
            // 
            // lblKeyboardDefinition
            // 
            this.lblKeyboardDefinition.AutoSize = true;
            this.lblKeyboardDefinition.Location = new System.Drawing.Point(14, 67);
            this.lblKeyboardDefinition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKeyboardDefinition.Name = "lblKeyboardDefinition";
            this.lblKeyboardDefinition.Size = new System.Drawing.Size(115, 15);
            this.lblKeyboardDefinition.TabIndex = 1;
            this.lblKeyboardDefinition.Text = "Keyboard Definition:";
            // 
            // CategoryCombo
            // 
            this.CategoryCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CategoryCombo.FormattingEnabled = true;
            this.CategoryCombo.Location = new System.Drawing.Point(18, 29);
            this.CategoryCombo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CategoryCombo.Name = "CategoryCombo";
            this.CategoryCombo.Size = new System.Drawing.Size(184, 23);
            this.CategoryCombo.TabIndex = 2;
            this.CategoryCombo.SelectedIndexChanged += new System.EventHandler(this.CategoryCombo_SelectedIndexChanged);
            // 
            // DefinitionsList
            // 
            this.DefinitionsList.ContextMenuStrip = this.DefinitionListMenu;
            this.DefinitionsList.FormattingEnabled = true;
            this.DefinitionsList.ItemHeight = 15;
            this.DefinitionsList.Location = new System.Drawing.Point(18, 85);
            this.DefinitionsList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DefinitionsList.Name = "DefinitionsList";
            this.DefinitionsList.Size = new System.Drawing.Size(184, 199);
            this.DefinitionsList.TabIndex = 3;
            this.DefinitionsList.SelectedIndexChanged += new System.EventHandler(this.DefinitionsList_SelectedIndexChanged);
            // 
            // DefinitionListMenu
            // 
            this.DefinitionListMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDeleteDefinition});
            this.DefinitionListMenu.Name = "DefinitionListMenu";
            this.DefinitionListMenu.Size = new System.Drawing.Size(108, 26);
            // 
            // mnuDeleteDefinition
            // 
            this.mnuDeleteDefinition.Name = "mnuDeleteDefinition";
            this.mnuDeleteDefinition.Size = new System.Drawing.Size(107, 22);
            this.mnuDeleteDefinition.Text = "Delete";
            this.mnuDeleteDefinition.Click += new System.EventHandler(this.mnuDeleteDefinition_Click);
            // 
            // lblKeyboardStyle
            // 
            this.lblKeyboardStyle.AutoSize = true;
            this.lblKeyboardStyle.Location = new System.Drawing.Point(205, 67);
            this.lblKeyboardStyle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKeyboardStyle.Name = "lblKeyboardStyle";
            this.lblKeyboardStyle.Size = new System.Drawing.Size(88, 15);
            this.lblKeyboardStyle.TabIndex = 4;
            this.lblKeyboardStyle.Text = "Keyboard Style:";
            // 
            // StyleList
            // 
            this.StyleList.FormattingEnabled = true;
            this.StyleList.ItemHeight = 15;
            this.StyleList.Location = new System.Drawing.Point(209, 85);
            this.StyleList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.StyleList.Name = "StyleList";
            this.StyleList.Size = new System.Drawing.Size(184, 199);
            this.StyleList.TabIndex = 5;
            this.StyleList.SelectedIndexChanged += new System.EventHandler(this.StyleList_SelectedIndexChanged);
            // 
            // LoadLegacyButton
            // 
            this.LoadLegacyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LoadLegacyButton.Location = new System.Drawing.Point(306, 10);
            this.LoadLegacyButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LoadLegacyButton.Name = "LoadLegacyButton";
            this.LoadLegacyButton.Size = new System.Drawing.Size(88, 42);
            this.LoadLegacyButton.TabIndex = 8;
            this.LoadLegacyButton.Text = "Load Legacy kb file...";
            this.LoadLegacyButton.UseVisualStyleBackColor = true;
            this.LoadLegacyButton.Click += new System.EventHandler(this.LoadLegacyButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(304, 292);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(88, 27);
            this.CloseButton.TabIndex = 9;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // lblMissingFonts
            // 
            this.lblMissingFonts.AutoSize = true;
            this.lblMissingFonts.Location = new System.Drawing.Point(415, 15);
            this.lblMissingFonts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMissingFonts.Name = "lblMissingFonts";
            this.lblMissingFonts.Size = new System.Drawing.Size(438, 30);
            this.lblMissingFonts.TabIndex = 10;
            this.lblMissingFonts.Text = "The following fonts are defined in the chosen style but not present on this syste" +
    "m.\r\nIf a link is provided, you may download it by double clicking the link:";
            // 
            // fontsGrid
            // 
            this.fontsGrid.AllowUserToAddRows = false;
            this.fontsGrid.AllowUserToDeleteRows = false;
            this.fontsGrid.AllowUserToResizeRows = false;
            this.fontsGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.fontsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fontsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fontsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.fontsGrid.Enabled = false;
            this.fontsGrid.Location = new System.Drawing.Point(419, 48);
            this.fontsGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.fontsGrid.Name = "fontsGrid";
            this.fontsGrid.RowHeadersVisible = false;
            this.fontsGrid.ShowCellErrors = false;
            this.fontsGrid.ShowCellToolTips = false;
            this.fontsGrid.ShowEditingIcon = false;
            this.fontsGrid.ShowRowErrors = false;
            this.fontsGrid.Size = new System.Drawing.Size(577, 205);
            this.fontsGrid.TabIndex = 11;
            this.fontsGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.fontsGrid_CellDoubleClick);
            // 
            // lblRestart
            // 
            this.lblRestart.AutoSize = true;
            this.lblRestart.Location = new System.Drawing.Point(415, 298);
            this.lblRestart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRestart.Name = "lblRestart";
            this.lblRestart.Size = new System.Drawing.Size(443, 15);
            this.lblRestart.TabIndex = 12;
            this.lblRestart.Text = "After a new font has been installed, NohBoard needs to be restarted to recognize " +
    "it.";
            // 
            // btnRestart
            // 
            this.btnRestart.Enabled = false;
            this.btnRestart.Location = new System.Drawing.Point(908, 292);
            this.btnRestart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(88, 27);
            this.btnRestart.TabIndex = 13;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // LoadKeyboardForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(1009, 330);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.lblRestart);
            this.Controls.Add(this.fontsGrid);
            this.Controls.Add(this.lblMissingFonts);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.LoadLegacyButton);
            this.Controls.Add(this.StyleList);
            this.Controls.Add(this.lblKeyboardStyle);
            this.Controls.Add(this.DefinitionsList);
            this.Controls.Add(this.CategoryCombo);
            this.Controls.Add(this.lblKeyboardDefinition);
            this.Controls.Add(this.lblCategory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "LoadKeyboardForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Keyboard";
            this.Load += new System.EventHandler(this.LoadKeyboardForm_Load);
            this.DefinitionListMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fontsGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.DataGridView fontsGrid;
        private System.Windows.Forms.Label lblRestart;
        private System.Windows.Forms.Button btnRestart;
    }
}