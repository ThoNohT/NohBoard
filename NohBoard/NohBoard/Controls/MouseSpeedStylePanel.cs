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

namespace ThoNohT.NohBoard.Controls
{
    using System;
    using System.Windows.Forms;
    using Keyboard.Styles;
    using ThoNohT.NohBoard.Extra;

    public partial class MouseSpeedStylePanel : UserControl
    {
        private bool setting;

        #region Events

        public event Action<MouseSpeedIndicatorStyle> IndicatorStyleChanged;

        #endregion Events

        #region Constructors

        public MouseSpeedStylePanel()
        {
            this.InitializeComponent();
            this.ApplyLocalizedChrome();
        }

        #endregion Constructors

        private void ApplyLocalizedChrome()
        {
            this.grpOutline.Text = PropertyDialogsLocalization.StyleMouseSpeedGeneralGroup;
            this.lblOutlineWidth.Text = PropertyDialogsLocalization.StyleOutlineWidthLabel;
            this.clrInner.LabelText = PropertyDialogsLocalization.StyleMouseSpeedColor1Low;
            this.clrOuter.LabelText = PropertyDialogsLocalization.StyleMouseSpeedColor2High;
        }

        #region Properties

        public MouseSpeedIndicatorStyle IndicatorStyle
        {
            get
            {
                return new MouseSpeedIndicatorStyle
                {
                    InnerColor = this.clrInner.Color,
                    OuterColor = this.clrOuter.Color,
                    OutlineWidth = (int)this.udOutlineWidth.Value
                };
            }
            set
            {
                this.setting = true;

                this.clrInner.Color = value.InnerColor;
                this.clrOuter.Color = value.OuterColor;
                this.udOutlineWidth.Value = value.OutlineWidth;

                this.setting = false;
            }
        }

        public string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        #endregion Properties

        #region Methods

        private void clr_ColorChanged(ColorChooser sender, System.Drawing.Color color)
        {
            if (!this.setting) this.IndicatorStyleChanged?.Invoke(this.IndicatorStyle);
        }

        private void udOutlineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            if (!this.setting) this.IndicatorStyleChanged?.Invoke(this.IndicatorStyle);
        }

        #endregion Methods

        private void grpOutline_Enter(object sender, EventArgs e)
        {

        }
    }
}
