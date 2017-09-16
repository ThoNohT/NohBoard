namespace ThoNohT.NohBoard.Controls
{
    partial class MouseSpeedStylePanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.clrOuter = new ColorChooser();
            this.grpOutline = new System.Windows.Forms.GroupBox();
            this.lblOutlineWidth = new System.Windows.Forms.Label();
            this.udOutlineWidth = new System.Windows.Forms.NumericUpDown();
            this.clrInner = new ColorChooser();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpOutline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOutlineWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // clrOuter
            // 
            this.clrOuter.BackColor = System.Drawing.SystemColors.Control;
            this.clrOuter.Color = System.Drawing.Color.Black;
            this.clrOuter.LabelText = "Color 2 (high speed)";
            this.clrOuter.Location = new System.Drawing.Point(7, 51);
            this.clrOuter.Name = "clrOuter";
            this.clrOuter.PreviewShape = ColorChooser.Shape.Square;
            this.clrOuter.Size = new System.Drawing.Size(157, 26);
            this.clrOuter.TabIndex = 0;
            this.clrOuter.ColorChanged += this.clr_ColorChanged;
            // 
            // grpOutline
            // 
            this.grpOutline.Controls.Add(this.clrOuter);
            this.grpOutline.Controls.Add(this.lblOutlineWidth);
            this.grpOutline.Controls.Add(this.udOutlineWidth);
            this.grpOutline.Controls.Add(this.clrInner);
            this.grpOutline.Location = new System.Drawing.Point(2, 30);
            this.grpOutline.Name = "grpOutline";
            this.grpOutline.Size = new System.Drawing.Size(166, 111);
            this.grpOutline.TabIndex = 3;
            this.grpOutline.TabStop = false;
            this.grpOutline.Text = "General";
            // 
            // lblOutlineWidth
            // 
            this.lblOutlineWidth.AutoSize = true;
            this.lblOutlineWidth.Location = new System.Drawing.Point(52, 88);
            this.lblOutlineWidth.Name = "lblOutlineWidth";
            this.lblOutlineWidth.Size = new System.Drawing.Size(71, 13);
            this.lblOutlineWidth.TabIndex = 3;
            this.lblOutlineWidth.Text = "Outline Width";
            // 
            // udOutlineWidth
            // 
            this.udOutlineWidth.Location = new System.Drawing.Point(6, 83);
            this.udOutlineWidth.Name = "udOutlineWidth";
            this.udOutlineWidth.Size = new System.Drawing.Size(40, 20);
            this.udOutlineWidth.TabIndex = 2;
            this.udOutlineWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udOutlineWidth.ValueChanged += new System.EventHandler(this.udOutlineWidth_ValueChanged);
            // 
            // clrInner
            // 
            this.clrInner.BackColor = System.Drawing.SystemColors.Control;
            this.clrInner.Color = System.Drawing.Color.Black;
            this.clrInner.LabelText = "Color 1 (low speed)";
            this.clrInner.Location = new System.Drawing.Point(7, 19);
            this.clrInner.Name = "clrInner";
            this.clrInner.PreviewShape = ColorChooser.Shape.Square;
            this.clrInner.Size = new System.Drawing.Size(156, 26);
            this.clrInner.TabIndex = 0;
            this.clrInner.ColorChanged += this.clr_ColorChanged;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(168, 18);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "MouseSpeedIndicator";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MouseSpeedStylePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.grpOutline);
            this.Name = "MouseSpeedStylePanel";
            this.Size = new System.Drawing.Size(171, 144);
            this.grpOutline.ResumeLayout(false);
            this.grpOutline.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOutlineWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ColorChooser clrOuter;
        private System.Windows.Forms.GroupBox grpOutline;
        private ColorChooser clrInner;
        private System.Windows.Forms.Label lblOutlineWidth;
        private System.Windows.Forms.NumericUpDown udOutlineWidth;
        private System.Windows.Forms.Label lblTitle;
    }
}
