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


    /// <summary>
    /// A panel containin all the controls for defining a <see cref="MouseSpeedIndicatorStyle"/>.
    /// </summary>
    public partial class MouseSpeedStylePanel : UserControl
    {
        /// <summary>
        /// Indicates whether the style is currently being programmatically set, this should not raise events.
        /// </summary>
        private bool setting;

        #region Events

        /// <summary>
        /// The event that is invoked when the style has been changed. Only invoked when the style is changed through
        /// the user interface, not when it is changed programmatically.
        /// </summary>
        public event Action<MouseSpeedIndicatorStyle> IndicatorStyleChanged;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseSpeedStylePanel" /> class.
        /// </summary>
        public MouseSpeedStylePanel()
        {
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The currently defined indicator style.
        /// </summary>
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

        /// <summary>
        /// The title of the mouse speed indicator panel.
        /// </summary>
        public string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the color changed event of any color chooser.
        /// </summary>
        private void clr_ColorChanged(ColorChooser sender, System.Drawing.Color color)
        {
            if (!this.setting) this.IndicatorStyleChanged?.Invoke(this.IndicatorStyle);
        }

        /// <summary>
        /// Handles the value changed event of the outline width updown.
        /// </summary>
        private void udOutlineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            if (!this.setting) this.IndicatorStyleChanged?.Invoke(this.IndicatorStyle);
        }

        #endregion Methods
    }
}
