﻿namespace ThoNohT.NohBoard.Forms.Properties
{
    partial class MouseElementPropertiesForm
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.txtTextPosition = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.lblTextPosition = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.cmbKeyCode = new System.Windows.Forms.ComboBox();
            this.lblKeyCode = new System.Windows.Forms.Label();
            this.lstBoundaries = new System.Windows.Forms.ListBox();
            this.lblBoundaries = new System.Windows.Forms.Label();
            this.CancelButton2 = new System.Windows.Forms.Button();
            this.AcceptButton2 = new System.Windows.Forms.Button();
            this.txtBoundaries = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.btnAddBoundary = new System.Windows.Forms.Button();
            this.btnRemoveBoundary = new System.Windows.Forms.Button();
            this.btnBoundaryUp = new System.Windows.Forms.Button();
            this.btnBoundaryDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTextPosition
            // 
            this.txtTextPosition.Location = new System.Drawing.Point(85, 65);
            this.txtTextPosition.MaxVal = 2147483647;
            this.txtTextPosition.Name = "txtTextPosition";
            this.txtTextPosition.Separator = ';';
            this.txtTextPosition.Size = new System.Drawing.Size(156, 20);
            this.txtTextPosition.SpacesAroundSeparator = true;
            this.txtTextPosition.TabIndex = 17;
            this.txtTextPosition.Text = "0 ; 0";
            this.txtTextPosition.X = 0;
            this.txtTextPosition.Y = 0;
            // 
            // lblTextPosition
            // 
            this.lblTextPosition.AutoSize = true;
            this.lblTextPosition.Location = new System.Drawing.Point(8, 68);
            this.lblTextPosition.Name = "lblTextPosition";
            this.lblTextPosition.Size = new System.Drawing.Size(71, 13);
            this.lblTextPosition.TabIndex = 16;
            this.lblTextPosition.Text = "Text Position:";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(85, 39);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(156, 20);
            this.txtText.TabIndex = 18;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(8, 42);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(31, 13);
            this.lblText.TabIndex = 19;
            this.lblText.Text = "Text:";
            // 
            // cmbKeyCode
            // 
            this.cmbKeyCode.FormattingEnabled = true;
            this.cmbKeyCode.Location = new System.Drawing.Point(85, 12);
            this.cmbKeyCode.Name = "cmbKeyCode";
            this.cmbKeyCode.Size = new System.Drawing.Size(156, 21);
            this.cmbKeyCode.TabIndex = 20;
            // 
            // lblKeyCode
            // 
            this.lblKeyCode.AutoSize = true;
            this.lblKeyCode.Location = new System.Drawing.Point(8, 15);
            this.lblKeyCode.Name = "lblKeyCode";
            this.lblKeyCode.Size = new System.Drawing.Size(53, 13);
            this.lblKeyCode.TabIndex = 21;
            this.lblKeyCode.Text = "KeyCode:";
            // 
            // lstBoundaries
            // 
            this.lstBoundaries.FormattingEnabled = true;
            this.lstBoundaries.Location = new System.Drawing.Point(85, 117);
            this.lstBoundaries.Name = "lstBoundaries";
            this.lstBoundaries.Size = new System.Drawing.Size(156, 108);
            this.lstBoundaries.TabIndex = 22;
            // 
            // lblBoundaries
            // 
            this.lblBoundaries.AutoSize = true;
            this.lblBoundaries.Location = new System.Drawing.Point(8, 94);
            this.lblBoundaries.Name = "lblBoundaries";
            this.lblBoundaries.Size = new System.Drawing.Size(63, 13);
            this.lblBoundaries.TabIndex = 23;
            this.lblBoundaries.Text = "Boundaries:";
            // 
            // CancelButton2
            // 
            this.CancelButton2.Location = new System.Drawing.Point(85, 231);
            this.CancelButton2.Name = "CancelButton2";
            this.CancelButton2.Size = new System.Drawing.Size(75, 23);
            this.CancelButton2.TabIndex = 25;
            this.CancelButton2.Text = "Cancel";
            this.CancelButton2.UseVisualStyleBackColor = true;
            // 
            // AcceptButton2
            // 
            this.AcceptButton2.Location = new System.Drawing.Point(166, 231);
            this.AcceptButton2.Name = "AcceptButton2";
            this.AcceptButton2.Size = new System.Drawing.Size(75, 23);
            this.AcceptButton2.TabIndex = 24;
            this.AcceptButton2.Text = "Accept";
            this.AcceptButton2.UseVisualStyleBackColor = true;
            // 
            // txtBoundaries
            // 
            this.txtBoundaries.Location = new System.Drawing.Point(85, 91);
            this.txtBoundaries.MaxVal = 2147483647;
            this.txtBoundaries.Name = "txtBoundaries";
            this.txtBoundaries.Separator = ';';
            this.txtBoundaries.Size = new System.Drawing.Size(156, 20);
            this.txtBoundaries.SpacesAroundSeparator = true;
            this.txtBoundaries.TabIndex = 26;
            this.txtBoundaries.Text = "0 ; 0";
            this.txtBoundaries.X = 0;
            this.txtBoundaries.Y = 0;
            // 
            // btnAddBoundary
            // 
            this.btnAddBoundary.Location = new System.Drawing.Point(4, 117);
            this.btnAddBoundary.Name = "btnAddBoundary";
            this.btnAddBoundary.Size = new System.Drawing.Size(75, 23);
            this.btnAddBoundary.TabIndex = 27;
            this.btnAddBoundary.Text = "Add";
            this.btnAddBoundary.UseVisualStyleBackColor = true;
            // 
            // btnRemoveBoundary
            // 
            this.btnRemoveBoundary.Location = new System.Drawing.Point(4, 146);
            this.btnRemoveBoundary.Name = "btnRemoveBoundary";
            this.btnRemoveBoundary.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveBoundary.TabIndex = 28;
            this.btnRemoveBoundary.Text = "Remove";
            this.btnRemoveBoundary.UseVisualStyleBackColor = true;
            // 
            // btnBoundaryUp
            // 
            this.btnBoundaryUp.Location = new System.Drawing.Point(4, 175);
            this.btnBoundaryUp.Name = "btnBoundaryUp";
            this.btnBoundaryUp.Size = new System.Drawing.Size(75, 23);
            this.btnBoundaryUp.TabIndex = 29;
            this.btnBoundaryUp.Text = "Up";
            this.btnBoundaryUp.UseVisualStyleBackColor = true;
            // 
            // btnBoundaryDown
            // 
            this.btnBoundaryDown.Location = new System.Drawing.Point(4, 204);
            this.btnBoundaryDown.Name = "btnBoundaryDown";
            this.btnBoundaryDown.Size = new System.Drawing.Size(75, 23);
            this.btnBoundaryDown.TabIndex = 30;
            this.btnBoundaryDown.Text = "Down";
            this.btnBoundaryDown.UseVisualStyleBackColor = true;
            // 
            // MouseElementPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 265);
            this.Controls.Add(this.btnBoundaryDown);
            this.Controls.Add(this.btnBoundaryUp);
            this.Controls.Add(this.btnRemoveBoundary);
            this.Controls.Add(this.btnAddBoundary);
            this.Controls.Add(this.txtBoundaries);
            this.Controls.Add(this.CancelButton2);
            this.Controls.Add(this.AcceptButton2);
            this.Controls.Add(this.lblBoundaries);
            this.Controls.Add(this.lstBoundaries);
            this.Controls.Add(this.lblKeyCode);
            this.Controls.Add(this.cmbKeyCode);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.txtTextPosition);
            this.Controls.Add(this.lblTextPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MouseElementPropertiesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MouseElementPropertiesForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.VectorTextBox txtTextPosition;
        private System.Windows.Forms.Label lblTextPosition;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.ComboBox cmbKeyCode;
        private System.Windows.Forms.Label lblKeyCode;
        private System.Windows.Forms.ListBox lstBoundaries;
        private System.Windows.Forms.Label lblBoundaries;
        private System.Windows.Forms.Button CancelButton2;
        private System.Windows.Forms.Button AcceptButton2;
        private Controls.VectorTextBox txtBoundaries;
        private System.Windows.Forms.Button btnAddBoundary;
        private System.Windows.Forms.Button btnRemoveBoundary;
        private System.Windows.Forms.Button btnBoundaryUp;
        private System.Windows.Forms.Button btnBoundaryDown;
    }
}