namespace ThoNohT.NohBoard.Forms.Properties
{
    partial class RectangleBoundaryForm
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
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.txtSize = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.txtPosition = new ThoNohT.NohBoard.Controls.VectorTextBox();
            this.SuspendLayout();
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(8, 14);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(44, 13);
            this.lblPosition.TabIndex = 0;
            this.lblPosition.Text = "Position";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(8, 40);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(27, 13);
            this.lblSize.TabIndex = 1;
            this.lblSize.Text = "Size";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(35, 63);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(116, 63);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(85, 37);
            this.txtSize.MaxVal = 2147483647;
            this.txtSize.Name = "txtSize";
            this.txtSize.Separator = ';';
            this.txtSize.Size = new System.Drawing.Size(100, 20);
            this.txtSize.SpacesAroundSeparator = true;
            this.txtSize.TabIndex = 3;
            this.txtSize.Text = "0 ; 0";
            this.txtSize.X = 0;
            this.txtSize.Y = 0;
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(85, 11);
            this.txtPosition.MaxVal = 2147483647;
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Separator = ';';
            this.txtPosition.Size = new System.Drawing.Size(100, 20);
            this.txtPosition.SpacesAroundSeparator = true;
            this.txtPosition.TabIndex = 2;
            this.txtPosition.Text = "0 ; 0";
            this.txtPosition.X = 0;
            this.txtPosition.Y = 0;
            // 
            // RectangleBoundaryForm
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(203, 96);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.lblPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RectangleBoundaryForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rectangle";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblSize;
        private Controls.VectorTextBox txtPosition;
        private Controls.VectorTextBox txtSize;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
    }
}