namespace ThoNohT.NohBoard.Controls
{
    using System.Drawing;

    partial class MouseSpeedStylePanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            clrOuter = new ColorChooser();
            grpOutline = new System.Windows.Forms.GroupBox();
            lblOutlineWidth = new System.Windows.Forms.Label();
            udOutlineWidth = new System.Windows.Forms.NumericUpDown();
            clrInner = new ColorChooser();
            lblTitle = new System.Windows.Forms.Label();
            grpOutline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)udOutlineWidth).BeginInit();
            SuspendLayout();
            clrOuter.BackColor = SystemColors.Control;
            clrOuter.Color = Color.Black;
            clrOuter.LabelText = "Color 2 (high speed)";
            clrOuter.Location = new Point(10, 55);
            clrOuter.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            clrOuter.Name = "clrOuter";
            clrOuter.PreviewShape = ColorChooser.Shape.Square;
            clrOuter.Size = new Size(185, 23);
            clrOuter.TabIndex = 0;
            clrOuter.ColorChanged += clr_ColorChanged;
            grpOutline.Controls.Add(clrOuter);
            grpOutline.Controls.Add(lblOutlineWidth);
            grpOutline.Controls.Add(udOutlineWidth);
            grpOutline.Controls.Add(clrInner);
            grpOutline.Location = new Point(0, 25);
            grpOutline.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpOutline.Name = "grpOutline";
            grpOutline.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpOutline.Size = new Size(200, 120);
            grpOutline.TabIndex = 3;
            grpOutline.TabStop = false;
            grpOutline.Text = "General";
            grpOutline.Enter += grpOutline_Enter;
            lblOutlineWidth.Location = new Point(70, 85);
            lblOutlineWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblOutlineWidth.Name = "lblOutlineWidth";
            lblOutlineWidth.Size = new Size(125, 23);
            lblOutlineWidth.TabIndex = 3;
            lblOutlineWidth.Text = "Outline Width";
            lblOutlineWidth.TextAlign = ContentAlignment.MiddleLeft;
            udOutlineWidth.Location = new Point(10, 85);
            udOutlineWidth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            udOutlineWidth.Name = "udOutlineWidth";
            udOutlineWidth.Size = new Size(60, 23);
            udOutlineWidth.TabIndex = 2;
            udOutlineWidth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            udOutlineWidth.ValueChanged += udOutlineWidth_ValueChanged;
            clrInner.BackColor = SystemColors.Control;
            clrInner.Color = Color.Black;
            clrInner.LabelText = "Color 1 (low speed)";
            clrInner.Location = new Point(10, 25);
            clrInner.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            clrInner.Name = "clrInner";
            clrInner.PreviewShape = ColorChooser.Shape.Square;
            clrInner.Size = new Size(185, 23);
            clrInner.TabIndex = 0;
            clrInner.ColorChanged += clr_ColorChanged;
            lblTitle.AutoEllipsis = true;
            lblTitle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(200, 23);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "MouseSpeedIndicator";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lblTitle);
            Controls.Add(grpOutline);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MouseSpeedStylePanel";
            Size = new Size(200, 145);
            grpOutline.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)udOutlineWidth).EndInit();
            ResumeLayout(false);

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
