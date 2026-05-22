namespace ThoNohT.NohBoard.Controls
{
    using System.Drawing;

    partial class KeySubStylePanel
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
            grpBackground = new System.Windows.Forms.GroupBox();
            txtBackgoundImage = new System.Windows.Forms.TextBox();
            lblBackgroundImage = new System.Windows.Forms.Label();
            clrBackground = new ColorChooser();
            grpText = new System.Windows.Forms.GroupBox();
            fntText = new FontChooser();
            clrText = new ColorChooser();
            grpOutline = new System.Windows.Forms.GroupBox();
            lblOutlineWidth = new System.Windows.Forms.Label();
            udOutlineWidth = new System.Windows.Forms.NumericUpDown();
            chkShowOutline = new System.Windows.Forms.CheckBox();
            clrOutline = new ColorChooser();
            lblTitle = new System.Windows.Forms.Label();
            grpBackground.SuspendLayout();
            grpText.SuspendLayout();
            grpOutline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)udOutlineWidth).BeginInit();
            SuspendLayout();
            grpBackground.Controls.Add(txtBackgoundImage);
            grpBackground.Controls.Add(lblBackgroundImage);
            grpBackground.Controls.Add(clrBackground);
            grpBackground.Location = new Point(0, 25);
            grpBackground.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpBackground.Name = "grpBackground";
            grpBackground.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpBackground.Size = new Size(200, 85);
            grpBackground.TabIndex = 1;
            grpBackground.TabStop = false;
            grpBackground.Text = "Background";
            txtBackgoundImage.Location = new Point(70, 50);
            txtBackgoundImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtBackgoundImage.Name = "txtBackgoundImage";
            txtBackgoundImage.Size = new Size(125, 23);
            txtBackgoundImage.TabIndex = 2;
            txtBackgoundImage.TextChanged += txtBackgoundImage_TextChanged;
            lblBackgroundImage.Location = new Point(5, 50);
            lblBackgroundImage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBackgroundImage.Name = "lblBackgroundImage";
            lblBackgroundImage.Size = new Size(60, 23);
            lblBackgroundImage.TabIndex = 1;
            lblBackgroundImage.Text = "Image:";
            lblBackgroundImage.TextAlign = ContentAlignment.MiddleLeft;
            clrBackground.BackColor = SystemColors.Control;
            clrBackground.Color = Color.Black;
            clrBackground.LabelText = "Background Color";
            clrBackground.Location = new Point(10, 20);
            clrBackground.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            clrBackground.Name = "clrBackground";
            clrBackground.PreviewShape = ColorChooser.Shape.Square;
            clrBackground.Size = new Size(185, 23);
            clrBackground.TabIndex = 0;
            clrBackground.ColorChanged += clr_ColorChanged;
            clrBackground.Load += clrBackground_Load;
            grpText.Controls.Add(fntText);
            grpText.Controls.Add(clrText);
            grpText.Location = new Point(0, 125);
            grpText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpText.Name = "grpText";
            grpText.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpText.Size = new Size(200, 140);
            grpText.TabIndex = 2;
            grpText.TabStop = false;
            grpText.Text = "Text";
            fntText.BackColor = SystemColors.Control;
            fntText.LabelText = "Pick a font.";
            fntText.Link = null;
            fntText.Location = new Point(5, 50);
            fntText.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            fntText.Name = "fntText";
            fntText.Size = new Size(190, 85);
            fntText.TabIndex = 1;
            fntText.FontChanged += fntText_FontChanged;
            fntText.Load += fntText_Load;
            clrText.BackColor = SystemColors.Control;
            clrText.Color = Color.Black;
            clrText.LabelText = "Text Color";
            clrText.Location = new Point(10, 20);
            clrText.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            clrText.Name = "clrText";
            clrText.PreviewShape = ColorChooser.Shape.Square;
            clrText.Size = new Size(185, 23);
            clrText.TabIndex = 0;
            clrText.ColorChanged += clr_ColorChanged;
            grpOutline.Controls.Add(lblOutlineWidth);
            grpOutline.Controls.Add(udOutlineWidth);
            grpOutline.Controls.Add(chkShowOutline);
            grpOutline.Controls.Add(clrOutline);
            grpOutline.Location = new Point(0, 280);
            grpOutline.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpOutline.Name = "grpOutline";
            grpOutline.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grpOutline.Size = new Size(200, 115);
            grpOutline.TabIndex = 3;
            grpOutline.TabStop = false;
            grpOutline.Text = "Outline";
            lblOutlineWidth.Location = new Point(70, 80);
            lblOutlineWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblOutlineWidth.Name = "lblOutlineWidth";
            lblOutlineWidth.Size = new Size(125, 23);
            lblOutlineWidth.TabIndex = 3;
            lblOutlineWidth.Text = "Outline Width";
            lblOutlineWidth.TextAlign = ContentAlignment.MiddleLeft;
            udOutlineWidth.Location = new Point(5, 80);
            udOutlineWidth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            udOutlineWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            udOutlineWidth.Name = "udOutlineWidth";
            udOutlineWidth.Size = new Size(60, 23);
            udOutlineWidth.TabIndex = 2;
            udOutlineWidth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            udOutlineWidth.ValueChanged += udOutlineWidth_ValueChanged;
            chkShowOutline.Location = new Point(10, 50);
            chkShowOutline.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkShowOutline.Name = "chkShowOutline";
            chkShowOutline.Size = new Size(185, 23);
            chkShowOutline.TabIndex = 1;
            chkShowOutline.Text = "Show Outline";
            chkShowOutline.UseVisualStyleBackColor = true;
            chkShowOutline.CheckedChanged += chkShowOutline_CheckedChanged;
            clrOutline.BackColor = SystemColors.Control;
            clrOutline.Color = Color.Black;
            clrOutline.LabelText = "Outline Color";
            clrOutline.Location = new Point(10, 20);
            clrOutline.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            clrOutline.Name = "clrOutline";
            clrOutline.PreviewShape = ColorChooser.Shape.Square;
            clrOutline.Size = new Size(185, 23);
            clrOutline.TabIndex = 0;
            clrOutline.ColorChanged += clr_ColorChanged;
            lblTitle.AutoEllipsis = true;
            lblTitle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(200, 23);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "SubStyle";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lblTitle);
            Controls.Add(grpText);
            Controls.Add(grpBackground);
            Controls.Add(grpOutline);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "KeySubStylePanel";
            Size = new Size(200, 395);
            grpBackground.ResumeLayout(false);
            grpBackground.PerformLayout();
            grpText.ResumeLayout(false);
            grpOutline.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)udOutlineWidth).EndInit();
            ResumeLayout(false);

        }

        #endregion
        private ColorChooser clrBackground;
        private System.Windows.Forms.GroupBox grpBackground;
        private System.Windows.Forms.GroupBox grpText;
        private ColorChooser clrText;
        private System.Windows.Forms.GroupBox grpOutline;
        private ColorChooser clrOutline;
        private System.Windows.Forms.CheckBox chkShowOutline;
        private System.Windows.Forms.Label lblOutlineWidth;
        private System.Windows.Forms.NumericUpDown udOutlineWidth;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBackgroundImage;
        private System.Windows.Forms.TextBox txtBackgoundImage;
        private FontChooser fntText;
    }
}
