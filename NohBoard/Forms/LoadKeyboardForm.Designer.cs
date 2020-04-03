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
            this.lblCategory.Location = new System.Drawing.Point(12, 9);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 0;
            this.lblCategory.Text = "Category:";
            // 
            // lblKeyboardDefinition
            // 
            this.lblKeyboardDefinition.AutoSize = true;
            this.lblKeyboardDefinition.Location = new System.Drawing.Point(12, 58);
            this.lblKeyboardDefinition.Name = "lblKeyboardDefinition";
            this.lblKeyboardDefinition.Size = new System.Drawing.Size(102, 13);
            this.lblKeyboardDefinition.TabIndex = 1;
            this.lblKeyboardDefinition.Text = "Keyboard Definition:";
            // 
            // CategoryCombo
            // 
            this.CategoryCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CategoryCombo.FormattingEnabled = true;
            this.CategoryCombo.Location = new System.Drawing.Point(15, 25);
            this.CategoryCombo.Name = "CategoryCombo";
            this.CategoryCombo.Size = new System.Drawing.Size(158, 21);
            this.CategoryCombo.TabIndex = 2;
            this.CategoryCombo.SelectedIndexChanged += new System.EventHandler(this.CategoryCombo_SelectedIndexChanged);
            // 
            // DefinitionsList
            // 
            this.DefinitionsList.ContextMenuStrip = this.DefinitionListMenu;
            this.DefinitionsList.FormattingEnabled = true;
            this.DefinitionsList.Location = new System.Drawing.Point(15, 74);
            this.DefinitionsList.Name = "DefinitionsList";
            this.DefinitionsList.Size = new System.Drawing.Size(158, 173);
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
            this.lblKeyboardStyle.Location = new System.Drawing.Point(176, 58);
            this.lblKeyboardStyle.Name = "lblKeyboardStyle";
            this.lblKeyboardStyle.Size = new System.Drawing.Size(81, 13);
            this.lblKeyboardStyle.TabIndex = 4;
            this.lblKeyboardStyle.Text = "Keyboard Style:";
            // 
            // StyleList
            // 
            this.StyleList.FormattingEnabled = true;
            this.StyleList.Location = new System.Drawing.Point(179, 74);
            this.StyleList.Name = "StyleList";
            this.StyleList.Size = new System.Drawing.Size(158, 173);
            this.StyleList.TabIndex = 5;
            this.StyleList.SelectedIndexChanged += new System.EventHandler(this.StyleList_SelectedIndexChanged);
            // 
            // LoadLegacyButton
            // 
            this.LoadLegacyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadLegacyButton.Location = new System.Drawing.Point(262, 9);
            this.LoadLegacyButton.Name = "LoadLegacyButton";
            this.LoadLegacyButton.Size = new System.Drawing.Size(75, 36);
            this.LoadLegacyButton.TabIndex = 8;
            this.LoadLegacyButton.Text = "Load Legacy kb file...";
            this.LoadLegacyButton.UseVisualStyleBackColor = true;
            this.LoadLegacyButton.Click += new System.EventHandler(this.LoadLegacyButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(261, 253);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 9;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // lblMissingFonts
            // 
            this.lblMissingFonts.AutoSize = true;
            this.lblMissingFonts.Location = new System.Drawing.Point(356, 13);
            this.lblMissingFonts.Name = "lblMissingFonts";
            this.lblMissingFonts.Size = new System.Drawing.Size(389, 26);
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
            this.fontsGrid.Location = new System.Drawing.Point(359, 42);
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
            this.lblRestart.Location = new System.Drawing.Point(356, 258);
            this.lblRestart.Name = "lblRestart";
            this.lblRestart.Size = new System.Drawing.Size(399, 13);
            this.lblRestart.TabIndex = 12;
            this.lblRestart.Text = "After a new font has been installed, NohBoard needs to be restarted to recognize " +
    "it.";
            // 
            // btnRestart
            // 
            this.btnRestart.Enabled = false;
            this.btnRestart.Location = new System.Drawing.Point(861, 253);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 23);
            this.btnRestart.TabIndex = 13;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // LoadKeyboardForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(348, 286);
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