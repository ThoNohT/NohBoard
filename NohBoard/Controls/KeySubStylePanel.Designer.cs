namespace ThoNohT.NohBoard.Controls
{
    partial class KeySubStylePanel
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
            this.grpBackground = new System.Windows.Forms.GroupBox();
            this.txtBackgoundImage = new System.Windows.Forms.TextBox();
            this.lblBackgroundImage = new System.Windows.Forms.Label();
            this.grpText = new System.Windows.Forms.GroupBox();
            this.grpOutline = new System.Windows.Forms.GroupBox();
            this.lblOutlineWidth = new System.Windows.Forms.Label();
            this.udOutlineWidth = new System.Windows.Forms.NumericUpDown();
            this.chkShowOutline = new System.Windows.Forms.CheckBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.clrText = new ThoNohT.NohBoard.Controls.ColorChooser();
            this.clrBackground = new ThoNohT.NohBoard.Controls.ColorChooser();
            this.clrOutline = new ThoNohT.NohBoard.Controls.ColorChooser();
            this.fntText = new ThoNohT.NohBoard.Controls.FontChooser();
            this.grpBackground.SuspendLayout();
            this.grpText.SuspendLayout();
            this.grpOutline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOutlineWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBackground
            // 
            this.grpBackground.Controls.Add(this.txtBackgoundImage);
            this.grpBackground.Controls.Add(this.lblBackgroundImage);
            this.grpBackground.Controls.Add(this.clrBackground);
            this.grpBackground.Location = new System.Drawing.Point(2, 21);
            this.grpBackground.Name = "grpBackground";
            this.grpBackground.Size = new System.Drawing.Size(166, 80);
            this.grpBackground.TabIndex = 1;
            this.grpBackground.TabStop = false;
            this.grpBackground.Text = "Background";
            // 
            // txtBackgoundImage
            // 
            this.txtBackgoundImage.Location = new System.Drawing.Point(54, 52);
            this.txtBackgoundImage.Name = "txtBackgoundImage";
            this.txtBackgoundImage.Size = new System.Drawing.Size(100, 20);
            this.txtBackgoundImage.TabIndex = 2;
            this.txtBackgoundImage.TextChanged += new System.EventHandler(this.txtBackgoundImage_TextChanged);
            // 
            // lblBackgroundImage
            // 
            this.lblBackgroundImage.Location = new System.Drawing.Point(6, 48);
            this.lblBackgroundImage.Name = "lblBackgroundImage";
            this.lblBackgroundImage.Size = new System.Drawing.Size(41, 23);
            this.lblBackgroundImage.TabIndex = 1;
            this.lblBackgroundImage.Text = "Image:";
            this.lblBackgroundImage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpText
            // 
            this.grpText.Controls.Add(this.fntText);
            this.grpText.Controls.Add(this.clrText);
            this.grpText.Location = new System.Drawing.Point(2, 109);
            this.grpText.Name = "grpText";
            this.grpText.Size = new System.Drawing.Size(166, 107);
            this.grpText.TabIndex = 2;
            this.grpText.TabStop = false;
            this.grpText.Text = "Text";
            // 
            // grpOutline
            // 
            this.grpOutline.Controls.Add(this.lblOutlineWidth);
            this.grpOutline.Controls.Add(this.udOutlineWidth);
            this.grpOutline.Controls.Add(this.chkShowOutline);
            this.grpOutline.Controls.Add(this.clrOutline);
            this.grpOutline.Location = new System.Drawing.Point(2, 222);
            this.grpOutline.Name = "grpOutline";
            this.grpOutline.Size = new System.Drawing.Size(166, 105);
            this.grpOutline.TabIndex = 3;
            this.grpOutline.TabStop = false;
            this.grpOutline.Text = "Outline";
            // 
            // lblOutlineWidth
            // 
            this.lblOutlineWidth.AutoSize = true;
            this.lblOutlineWidth.Location = new System.Drawing.Point(53, 80);
            this.lblOutlineWidth.Name = "lblOutlineWidth";
            this.lblOutlineWidth.Size = new System.Drawing.Size(71, 13);
            this.lblOutlineWidth.TabIndex = 3;
            this.lblOutlineWidth.Text = "Outline Width";
            // 
            // udOutlineWidth
            // 
            this.udOutlineWidth.Location = new System.Drawing.Point(7, 75);
            this.udOutlineWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
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
            // chkShowOutline
            // 
            this.chkShowOutline.AutoSize = true;
            this.chkShowOutline.Location = new System.Drawing.Point(7, 52);
            this.chkShowOutline.Name = "chkShowOutline";
            this.chkShowOutline.Size = new System.Drawing.Size(89, 17);
            this.chkShowOutline.TabIndex = 1;
            this.chkShowOutline.Text = "Show Outline";
            this.chkShowOutline.UseVisualStyleBackColor = true;
            this.chkShowOutline.CheckedChanged += new System.EventHandler(this.chkShowOutline_CheckedChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(168, 18);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "SubStyle";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clrText
            // 
            this.clrText.BackColor = System.Drawing.SystemColors.Control;
            this.clrText.Color = System.Drawing.Color.Black;
            this.clrText.LabelText = "Text Color";
            this.clrText.Location = new System.Drawing.Point(7, 19);
            this.clrText.Name = "clrText";
            this.clrText.PreviewShape = ThoNohT.NohBoard.Controls.ColorChooser.Shape.Square;
            this.clrText.Size = new System.Drawing.Size(156, 26);
            this.clrText.TabIndex = 0;
            this.clrText.ColorChanged += this.clr_ColorChanged;
            // 
            // clrBackground
            // 
            this.clrBackground.BackColor = System.Drawing.SystemColors.Control;
            this.clrBackground.Color = System.Drawing.Color.Black;
            this.clrBackground.LabelText = "Background Color";
            this.clrBackground.Location = new System.Drawing.Point(6, 19);
            this.clrBackground.Name = "clrBackground";
            this.clrBackground.PreviewShape = ThoNohT.NohBoard.Controls.ColorChooser.Shape.Square;
            this.clrBackground.Size = new System.Drawing.Size(157, 26);
            this.clrBackground.TabIndex = 0;
            this.clrBackground.ColorChanged += this.clr_ColorChanged;
            // 
            // clrOutline
            // 
            this.clrOutline.BackColor = System.Drawing.SystemColors.Control;
            this.clrOutline.Color = System.Drawing.Color.Black;
            this.clrOutline.LabelText = "Outline Color";
            this.clrOutline.Location = new System.Drawing.Point(7, 19);
            this.clrOutline.Name = "clrOutline";
            this.clrOutline.PreviewShape = ThoNohT.NohBoard.Controls.ColorChooser.Shape.Square;
            this.clrOutline.Size = new System.Drawing.Size(156, 26);
            this.clrOutline.TabIndex = 0;
            this.clrOutline.ColorChanged += this.clr_ColorChanged;
            // 
            // fntText
            // 
            this.fntText.BackColor = System.Drawing.SystemColors.Control;
            this.fntText.LabelText = "Pick a font.";
            this.fntText.Location = new System.Drawing.Point(9, 52);
            this.fntText.Name = "fntText";
            this.fntText.Size = new System.Drawing.Size(154, 50);
            this.fntText.TabIndex = 1;
            this.fntText.FontChanged += this.fntText_FontChanged;
            // 
            // KeySubStylePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.grpText);
            this.Controls.Add(this.grpBackground);
            this.Controls.Add(this.grpOutline);
            this.Name = "KeySubStylePanel";
            this.Size = new System.Drawing.Size(171, 329);
            this.grpBackground.ResumeLayout(false);
            this.grpBackground.PerformLayout();
            this.grpText.ResumeLayout(false);
            this.grpOutline.ResumeLayout(false);
            this.grpOutline.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udOutlineWidth)).EndInit();
            this.ResumeLayout(false);

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
