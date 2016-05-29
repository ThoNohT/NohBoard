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
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Extra;
    using Hooking;
    using Hooking.Interop;
    using Keyboard;
    using Keyboard.ElementDefinitions;

    public partial class MainForm : Form
    {
        // TODO: Document all methods in form files.

        /// <summary>
        /// The back-brushes used for efficient drawing.
        /// </summary>
        private readonly Dictionary<bool, Dictionary<bool, Brush>> backBrushes =
            new Dictionary<bool, Dictionary<bool, Brush>>();

        public MainForm()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #region Keyboard loading and saving

        private void LoadKeyboard()
        {
            if (GlobalSettings.CurrentDefinition == null)
                return;

            this.ClientSize = new Size(GlobalSettings.CurrentDefinition.Width, GlobalSettings.CurrentDefinition.Height);

            this.ResetBackBrushes();
        }

        public void ResetBackBrushes()
        {
            foreach (var brush in this.backBrushes)
            {
                foreach (var b in brush.Value)
                    b.Value.Dispose();

                brush.Value.Clear();
            }
            this.backBrushes.Clear();

            // Fill the back-brushes.
            foreach (var shift in new[] { false, true })
            {
                this.backBrushes.Add(shift, new Dictionary<bool, Brush>());

                foreach (var caps in new[] { false, true })
                {
                    var bmp = new Bitmap(
                        GlobalSettings.CurrentDefinition.Width,
                        GlobalSettings.CurrentDefinition.Height);
                    var g = Graphics.FromImage(bmp);

                    foreach (var def in GlobalSettings.CurrentDefinition.Elements.OfType<KeyboardKeyDefinition>())
                        def.Render(g, false, shift, caps);

                    foreach (var def in GlobalSettings.CurrentDefinition.Elements.OfType<MouseKeyDefinition>())
                        def.Render(g, false, shift, caps);

                    foreach (var def in GlobalSettings.CurrentDefinition.Elements.OfType<MouseScrollDefinition>())
                        def.Render(g, 0);

                    foreach (var def in GlobalSettings.CurrentDefinition.Elements.OfType<MouseSpeedIndicatorDefinition>())
                        def.Render(g, new SizeF());

                    this.backBrushes[shift].Add(caps, new TextureBrush(bmp));
                }
            }
        }

        internal void LoadNewKeyboard(KeyboardDefinition kbDef, KeyboardStyle kbStyle)
        {
            GlobalSettings.CurrentDefinition = kbDef;
            GlobalSettings.CurrentStyle = kbStyle;
            this.LoadKeyboard();
        }

        private void mnuLoadKeyboard_Click(object sender, EventArgs e)
        {
            var manageForm = new LoadKeyboardForm(this);
            var result = manageForm.ShowDialog(this);

            // If a legacy kb file was loaded, this has to be handled manually. Other types are handled by the load
            // form.
            if (result == DialogResult.Yes)
            {
                GlobalSettings.CurrentDefinition = manageForm.LoadedLegacyDefinition;
                this.LoadKeyboard();
            }
        }

        private void mnuSaveDefinitionAsName_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentDefinition.Save();
            GlobalSettings.Settings.LoadedCategory = GlobalSettings.CurrentDefinition.Category;
            GlobalSettings.Settings.LoadedKeyboard = GlobalSettings.CurrentDefinition.Name;
        }

        private void mnuSaveDefinitionAs_Click(object sender, EventArgs e)
        {
            var saveForm = new SaveKeyboardAsForm();
            saveForm.ShowDialog(this);
        }

        #endregion Keyboard loading and saving

        #region Settings
        private void MainForm_Load(object sender, System.EventArgs e)
        {
            // Load the settings
            
            if (!GlobalSettings.Load())
            {
                MessageBox.Show(
                    this,
                    $"Failed to load the settings: {GlobalSettings.Errors}",
                    "Failed to load settings");
            }

            this.Location = new Point(GlobalSettings.Settings.X, GlobalSettings.Settings.Y);

            // Load a definition if possible.
            if (GlobalSettings.Settings.LoadedKeyboard != null && GlobalSettings.Settings.LoadedCategory != null)
            {
                GlobalSettings.CurrentDefinition = KeyboardDefinition
                    .Load(GlobalSettings.Settings.LoadedCategory, GlobalSettings.Settings.LoadedKeyboard);
                this.LoadKeyboard();
            }

            // Load a style if possible.
            if (GlobalSettings.Settings.LoadedStyle != null)
            {
                try
                {
                    GlobalSettings.CurrentStyle = KeyboardStyle.Load(
                        GlobalSettings.Settings.LoadedStyle,
                        GlobalSettings.Settings.LoadedGlobalStyle);
                    this.ResetBackBrushes();
                }
                catch
                {
                    MessageBox.Show(
                        $"Failed to load style {GlobalSettings.Settings.LoadedStyle}, loading default style.",
                        "Error loading style.");
                }
            }

            HookManager.EnableMouseHook();
            HookManager.EnableKeyboardHook();

            this.UpdateTimer.Enabled = true;
            this.KeyCheckTimer.Enabled = true;

            this.Activate();
            this.ApplySettings();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HookManager.DisableMouseHook();
            HookManager.DisableKeyboardHook();

            GlobalSettings.Settings.X = this.Location.X;
            GlobalSettings.Settings.Y = this.Location.Y;

            GlobalSettings.Save();
        }

        /// <summary>
        /// Applies the currently stored settings.
        /// </summary>
        private void ApplySettings()
        {
            HookManager.TrapKeyboard = GlobalSettings.Settings.TrapKeyboard;
            HookManager.TrapMouse = GlobalSettings.Settings.TrapMouse;
            HookManager.TrapToggleKeyCode = GlobalSettings.Settings.TrapToggleKeyCode;
            HookManager.ScrollHold = GlobalSettings.Settings.ScrollHold;
            this.LoadKeyboard();
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            var result = settingsForm.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            // Re-initialize with the new settings.
            this.ApplySettings();
        }

        private void MainMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.mnuSaveDefinition.Enabled = GlobalSettings.CurrentDefinition != null;
            if (GlobalSettings.CurrentDefinition != null)
            {
                this.mnuSaveDefinitionAsName.Text =
                    $"Save &To '{GlobalSettings.CurrentDefinition.Category}/{GlobalSettings.CurrentDefinition.Name}'";
            }

            this.mnuEditKeyboardStyle.Enabled = GlobalSettings.CurrentDefinition != null;

            this.mnuEditElementStyle.Enabled = false; // TODO: Implement element styles.
            this.mnuSaveStyleToName.Text = $"Save &To '{GlobalSettings.CurrentStyle.Name}'";
            this.mnuSaveStyleToName.Visible = !GlobalSettings.Settings.LoadedGlobalStyle;
            this.mnuSaveToGlobalStyleName.Text = $"Save To &Global '{GlobalSettings.CurrentStyle.Name}'";
            this.mnuSaveToGlobalStyleName.Enabled = GlobalSettings.CurrentStyle.IsGlobal;
            this.mnuSaveToGlobalStyleName.Visible = GlobalSettings.Settings.LoadedGlobalStyle;

            this.mnuToggleEditMode.Enabled = false; // TODO: Implement edit mode.
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion Settings

        #region Rendering
        
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(GlobalSettings.CurrentStyle.BackgroundColor);

            if (GlobalSettings.CurrentDefinition == null || !this.backBrushes.Any())
                return;

            // Fill the appropriate back brush.
            e.Graphics.FillRectangle(
                this.backBrushes[KeyboardState.ShiftDown][KeyboardState.CapsActive],
                new Rectangle(0, 0, GlobalSettings.CurrentDefinition.Width, GlobalSettings.CurrentDefinition.Height));

            // Render keyboard keys.
            var kbKeys = KeyboardState.PressedKeys;
            foreach (var def in kbKeys.SelectMany(
                keyCode => GlobalSettings.CurrentDefinition.Elements.OfType<KeyboardKeyDefinition>()
                    .Where(x => x.KeyCode == keyCode)))
            {
                def.Render(e.Graphics, true, KeyboardState.ShiftDown, KeyboardState.CapsActive);
            }

            // Render mouse keys.
            var mouseKeys = MouseState.PressedKeys;
            foreach (var def in mouseKeys.SelectMany(
                keyCode => GlobalSettings.CurrentDefinition.Elements.OfType<MouseKeyDefinition>()
                    .Where(x => x.KeyCode == (int)keyCode)))
            {
                def.Render(e.Graphics, true, KeyboardState.ShiftDown, KeyboardState.CapsActive);
            }

            // Render mouse scrolls.
            MouseState.CheckScrollAndMovement();
            var scrollCounts = MouseState.ScrollCounts;
            for (var i = 0; i < scrollCounts.Count; i++)
            {
                if (scrollCounts[i] == 0)
                    continue;

                foreach (var def in GlobalSettings.CurrentDefinition.Elements.OfType<MouseScrollDefinition>()
                    .Where(x => x.KeyCode == i)
                    .ToList())
                    def.Render(e.Graphics, scrollCounts[i]);
            }

            // Render mouse speeds.
            foreach (var def in GlobalSettings.CurrentDefinition.Elements.OfType<MouseSpeedIndicatorDefinition>())
                def.Render(e.Graphics, MouseState.AverageSpeed);

            base.OnPaint(e);
        }

        private void UpdateTimer_Tick(object sender, System.EventArgs e)
        {
            if (KeyboardState.Updated || MouseState.Updated)
                this.Refresh();
        }

        /// <summary>
        /// Periodically checks that no keys got stuck.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyCheckTimer_Tick(object sender, EventArgs e)
        {
            MouseState.CheckKeys();
            KeyboardState.CheckKeys();
        }

        #endregion Rendering

        #region Styles

        private void mnuEditKeyboardStyle_Click(object sender, EventArgs e)
        {
            var styleForm = new KeyboardStyleForm(this);
            styleForm.ShowDialog(this);
        }

        private void mnuSaveStyleToName_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentStyle.Save(false);
            GlobalSettings.Settings.LoadedStyle = GlobalSettings.CurrentStyle.Name;
            GlobalSettings.Settings.LoadedGlobalStyle = false;
        }

        private void mnuSaveToGlobalStyleName_Click(object sender, EventArgs e)
        {
            GlobalSettings.CurrentStyle.Save(true);
            GlobalSettings.Settings.LoadedStyle = GlobalSettings.CurrentStyle.Name;
            GlobalSettings.Settings.LoadedGlobalStyle = true;
        }

        private void mnuSaveStyleAs_Click(object sender, EventArgs e)
        {
            var saveForm = new SaveStyleAsForm();
            saveForm.ShowDialog(this);
        }

        #endregion Styles
    }
}