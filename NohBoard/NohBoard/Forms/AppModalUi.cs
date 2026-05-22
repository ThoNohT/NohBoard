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
    using System.Linq;
    using System.Windows.Forms;
    using ThoNohT.NohBoard.Hooking.Interop;

    internal static class AppModalUi
    {
        public static MainForm TryGetMainForm() =>
            Application.OpenForms.OfType<MainForm>().FirstOrDefault(f => !f.IsDisposed);

        public static DialogResult ShowDialog(Form dialog, IWin32Window owner = null)
        {
            var main = TryGetMainForm();
            main?.PrepareDialogAboveLayeredOverlay(dialog);

            return HookManager.RunModalUi(
                () => owner != null ? dialog.ShowDialog(owner) : dialog.ShowDialog());
        }

        public static DialogResult ShowCommonDialog(CommonDialog dialog, IWin32Window owner)
        {
            var main = TryGetMainForm();
            if (owner is Control control)
                main?.PrepareDialogAboveLayeredOverlay(control.FindForm());

            return HookManager.RunModalUi(() => dialog.ShowDialog(owner));
        }

        public static DialogResult ShowMessageBox(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon = MessageBoxIcon.None)
        {
            var main = TryGetMainForm();
            if (main != null && main.IsLayeredOverlayActive)
                return main.ShowAppMessageBox(text, caption, buttons, icon);

            return MessageBox.Show(owner, text, caption, buttons, icon);
        }
    }
}
