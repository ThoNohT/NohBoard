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
    using Extra;
    using Hooking;
    using Keyboard;
    using Keyboard.ElementDefinitions;
    using Keyboard.Styles;
    using Properties;
    using Style;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public partial class MainForm
    {
        #region Manipulations

        private (int id, ElementDefinition definition)? currentlyManipulating = null;

        private ElementDefinition manipulationStart = null;

        private Size cumulManipulation;

        private Point currentManipulationPoint;

        private TPoint movingEverythingFrom;

        private KeyboardDefinition movingEverythingStart;

        #endregion Manipulations

        private ElementDefinition highlightedDefinition = null;

        private ElementDefinition selectedDefinition = null;

        private ElementDefinition relevantDefinition => this.selectedDefinition ?? this.highlightedDefinition;

        private ElementDefinition clipboard;

        private bool menuOpen;

        private void mnuToggleEditMode_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            this.ApplyLocalizedEditModeToggleText();
            this.FormBorderStyle =
                this.mnuToggleEditMode.Checked ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;

            if (!this.mnuToggleEditMode.Checked)
            {
                this.currentlyManipulating = null;
                this.highlightedDefinition = null;
                this.selectedDefinition = null;
            }
        }

        private void mnuUpdateTextPosition_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateTextPosition = this.mnuUpdateTextPosition.Checked;
            GlobalSettings.Save();
        }

        #region Mouse manipulations

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._overlayLocked) return;
            if (e.Button != MouseButtons.Left) return;
            if (!this.mnuToggleEditMode.Checked || this.menuOpen) return;

            var location = this.ScalePointToKeyboardPoint(e.Location);
            ElementDefinition toManipulate;
            if (this.selectedDefinition != null)
            {
                this.selectedDefinition.StartManipulating(location, KeyboardState.AltDown, translateOnly: KeyboardState.CtrlDown);
                toManipulate = this.selectedDefinition;
            }
            else
            {
                toManipulate = GlobalSettings.CurrentDefinition.Elements
                    .LastOrDefault(x => x.StartManipulating(location, KeyboardState.AltDown, translateOnly: KeyboardState.CtrlDown));
            }

            if (toManipulate == null)
            {
                this.currentlyManipulating = null;
                this.selectedDefinition = null;

                if (KeyboardState.AltDown)
                {
                    this.movingEverythingStart = GlobalSettings.CurrentDefinition.Clone();
                    this.movingEverythingFrom = location;
                }

                return;
            }

            var indexToManipulate = GlobalSettings.CurrentDefinition.Elements.IndexOf(toManipulate);
            this.currentlyManipulating = (indexToManipulate, toManipulate);
            this.highlightedDefinition = null;
            this.manipulationStart = toManipulate;
            this.cumulManipulation = new Size();

            GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition.RemoveElement(toManipulate), false);

            this.ResetBackBrushes();
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._overlayLocked) return;
            if (!this.mnuToggleEditMode.Checked || this.menuOpen) return;

            var location = this.ScalePointToKeyboardPoint(e.Location);

            if (this.movingEverythingFrom != null)
            {
                var newDef = this.movingEverythingStart.Clone();
                newDef.Elements.Clear();
                foreach (var element in this.movingEverythingStart.Elements)
                {
                    var diff = location - this.movingEverythingFrom;
                    newDef.Elements.Add(element.Translate(diff.Width, diff.Height));
                }

                GlobalSettings.Settings.UpdateDefinition(newDef, false);
                this.ResetBackBrushes();
            }

            if (this.currentlyManipulating != null)
            {
                var diff = (TPoint)location - this.currentManipulationPoint;
                this.cumulManipulation += diff;

                this.currentlyManipulating = (this.currentlyManipulating.Value.id, this.manipulationStart.Manipulate(this.cumulManipulation));
                this.currentManipulationPoint = location;
            }
            else
            {
                this.currentManipulationPoint = location;

                if (this.selectedDefinition != null)
                {
                    this.selectedDefinition.StartManipulating(location, KeyboardState.AltDown, true, KeyboardState.CtrlDown);
                }
                else
                {
                    this.highlightedDefinition = GlobalSettings.CurrentDefinition.Elements
                        .LastOrDefault(x => x.StartManipulating(location, KeyboardState.AltDown, translateOnly: KeyboardState.CtrlDown));
                }
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (this._overlayLocked) return;
            var location = this.ScalePointToKeyboardPoint(e.Location);

            if (this.movingEverythingFrom != null)
            {
                GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition, true);
                this.movingEverythingFrom = null;
                this.ResetBackBrushes();
            }

            if (e.Button != MouseButtons.Left) return;
            this.menuOpen = false;

            if (!this.mnuToggleEditMode.Checked || this.currentlyManipulating == null) return;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.AddElement(
                    this.currentlyManipulating.Value.definition,
                    this.currentlyManipulating.Value.id),
                true);

            if (this.cumulManipulation.Length() == 0 && this.selectedDefinition != null)
            {
                var elementsUnderCursor = GlobalSettings.CurrentDefinition.Elements
                  .Where(x => x.StartManipulating(location, KeyboardState.AltDown, translateOnly: KeyboardState.CtrlDown))
                  .Reverse();

                var nextelementUnderCursor = elementsUnderCursor
                    .SkipWhile(el => el.Id != this.currentlyManipulating.Value.definition.Id).Skip(1).FirstOrDefault()
                    ?? elementsUnderCursor.FirstOrDefault();
                this.selectedDefinition = nextelementUnderCursor;
            }
            else
            {
                this.selectedDefinition = this.currentlyManipulating.Value.definition;
            }
            this.currentlyManipulating = null;
            this.manipulationStart = null;
            this.currentManipulationPoint = new Point();
            this.ResetBackBrushes();
        }

        #endregion Mouse manipulations

        #region Element z-order moving

        private void mnuMoveToTop_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.MoveElementDown(this.relevantDefinition, int.MaxValue), true);
            this.ResetBackBrushes();
        }

        private void mnuMoveUp_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.MoveElementDown(this.relevantDefinition, 1), true);
            this.ResetBackBrushes();
        }

        private void mnuMoveDown_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.MoveElementDown(this.relevantDefinition, -1), true);
            this.ResetBackBrushes();
        }

        private void mnuMoveToBottom_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.MoveElementDown(this.relevantDefinition, -int.MaxValue), true);
            this.ResetBackBrushes();
        }

        #endregion Element z-order moving

        #region Keyboard input handling

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!this.mnuToggleEditMode.Checked) return base.ProcessCmdKey(ref msg, keyData);

            var keyCode = keyData & Keys.KeyCode;

            if (this.selectedDefinition != null && new[] { Keys.Up, Keys.Right, Keys.Down, Keys.Left }.Contains(keyCode))
            {
                ElementDefinition newDefinition;
                var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(this.selectedDefinition);
                switch (keyCode)
                {
                    case Keys.Right:
                        newDefinition = this.selectedDefinition.Manipulate(new Size(1, 0));
                        break;
                    case Keys.Down:
                        newDefinition = this.selectedDefinition.Manipulate(new Size(0, 1));
                        break;
                    case Keys.Left:
                        newDefinition = this.selectedDefinition.Manipulate(new Size(-1, 0));
                        break;
                    case Keys.Up:
                        newDefinition = this.selectedDefinition.Manipulate(new Size(0, -1));
                        break;
                    default:
                        throw new Exception("If this happens, the if statement around this switch is incorrect.");
                }

                GlobalSettings.Settings.UpdateDefinition(
                    GlobalSettings.CurrentDefinition
                        .RemoveElement(this.selectedDefinition).AddElement(newDefinition, index),
                    true);

                this.selectedDefinition = newDefinition;
                this.ResetBackBrushes();
                this.InvalidateKeyboardSurface();
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if (keyCode == Keys.Escape || keyCode == Keys.Enter)
            {
                this.selectedDefinition = null;
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if ((keyData & Keys.Control) != 0 && keyCode == Keys.Z)
            {
                if ((keyData & Keys.Shift) == 0)
                {
                    if (GlobalSettings.Settings.Undo())
                    {
                        this.ClientSize = new Size(
                          GlobalSettings.CurrentDefinition.Width,
                          GlobalSettings.CurrentDefinition.Height);
                    }
                    else
                    {
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
                }
                else
                {
                    if (GlobalSettings.Settings.Redo())
                    {
                        this.ClientSize = new Size(
                          GlobalSettings.CurrentDefinition.Width,
                          GlobalSettings.CurrentDefinition.Height);
                    }
                    else
                    {
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
                }

                this.selectedDefinition = null;
                this.highlightedDefinition = null;
                this.ResetBackBrushes();
                this.InvalidateKeyboardSurface();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion Keyboard input handling

        #region Element management

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.menuOpen = false;

            if (e.KeyCode == Keys.C && e.Control && this.mnuToggleEditMode.Checked)
            {
                if (this.selectedDefinition == null) return;
                this.clipboard = this.selectedDefinition;
            }

            if (e.KeyCode == Keys.V && e.Control && this.mnuToggleEditMode.Checked)
            {
                if (this.clipboard == null) return;

                var newPos = this.PointToClient(MousePosition);
                var validArea = new Rectangle(0, 0, GlobalSettings.CurrentDefinition.Width,
                        GlobalSettings.CurrentDefinition.Height);
                if (!validArea.Contains(newPos)) return;

                var oldPos = this.clipboard.GetBoundingBox().GetCenter();
                var dist = newPos - oldPos;

                var elementToAdd = this.clipboard
                    .SetId(GlobalSettings.CurrentDefinition.GetNextId())
                    .Translate(dist.Width, dist.Height);

                var newDefinition = GlobalSettings.CurrentDefinition.AddElement(elementToAdd);

                if (GlobalSettings.CurrentStyle.ElementIsStyled(this.clipboard.Id))
                {
                    var newStyle = GlobalSettings.CurrentStyle
                        .SetElementStyle(elementToAdd.Id, GlobalSettings.CurrentStyle.ElementStyles[this.clipboard.Id]);
                    GlobalSettings.Settings.UpdateBoth(newDefinition, newStyle, true);
                }
                else
                {
                    GlobalSettings.Settings.UpdateDefinition(newDefinition, true);
                }

                this.selectedDefinition = elementToAdd;

                this.ResetBackBrushes();
            }
        }

        private void AddElement(ElementDefinition definition)
        {
            if (!this.mnuToggleEditMode.Checked) return;
            if (this.highlightedDefinition != null) return;

            GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition.AddElement(definition), true);

            this.ResetBackBrushes();
        }

        private void mnuAddKeyboardKeyDefinition_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            var c = this.currentManipulationPoint;
            var w = Constants.DefaultElementSize / 2;
            var boundaries = new List<TPoint>
            {
                new TPoint(c.X - w, c.Y - w),
                new TPoint(c.X + w, c.Y - w),
                new TPoint(c.X + w, c.Y + w),
                new TPoint(c.X - w, c.Y + w),
            };

            this.AddElement(
                new KeyboardKeyDefinition(
                    GlobalSettings.CurrentDefinition.GetNextId(),
                    boundaries,
                    new List<int>(),
                    "",
                    "",
                    true));
        }

        private void mnuAddMouseKeyDefinition_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            var c = this.currentManipulationPoint;
            var w = Constants.DefaultElementSize / 2;
            var boundaries = new List<TPoint>
            {
                new TPoint(c.X - w, c.Y - w),
                new TPoint(c.X + w, c.Y - w),
                new TPoint(c.X + w, c.Y + w),
                new TPoint(c.X - w, c.Y + w),
            };

            this.AddElement(
                new MouseKeyDefinition(
                    GlobalSettings.CurrentDefinition.GetNextId(),
                    boundaries,
                    (int) MouseKeyCode.LeftButton,
                    ""));
        }

        private void mnuAddMouseScrollDefinition_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            var c = this.currentManipulationPoint;
            var w = Constants.DefaultElementSize / 2;
            var boundaries = new List<TPoint>
            {
                new TPoint(c.X - w, c.Y - w),
                new TPoint(c.X + w, c.Y - w),
                new TPoint(c.X + w, c.Y + w),
                new TPoint(c.X - w, c.Y + w),
            };

            this.AddElement(
                new MouseScrollDefinition(
                    GlobalSettings.CurrentDefinition.GetNextId(),
                    boundaries,
                    (int) MouseScrollKeyCode.ScrollDown,
                    ""));
        }

        private void mnuAddMouseSpeedIndicatorDefinition_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            this.AddElement(
                new MouseSpeedIndicatorDefinition(
                    GlobalSettings.CurrentDefinition.GetNextId(),
                    this.currentManipulationPoint,
                    Constants.DefaultElementSize / 2));
        }

        private void mnuRemoveElement_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            if (!this.mnuToggleEditMode.Checked) return;
            if (this.highlightedDefinition == null && this.selectedDefinition == null) return;

            var definitionToRemove = this.highlightedDefinition ?? this.selectedDefinition;
            var newDefinition = GlobalSettings.CurrentDefinition.RemoveElement(definitionToRemove);

            if (GlobalSettings.CurrentStyle.ElementIsStyled(definitionToRemove.Id))
            {
                var newStyle = GlobalSettings.CurrentStyle.RemoveElementStyle(definitionToRemove.Id);
                GlobalSettings.Settings.UpdateBoth(newDefinition, newStyle, true);
            }
            else
            {
                GlobalSettings.Settings.UpdateDefinition(newDefinition, true);
            }

            this.highlightedDefinition = null;
            this.currentlyManipulating = null;
            this.manipulationStart = null;
            this.selectedDefinition = null;

            this.ResetBackBrushes();
        }

        #endregion Element management

        #region Boundary management

        private void mnuAddBoundaryPoint_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            if (!this.mnuToggleEditMode.Checked) return;
            if (!(this.relevantDefinition is KeyDefinition)) return;

            var def = (KeyDefinition)this.relevantDefinition;
            var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(def);
            var newDef = def.AddBoundary(this.currentManipulationPoint);
            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.RemoveElement(def).AddElement(newDef, index), true);

            if (this.selectedDefinition != null) this.selectedDefinition = newDef;

            this.ResetBackBrushes();
        }

        private void mnuRemoveBoundaryPoint_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            if (!this.mnuToggleEditMode.Checked) return;
            if (!(this.relevantDefinition is KeyDefinition)) return;

            var def = (KeyDefinition)this.relevantDefinition;
            if (def.Boundaries.Count < 4)
            {
                this.ShowAppMessageBox(
                    UiTranslate.T(
                        "You cannot remove another boundary, there must be at least 3.",
                        "無法再移除邊界點，至少需要保留三個。",
                        "无法再移除边界点，至少需要保留三个。",
                        "境界点をこれ以上削除できません。最低 3 つ必要です。"),
                    UiTranslate.T("Error removing boundary", "移除邊界錯誤", "移除边界错误", "境界削除エラー"),
                    MessageBoxButtons.OK);
                return;
            }

            var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(def);
            var newDef = def.RemoveBoundary();
            GlobalSettings.Settings.UpdateDefinition(
                GlobalSettings.CurrentDefinition.RemoveElement(def).AddElement(newDef, index), true);

            if (this.selectedDefinition != null) this.selectedDefinition = newDef;

            this.ResetBackBrushes();
        }

        #endregion Boundary management

        #region Properties

        private void mnuElementProperties_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            var relevantElement = this.selectedDefinition ?? this.elementUnderCursor;
            if (relevantElement == null) return;

            void OnDefinitionChanged(ElementDefinition def)
            {
                this.elementUnderCursor = null;
                this.currentlyManipulating = null;
                if (def.Equals(this.selectedDefinition))
                    this.selectedDefinition = def;

                var index = GlobalSettings.CurrentDefinition.Elements.IndexOf(def);
                GlobalSettings.Settings.UpdateDefinition(
                    GlobalSettings.CurrentDefinition.RemoveElement(def).AddElement(def, index),
                    false);

                this.ResetBackBrushes();
            }

            void OnDefinitionSaved()
            {
                GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition, true);
            }

            if (relevantElement is MouseKeyDefinition || relevantElement is MouseScrollDefinition)
            {
                using (var propertiesForm = new MouseElementPropertiesForm((KeyDefinition) relevantElement))
                {
                    propertiesForm.DefinitionChanged += OnDefinitionChanged;
                    propertiesForm.DefinitionSaved += OnDefinitionSaved;

                    this.ShowAppModalDialog(propertiesForm);
                    return;
                }
            }

            if (relevantElement is MouseSpeedIndicatorDefinition mouseSpeedElement)
            {
                using (var propertiesForm = new MouseSpeedPropertiesForm(mouseSpeedElement))
                {
                    propertiesForm.DefinitionChanged += OnDefinitionChanged;
                    propertiesForm.DefinitionSaved += OnDefinitionSaved;

                    this.ShowAppModalDialog(propertiesForm);
                    return;
                }
            }

            if (relevantElement is KeyboardKeyDefinition keyboardKeyElement)
            {
                using (var propertiesForm = new KeyboardKeyPropertiesForm(keyboardKeyElement))
                {
                    propertiesForm.DefinitionChanged += OnDefinitionChanged;
                    propertiesForm.DefinitionSaved += OnDefinitionSaved;

                    this.ShowAppModalDialog(propertiesForm);
                    return;
                }
            }

            throw new Exception("Unknown element, cannot open properties form.");
        }

        private void mnuKeyboardProperties_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            using (var propertiesForm = new KeyboardPropertiesForm(GlobalSettings.CurrentDefinition))
            {
                propertiesForm.DefinitionChanged += def =>
                {
                    this.elementUnderCursor = null;
                    this.currentlyManipulating = null;

                    GlobalSettings.Settings.UpdateDefinition(def, false);

                    this.ClientSize = new Size(def.Width, def.Height);

                    this.ResetBackBrushes();
                };

                propertiesForm.DefinitionSaved += () =>
                {
                    GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition, true);
                };

                this.ShowAppModalDialog(propertiesForm);
            }
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (!this.mnuToggleEditMode.Checked)
                return;

            if (this.IsLayeredOverlayActive)
            {
                this.SyncLayeredOverlayBounds();
                this.InvalidateKeyboardSurface();
                return;
            }

            if (GlobalSettings.CurrentDefinition == null)
                return;

            var client = this.ClientSize;
            if (client.Width <= 0 || client.Height <= 0)
                return;

            GlobalSettings.Settings.UpdateDefinition(GlobalSettings.CurrentDefinition.Resize(client), true);

            this.ResetBackBrushes();
        }

        #endregion Properties

        #region Styles

        private void mnuEditElementStyle_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            if (GlobalSettings.Settings.LoadedStyle == null)
            {
                this.ShowAppMessageBox(
                    UiTranslate.T(
                        "Please load or save a style before editing element styles.",
                        "請先載入或儲存樣式，再編輯元素樣式。",
                        "请先加载或保存样式，再编辑元素样式。",
                        "要素スタイルを編集する前に、スタイルを読み込むか保存してください。"),
                    UiTranslate.T("Style", "樣式", "样式", "スタイル"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var relevantElement = this.selectedDefinition ?? this.elementUnderCursor;
            if (relevantElement == null) return;
            var id = relevantElement.Id;

            if (relevantElement is KeyDefinition)
            {
                using (var styleForm = new KeyStyleForm(
                    GlobalSettings.CurrentStyle.TryGetElementStyle<KeyStyle>(id),
                    GlobalSettings.CurrentStyle.DefaultKeyStyle))
                {
                    styleForm.StyleChanged += style =>
                    {
                        if (style.Loose == null && style.Pressed == null)
                        {
                            if (GlobalSettings.CurrentStyle.ElementIsStyled(id))
                            {
                                GlobalSettings.Settings.UpdateStyle(
                                    GlobalSettings.CurrentStyle.RemoveElementStyle(id),
                                    false);
                            }
                        }
                        else
                        {
                            GlobalSettings.Settings.UpdateStyle(
                                GlobalSettings.CurrentStyle.SetElementStyle(id, style),
                                false);
                        }

                        this.ResetBackBrushes();
                    };

                    styleForm.StyleSaved += () =>
                    {
                        GlobalSettings.Settings.UpdateStyle(GlobalSettings.CurrentStyle, true);
                    };

                    this.ShowAppModalDialog(styleForm);
                }
            }

            if (relevantElement is MouseSpeedIndicatorDefinition)
            {
                using (var styleForm = new MouseSpeedStyleForm(
                    GlobalSettings.CurrentStyle.TryGetElementStyle<MouseSpeedIndicatorStyle>(id),
                    GlobalSettings.CurrentStyle.DefaultMouseSpeedIndicatorStyle))
                {
                    styleForm.StyleChanged += style =>
                    {
                        if (style == null)
                        {
                            if (GlobalSettings.CurrentStyle.ElementIsStyled(id))
                            {
                                GlobalSettings.Settings.UpdateStyle(
                                    GlobalSettings.CurrentStyle.RemoveElementStyle(id),
                                    false);
                            }
                        }
                        else
                        {
                            GlobalSettings.Settings.UpdateStyle(
                                GlobalSettings.CurrentStyle.SetElementStyle(id, style),
                                false);
                        }

                        this.ResetBackBrushes();
                    };

                    styleForm.StyleSaved += () =>
                    {
                        GlobalSettings.Settings.UpdateStyle(GlobalSettings.CurrentStyle, true);
                    };

                    this.ShowAppModalDialog(styleForm);
                }
            }
        }

        private void mnuEditKeyboardStyle_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            if (GlobalSettings.Settings.LoadedStyle == null)
            {
                this.ShowAppMessageBox(
                    UiTranslate.T(
                        "Please load or save a style before editing the keyboard style.",
                        "請先載入或儲存樣式，再編輯鍵盤樣式。",
                        "请先加载或保存样式，再编辑键盘样式。",
                        "キーボードスタイルを編集する前に、スタイルを読み込むか保存してください。"),
                    UiTranslate.T("Style", "樣式", "样式", "スタイル"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using (var styleForm = new KeyboardStyleForm(GlobalSettings.CurrentStyle))
            {
                styleForm.StyleChanged += style =>
                {
                    GlobalSettings.Settings.UpdateStyle(style, false);
                    this.ResetBackBrushes();
                };

                styleForm.StyleSaved += () => {
                    GlobalSettings.Settings.UpdateStyle(GlobalSettings.CurrentStyle, true);
                };
                this.ShowAppModalDialog(styleForm);
            }
        }

        private void mnuSaveStyleToName_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.CurrentStyle.Save(false);
            GlobalSettings.Settings.LoadedStyle = GlobalSettings.CurrentStyle.Name;
            GlobalSettings.Settings.LoadedGlobalStyle = false;
        }

        private void mnuSaveToGlobalStyleName_Click(object sender, EventArgs e)
        {
            this.menuOpen = false;

            GlobalSettings.CurrentStyle.Save(true);
            GlobalSettings.Settings.LoadedStyle = GlobalSettings.CurrentStyle.Name;
            GlobalSettings.Settings.LoadedGlobalStyle = true;
        }

        private void mnuSaveStyleAs_Click(object sender, EventArgs e)
        {
            using (var saveForm = new SaveStyleAsForm())
            {
                this.ShowAppModalDialog(saveForm);
                saveForm.Dispose();
            }
        }

        #endregion Styles
    }
}
