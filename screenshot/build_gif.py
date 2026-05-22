"""One-shot helper: combine screenshot01..04.png into an animated GIF."""
from pathlib import Path

from PIL import Image

SCREENSHOT_DIR = Path(__file__).resolve().parent
OUT = SCREENSHOT_DIR / "screenshot.gif"


def main() -> None:
    paths = sorted(SCREENSHOT_DIR.glob("screenshot*.png"))
    # Exclude the output gif if present in pattern
    paths = [p for p in paths if p.suffix.lower() == ".png" and p.name.startswith("screenshot")]
    paths.sort(key=lambda p: p.name)
    if len(paths) != 4:
        raise SystemExit(f"Expected 4 screenshot*.png files, found {len(paths)}")

    frames_rgba = []
    for p in paths:
        im = Image.open(p)
        frames_rgba.append(im.convert("RGBA"))

    w = max(f.width for f in frames_rgba)
    h = max(f.height for f in frames_rgba)
    unified = []
    for f in frames_rgba:
        if f.size != (w, h):
            canvas = Image.new("RGBA", (w, h), (255, 255, 255, 255))
            canvas.paste(f, ((w - f.width) // 2, (h - f.height) // 2))
            unified.append(canvas)
        else:
            unified.append(f)

    unified[0].save(
        OUT,
        save_all=True,
        append_images=unified[1:],
        duration=1000,
        loop=0,
        disposal=2,
    )
    print(f"Wrote {OUT} ({w}x{h}, 4 frames @ 1000ms, infinite loop)")


if __name__ == "__main__":
    main()
