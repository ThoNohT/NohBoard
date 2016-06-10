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

namespace ThoNohT.NohBoard.Keyboard.Styles
{
    using System.Drawing;
    using System.Runtime.Serialization;

    /// <summary>
    /// The style for a key definition.
    /// </summary>
    [DataContract(Name = "KeyStyle", Namespace = "")]
    public class KeyStyle : ElementStyle
    {
        [DataMember]
        public Color BackgroundLoose { get; set; } = Color.FromArgb(100, 100, 100);

        [DataMember]
        public Color BackgroundPressed { get; set; } = Color.FromArgb(255, 255, 255);

        [DataMember] 
        public Color TextLoose { get; set; } = Color.FromArgb(0, 0, 0);

        [DataMember]
        public Color TextPressed { get; set; } = Color.FromArgb(0, 0, 0);

        [DataMember]
        public Color OutlineLoose { get; set; } = Color.FromArgb(0, 255, 0);

        [DataMember]
        public Color OutlinePressed { get; set; } = Color.FromArgb(0, 255, 0);

        [DataMember]
        public bool ShowOutlineLoose { get; set; }

        [DataMember]
        public bool ShowOutlinePressed { get; set; }

        [DataMember]
        public Font Font { get; set; } = new Font(FontFamily.GenericMonospace, 10);
    }
}