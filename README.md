# NohBoard

NohBoard is a keyboard visualization program. I know certain applications already exist that do just this, display your keyboard on-screen. And even more probably. However, so far I have found none that were both free and easy to use. That's where this program came in, I made it to be free and easy to use, without any fancy graphics, and easily capturable (possibly with chroma key). Furthermore, it's very customizable.

## Rewrite

An initial version was made in C++, this originated from the desire to make something with graphics, and what I knew was [OBS](http://github.com/jp9000/OBS), now replaced by [OBS Studio](http://github.com/jp9000/obs-studio). That's why I started in the same spirit, using C++, and rendering with DirectX. However, having spent most of my time on C# during at least the last decade or so, I decided that I would be much more able to create awesome things in this language. That's when I re-started. Rather than using DirectX, I switched to GDI+, as we're Windows only (I'm sorry, but I just really don't use any other OS, and so far it is still the go-to OS for gaming). No really fancy graphics are required, no 3D is required. This also makes it easier to capture, as a simple window capture in OBS will do the trick now, rather than having to fiddle with game capture which might not work due to a game typically being run at the same time as NohBoard.

## Transparency and borderless overlay

This fork adds an optional transparent, borderless overlay mode (branch `add-transparency-support`):

- **F1** toggles per-pixel transparency (the window uses a layered window, so the desktop shows through the background instead of rendering it black). **F1/F2 only work while the NohBoard window is focused** — they are not global hotkeys.
- **F2** toggles a borderless window (transparency implies borderless); drag the four corners to **resize while keeping the layout aspect ratio** (the scale is remembered in `NohBoard.json`). In borderless mode the window can be dragged with the left mouse button outside edit mode. The window is a normal, non-topmost window and keeps rendering while minimized.
- **Click-through**: while transparent and unfocused the mouse passes through the overlay; focus it (Alt+Tab / taskbar) to click and drag it again.
- Keyboard/mouse input is captured both by the low-level hooks and by Raw Input (`RegisterRawInputDevices`), so keys pressed in games and elevated (UAC) windows are picked up more reliably. For the few cases where even that is not enough (some elevated processes), the context menu offers **Restart as administrator**.
- Styles can request transparency explicitly: add `"Alpha": 0` (0–255, optional) to a style's `BackgroundColor`. A background with `Alpha < 255` starts transparent automatically. The mode is also remembered in `NohBoard.json` (`TransparentMode` / `BorderlessMode`).

See [docs/transparency-support.md](docs/transparency-support.md) and [docs/input-capture-and-hotkeys.md](docs/input-capture-and-hotkeys.md) (Chinese) for configuration examples, implementation details and test results.

## Contributors

**Maintainer / original author**
- Eric "ThoNohT" Bataille (e.c.p.bataille@gmail.com) - Original author

**Contributors**
- Marius "Buttercak3" Becker - Various bugfixes
- Ivan "YaLTeR" Molodetskikh - Added the scroll counter *(NohBoard classic)*
- Michal Mitter - Added button outline *(NohBoard classic)*

**Keyboard layouts**
- BaronBargy
- Burning Fish
- Cloudwolf
- Daigtas
- Floatingthru
- HAJohnny
- Helixia
- joao7yt
- kernel1337
- Krazy
- layarion
- MCCrafterTV
- MtB1980
- TicTacFoe
- ToxicMirror
- WayZHC
- wingsltd
- zolia
- SirDifferential
- flyingmongoose
- JapanYoshi
- dchitra

If you want to contribute, either with code, with keyboard definitions or keyboard styles, feel free to fork this repository and provide your changes via a pull request, or other means of submitting your changes back to me.

## Changelog

For the changelog, see the [Releases](https://github.com/ThoNohT/NohBoard/releases) page.

## Full Documentation

See the [Wiki](https://github.com/ThoNohT/NohBoard/wiki) for full documentation.

## Donations

Donations are neither required nor requested. They are, however, always appreciated, and due to some demand, there now is the possibility to [donate](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=FFB9XFRWE5EK2).
Note that donations are to be made purely for appreciation of performed work, and not as a means of prioritizing or requesting future work. They will not in any way impact the speed or order in which features are implemented.

## License

NohBoard is licensed under the GPL version 2. The license agreement is attached in this repository and can be found [here](https://github.com/ThoNohT/NohBoard/blob/master/LICENSE).
